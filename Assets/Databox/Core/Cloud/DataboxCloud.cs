using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using Databox.FullSerializer;
using Databox.Utils;
using Databox.Dictionary;
using Databox.Ed;

// DATABOX CLOUD
// 
// Handles all cloud related methods.

namespace Databox
{
	public class DataboxCloud : MonoBehaviour
	{
		static public DataboxCloud instance;
		
		static DataboxObject database;
		
		static string serverRootURL = "";
		static string id = "";
		
		static string setDataPHP = "/setData.php";
		static string getDataPHP = "/getData.php";
		static string getVersionPHP = "/getVersion.php";
		static string getTimePHP = "/getTime.php";
		
		public static string serverVersion;
		public static string serverTime;
		
		public delegate void DataboxCloudEvents();
		public static DataboxCloudEvents OnDownloadComplete;
		public static DataboxCloudEvents OnDownloadFailed;
		public static DataboxCloudEvents OnUploadComplete;
		public static DataboxCloudEvents OnUploadFailed;
		
		public class ChangeLog
		{
			public List<string> tables;
			public List<string> entries;
			public List<string> fields;
			public List<string> values;
			
			public ChangeLog ()
			{
				tables = new List<string>();
				entries = new List<string>();
				fields = new List<string>();
				values = new List<string>();
			}
		}
		

		void Awake()
		{ 
			//called when an instance awakes in the game
			instance = this; //set our static reference to our newly initialized instance
		}
		
		public static void ForceDownloadRuntime(DataboxObject _database)
		{
			serverRootURL = _database.cloudServer;
			id = _database.cloudId;
			database = _database;
			
			instance.StartCoroutine(GetDataIE()); //this will launch the coroutine on our instance
		}
		
		public static void ForceUploadRuntime(DataboxObject _database)
		{
			serverRootURL = _database.cloudServer;
			id = _database.cloudId;
			database = _database;
			
			instance.StartCoroutine(ForceUploadIE());
		}
		
		public static void CheckRuntime(DataboxObject _database)
		{
			serverRootURL = _database.cloudServer;
			id = _database.cloudId;
			database = _database;
			
			instance.StartCoroutine(CheckIE());
		}
		
		public static void SyncRuntime(DataboxObject _database)
		{
			serverRootURL = _database.cloudServer;
			id = _database.cloudId;
			database = _database;
			
			instance.StartCoroutine(SyncIE());
		}
		
		
		public static void ForceDownloadEditor()
		{
			#if UNITY_EDITOR
			EditorCoroutines.Execute(GetDataIE());
			#endif
		}
		
		public static void ForceUploadEditor()
		{
			#if UNITY_EDITOR
			EditorCoroutines.Execute(ForceUploadIE());
			#endif
		}
		
		static IEnumerator ForceUploadIE()
		{
			yield return GetTimeIE();
			//Debug.Log(database.cloudVersion + " " + serverTime);
			
			serverVersion = serverTime;
			database.cloudVersion = serverTime;
			
			yield return UploadIE();
		}
	
		public static void Check(string _serverUrl, string _uniqueId, DataboxObject _databoxObject)
		{
			serverRootURL = _serverUrl;
			id = _uniqueId;
			database = _databoxObject;
			#if UNITY_EDITOR
			EditorCoroutines.Execute(CheckIE());
			#endif
		}
		
	
		
		static IEnumerator CheckIE()
		{
			DataboxEditor.deletedCompare = new ChangeLog();
			DataboxEditor.newCompare = new ChangeLog();
			
			database.cloudStatus = "";
			
			yield return GetTimeIE();
		
			if (!string.IsNullOrEmpty(serverTime))
			{
				// connection ok!
				database.cloudStatus = "- connection ok" + "\n";
				
				yield return GetVersionIE();
				
				if (!string.IsNullOrEmpty(serverVersion))
				{
					if (!string.IsNullOrEmpty(database.cloudVersion))
					{
						var _localVersion = double.Parse(database.cloudVersion);
						var _serverVersion = double.Parse(serverVersion);
						
						if (_localVersion < _serverVersion)
						{
							database.cloudStatus += "- local version is older (server version will be downloaded)" + "\n";
							database.cloudWarnings = DataboxObject.CloudWarnings.warning;
						}
						else if (_localVersion > _serverVersion)
						{
							database.cloudStatus += "- local version is newer" + "\n";			
							database.cloudWarnings = DataboxObject.CloudWarnings.ok;
						}
						else if (_localVersion == _serverVersion)
						{
							database.cloudStatus += "- local version is up to date" + "\n";
							database.cloudWarnings = DataboxObject.CloudWarnings.ok;
						}
					}
					else
					{
						database.cloudStatus += "- local version is older (server version will be downloaded)" + "\n";
						database.cloudWarnings = DataboxObject.CloudWarnings.warning;
					}
				}
				else
				{
					database.cloudStatus += "- local version is newer" + "\n";
					database.cloudWarnings = DataboxObject.CloudWarnings.ok;
				}
				
				database.cloudStatus += "- ready to sync" + "\n";
			}
			else
			{
				database.cloudStatus = "- connection error" + "\n";
				database.cloudWarnings = DataboxObject.CloudWarnings.error;
			}
		}
		

		public static void Sync(string _serverUrl, string _uniqueId, DataboxObject _databoxObject)
		{
			serverRootURL = _serverUrl;
			id = _uniqueId;
			database = _databoxObject;
			
			#if UNITY_EDITOR
			EditorCoroutines.Execute(SyncIE());
			#endif
		}
		
		static IEnumerator SyncIE()
		{
			DataboxEditor.deletedCompare = new ChangeLog();
			DataboxEditor.newCompare = new ChangeLog();
			
			database.SaveDatabase();
			
			database.cloudStatus = "";
			
			yield return GetVersionIE();
			
			// if local version is unknown or null
			// try get version from server
			if (string.IsNullOrEmpty(database.cloudVersion))
			{
				//Debug.Log("local version is null");
				if (string.IsNullOrEmpty(serverVersion))
				{
					
					//Debug.Log("server version is null");
					database.cloudStatus += "- upload to server" + "\n";
					
					// upload local to server
					yield return GetTimeIE();
					
					database.oldCloudVersion = database.cloudVersion;
					database.cloudVersion = serverTime;
					
					yield return UploadIE();
				}
				else
				{
					//Debug.Log("download from server");
					database.cloudStatus += "- download from server" + "\n";
					
					// download from server
					yield return GetDataIE();
				}
			}
			else
			{
				// if server version is unknown or null
				// get server time and upload data to server
				if (string.IsNullOrEmpty(serverVersion))
				{
					//Debug.Log("server version is null");
					yield return GetTimeIE();
					
					database.oldCloudVersion = database.cloudVersion;
					database.cloudVersion = serverTime;
					//Debug.Log(serverTime);
					
					yield return UploadIE();
				}
				else
				{
					var _localVersion = double.Parse(database.cloudVersion);
					var _serverVersion = double.Parse(serverVersion);
					
					// if version from server is greater, then
					// download data from server
					if (_localVersion < _serverVersion)
					{
						database.cloudStatus += "- local version is older" + "\n";
						//Debug.Log("server version greater");
						yield return GetDataIE();
					}
					// if local version is equal to server version
					// get new server time and upload data to server
					else if (_localVersion == _serverVersion)
					{
						
						//Debug.Log("local and server equal");
						database.cloudStatus += "- local version is equal to server" + "\n";
						
						yield return GetTimeIE();
						
						database.oldCloudVersion = database.cloudVersion;
						database.cloudVersion = serverTime;
						
						yield return UploadIE();
					
					}
					// if local version is greater, upload data
					// to server
					else if (_localVersion > _serverVersion )
					{
						//Debug.Log("local version is greater");
						database.cloudStatus += "- local version is newer" + "\n";
						yield return UploadIE();
					}
				}
			}
		
		}
		
		static IEnumerator GetTimeIE()
		{
			serverTime = "";
			
			WWWForm _form = new WWWForm();

			using (UnityWebRequest _download = UnityWebRequest.Post(serverRootURL + getTimePHP, _form))
			{
				yield return _download.SendWebRequest();
				
				while (_download.isDone == false)
					yield return null;
					
				if (_download.isNetworkError || _download.isHttpError)
				{
					Debug.Log(_download.error);
					database.cloudStatus += "- " + _download.error + "\n";
				}
				else
				{
					serverTime = _download.downloadHandler.text;
				}
			}
			
			// Obsolete
			//WWW _download = new WWW(serverRootURL + getTimePHP, _form);
			
			
			//yield return _download;
			
			//if (_download.error != null)
			//{
			//	Debug.Log(_download.error);
			//	database.cloudStatus += "- " + _download.error + "\n";
			//}
			//else
			//{
			//	Debug.Log(_download.text);
			//	serverTime = _download.text;
			//}
		}
		
		
		static IEnumerator GetVersionIE()
		{
			serverVersion = "";
			
			WWWForm _form = new WWWForm();
	
			_form.AddField("id", id);
			
			using (UnityWebRequest _download = UnityWebRequest.Post(serverRootURL + getVersionPHP, _form))
			{
				yield return _download.SendWebRequest();
				
				while (_download.isDone == false)
					yield return null;
				
				if (_download.isNetworkError || _download.isHttpError)
				{
					database.cloudStatus += "- " + _download.error + "\n";
				}
				else
				{
					
					serverVersion = _download.downloadHandler.text;

				}
			}
			
			
			// Obsolete
			//WWW _download = new WWW(serverRootURL + getVersionPHP, _form);
			
			
			//yield return _download;
			
			//if (_download.error != null)
			//{
			//	//Debug.Log(_download.error);
			//	database.cloudStatus += "- " + _download.error + "\n";
			//}
			//else
			//{
			//	//Debug.Log(_download.text);
			//	serverVersion = _download.text;
			//}
		}
		
		
		public static IEnumerator GetDataIE()
		{
			database.cloudProgress = 0f;
			
			WWWForm _form = new WWWForm();
	
			_form.AddField("id", id);
			
			
			using (UnityWebRequest _download = UnityWebRequest.Post(serverRootURL + getDataPHP, _form))
			{
				yield return _download.SendWebRequest();
				
				while (_download.isDone == false)
				{
					database.cloudStatus += ".";
					database.cloudProgress = _download.uploadProgress;
					//Debug.Log(_download.uploadProgress);
					yield return null;
				}
				
				database.cloudProgress = 1f;
				database.cloudStatus += "\n";
				
				if (_download.isNetworkError || _download.isHttpError)
				{
					Debug.LogWarning("Databox cloud: " + _download.error);
					database.cloudStatus += "- " + _download.error + "\n";
					
					if (OnDownloadFailed != null)
					{
						OnDownloadFailed();
					}
				}
				else
				{
					database.cloudStatus += "- download from server done" + "\n";
					database.oldDB = new OrderedDictionary<string, DataboxObject.Database>();
					database.oldDB = database.DeepCopy<OrderedDictionary<string, DataboxObject.Database>>(database.DB);
					
					database.DB = database.JsonToDB(_download.downloadHandler.text, serverVersion, true);
					
					// COMPARE to old DB
					DataboxEditor.deletedCompare = DataboxCloud.Compare(database.oldDB, database.DB);
					DataboxEditor.newCompare = DataboxCloud.Compare(database.DB, database.oldDB);
					
					if (OnDownloadComplete != null)
					{
						OnDownloadComplete();
					}
				}
			}
			
			// Obsolete
			//WWW _download = new WWW(serverRootURL + getDataPHP, _form);
			
			
			//yield return _download;
			
			//if (_download.error != null)
			//{
			//	//Debug.Log(_download.error);
			//	database.cloudStatus += "- " + _download.error + "\n";
			//}
			//else
			//{
			//	//Debug.Log(_download.text);
			//	database.cloudStatus += "- download from server" + "\n";
				
			//	database.JsonToDB(_download.text, serverVersion);
			//}
		}
		
		static IEnumerator UploadIE()
		{

			// First download server db to oldDB
			WWWForm _formDownload = new WWWForm();
	
			_formDownload.AddField("id", id);
			
			
			using (UnityWebRequest _download = UnityWebRequest.Post(serverRootURL + getDataPHP, _formDownload))
			{
				yield return _download.SendWebRequest();
				
				while (_download.isDone == false)
					yield return null;
					
				if (_download.isNetworkError || _download.isHttpError)
				{
					database.cloudStatus += "- " + _download.error + "\n";
					
					if (OnUploadFailed != null)
					{
						OnUploadFailed();
					}
				}
				else
				{
					//database.cloudStatus += "- download from server" + "\n";
					if (!string.IsNullOrEmpty(_download.downloadHandler.text))
					{
						database.oldDB = new OrderedDictionary<string, DataboxObject.Database>();
						database.oldDB = database.JsonToDB(_download.downloadHandler.text, serverVersion, false);
					}
					
					//// COMPARE to old DB
					DataboxEditor.newCompare = DataboxCloud.Compare(database.DB, database.oldDB);
					DataboxEditor.deletedCompare = DataboxCloud.Compare(database.oldDB, database.DB);
				}
			}
	

			// upload local db to server
			var _data = database.ReturnJson(database.DB);
				
			WWWForm _form = new WWWForm();
			//Debug.Log(_data);
		
			_form.AddField("id", id);
			_form.AddField("version", database.cloudVersion);
			_form.AddField("data", _data);
			
			using(UnityWebRequest _upload = UnityWebRequest.Post(serverRootURL + setDataPHP, _form))
			{
				yield return _upload.SendWebRequest();
				
				while (_upload.isDone == false)
				{
					database.cloudStatus += ".";
					yield return null;
				}
				
				database.cloudStatus += "\n";
				
				if (_upload.isNetworkError || _upload.isHttpError)
				{
					database.cloudStatus += "- " + _upload.error + "\n";
					
					
					if (OnUploadFailed != null)
					{
						OnUploadFailed();
					}
				}
				else
				{
					database.cloudStatus += "- upload successful" + "\n";
					
					if (OnUploadComplete != null)
					{
						OnUploadComplete();
					}
				}
			}
			
			// Obsolete
			//WWW _upload = new WWW(serverRootURL + setDataPHP, _form);
			//yield return _upload;
			
			
			//if (_upload.error != null)
			//{       
			//	database.cloudStatus += "- " + _upload.error + "\n";
			//}
			//else
			//{
			//	database.cloudStatus += "- upload successful" + "\n";
			//}
			
		}
		
		// Compare changed DB 
		public static ChangeLog Compare(OrderedDictionary<string, DataboxObject.Database> _oldDB, OrderedDictionary<string, DataboxObject.Database> _newDB)
		{
			var _modifiedTables = new List<string>();
			var _modifiedEntries = new List<string>();
			var _modifiedFields = new List<string>();
			var _modifiedValues = new List<string>();
			
			foreach(var db in _oldDB.Keys)
			{	
				
					DataboxObject.Database _existingDB = null;
					if (_newDB.TryGetValue(db, out _existingDB ))
					{
						foreach(var entry in _oldDB[db].entries.Keys)
						{
							DataboxObject.DatabaseEntry _existingEntry = null; 
							if (_newDB[db].entries.TryGetValue(entry, out _existingEntry ))
							{
								foreach(var value in _oldDB[db].entries[entry].data.Keys)
								{
									Dictionary<System.Type, DataboxType> _existingValue = null;
									if (_newDB[db].entries[entry].data.TryGetValue(value, out _existingValue))
									{										
										// Compare values
										for (int i = 0; i < DataboxEditor.allTypes.Count; i ++)
										{
											DataboxType _t = null;
											if (_newDB[db].entries[entry].data[value].TryGetValue(System.Type.GetType(DataboxEditor.allTypes[i]), out _t))
											{
												DataboxType _e = null;
												if (_oldDB[db].entries[entry].data[value].TryGetValue(System.Type.GetType(DataboxEditor.allTypes[i]), out _e))
												{
													var _results = _t.Equal(_e);
													
													if (!string.IsNullOrEmpty(_results))
													{
														_modifiedValues.Add(db + " -> " + entry + " -> " + value + " ( " + _results + " ) ");
													}
												}
											}
										}
													
									}
									else
									{
										_modifiedFields.Add(db + " -> " + entry + " -> " + value);
									}
								}
							}
							else
							{			
								_modifiedEntries.Add(db + " -> " + entry);
							}
						}
					}
					else
					{
						_modifiedTables.Add(db);
					}
			}
			
			var _m = new ChangeLog();
			_m.tables = _modifiedTables;
			_m.entries = _modifiedEntries;
			_m.fields = _modifiedFields;
			_m.values = _modifiedValues;
			
			return _m;
		}
	}
}