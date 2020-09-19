using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading.Tasks;


#if NET_4_6
using Databox.OdinSerializer;
#endif
using Databox.FullSerializer;
using Databox.Dictionary;
using Databox.Ed;
using Databox.Utils;

// DATABOX (c) 2019 by doorfortyfour
// 
// Version 1.1.2p2
//
// The databox object handles all relevant data modification methods and also stores the complete database.
// If you need any support please head over to the official documentation:
 
// DOCUMENTATION: http://databox.doorfortyfour.com/documentation
// WEBSITE: http://databox.doorfortyfour.com


namespace Databox
{

	[System.Serializable]
	[CreateAssetMenu(menuName = "Databox/New Databox Object")]
	public class DataboxObject : ScriptableObject {
		
		/////////////////////////////////////////////////////////////////////
		// DATABASE STRUCTURE  //////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////////
		//-------------------------------------------------------------------
		
		public OrderedDictionary<string, Database> DB = new OrderedDictionary<string, Database>();
		// old DB for cloud comparison
		public OrderedDictionary<string, Database> oldDB = new OrderedDictionary<string, Database>();
		
		public class Database
		{
			public OrderedDictionary<string, DatabaseEntry> entries = new OrderedDictionary<string, DatabaseEntry>();
			public Database(){ entries = new OrderedDictionary<string, DatabaseEntry>();}
		}
		
		public class DatabaseEntry
		{
			public bool selected = false;
			
			public Dictionary<string, Dictionary<System.Type, DataboxType>> data = new Dictionary<string, Dictionary<System.Type, DataboxType>>();
			
			public DatabaseEntry (){ data = new Dictionary<string, Dictionary<System.Type, DataboxType>>(); }
			public DatabaseEntry (Dictionary<string, Dictionary<System.Type, DataboxType>> _data)
			{
				data = new Dictionary<string, Dictionary<System.Type, DataboxType>>();
				foreach (var d in _data.Keys)
				{
					var _d = new Dictionary<System.Type, DataboxType>();
					
					foreach(var e in _data[d].Keys)
					{
						
						var _a = new DataboxType();
					
						_d.Add(_data[d][e].GetType(), _a);
						
					}
					
					data.Add(d, _d);
				}
			}
			
		}
	
		/////////////////////////////////////////////////////////////////////
		
		
		// EVENTS
		public delegate void DataboxEvents();
		public DataboxEvents OnDatabaseLoaded;
		public DataboxEvents OnDatabaseSaving;
		public DataboxEvents OnDatabaseSaved;
		public DataboxEvents OnDatabaseCloudDownloaded;
		public DataboxEvents OnDatabaseCloudDownloadFailed;
		public DataboxEvents OnDatabaseCloudUploaded;
		public DataboxEvents OnDatabaseCloudUploadFailed;
		public DataboxEvents OnImportFromGoogleComplete;
		/// ///////////////
		
		
		// PROPERTIES
		/// ////////////////
		public DataboxCloud databoxCloudService;
		
		// Cloud
		[SerializeField]
		public string cloudVersion;
		[SerializeField]
		public string oldCloudVersion;
		[SerializeField]
		public string cloudId;
		[SerializeField]
		public string cloudServer;
		public string cloudStatus;
		public float cloudProgress;
		
		// CONFIG
		///////////////////
		[SerializeField]
		public string savePath;
		public string fileName;
		public int selectedApplicationPath;
		
		[SerializeField]
		public bool debugMode;
		
		[SerializeField]
		public bool showFieldTypes;
		
		[SerializeField]
		public bool showInitValues;
	
		[SerializeField]
		public bool automaticSave = false;
	
	
		// FULL SERIALIZER CONFIGS
		/////////////////////////
		[SerializeField]
		public bool encryption = false;
	
		[SerializeField]
		public int encryptionKey = 123456;
		
		[SerializeField]
		public bool compressJson = false;
		/////////////////////////
		
		// ODIN SERIALIZER CONFIGS
		//////////////////////////
		#if NET_4_6
		public enum OdinDataFormat
		{
			binary,
			json_legacy,
			json
		}
		
		[SerializeField]
		public OdinDataFormat dataFormat;
		#endif
		//////////////////////////
		

		[System.NonSerialized]
		public bool databaseLoaded = false;
		
		[SerializeField]
		public bool collapsedView;
		
		[SerializeField]
		public bool showInspector = true;
		
		[SerializeField]
		public float entriesPerPage = 99f;
		
		// GOOGLE SHEET CONFIG
		[SerializeField]
		public string googleSheetUrl = "";
		
		[System.Serializable]
		public class GoogleWorksheet
		{
			public string name;
			public string id;
		}
		public List<GoogleWorksheet> googleWorksheets = new List<GoogleWorksheet>();
		
		public enum Serializer
		{
			FullSerializer,
			OdinSerializer
		}
		
		[SerializeField]
		public Serializer serializer;
		
		public enum ErrorType
		{
			None,
			NoDirectory,
			NoFileName,
			UnsavedChanges,
			KeyAlreadyExists
		}
		
		public ErrorType errors;
		
		public enum CloudWarnings
		{
			neutral,
			ok,
			warning,
			error
		}
		
		public CloudWarnings cloudWarnings;
		
		public bool cloudCheck = false;
		
		public void OnEnable()
		{
			#if UNITY_EDITOR
			LoadDatabase(false);
			#endif		
		}
		

		/// <summary>
		/// Add Data of type DataboxType to the Databox object
		/// </summary>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <param name="_data"></param>
		/// <returns></returns>
		public bool AddData(string _tableID, string _entryID, string _valueID, DataboxType _data)
		{
			Database _tempDB;
			if (!DB.TryGetValue(_tableID, out _tempDB))
			{
				DB.Add(_tableID, new Database());
			}
			
			DatabaseEntry _tempEntry;
			if (!DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
			{
				DB[_tableID].entries.Add(_entryID, new DatabaseEntry());
			}
			
			Dictionary<System.Type, DataboxType> _tempData;
			if (!DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _tempData))
			{
				Dictionary<System.Type, DataboxType> _tempValue = new Dictionary<System.Type, DataboxType>();
				_tempValue.Add(_data.GetType(), _data);
				DB[_tableID].entries[_entryID].data.Add(_valueID, _tempValue);
			}
			

			//DB.entry.Add(_entryID, new DatabaseEntry());
			//Dictionary<System.Type, A> _temp = new Dictionary<System.Type, A>();
			//_temp.Add(_data.GetType(), _data);
			//DB.entry[_entryID].data.Add(_dataID, _temp);
			
			return true;
		}
		
		/// <summary>
		/// SetData sets a value to an existing entry/value. Returns true if succeeded
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <param name="_value"></param>
		/// <returns></returns>
		public bool SetData<T>(string _tableID, string _entryID, string _valueID, DataboxType _value) where T : DataboxType
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			DataboxType _data;
			Dictionary<System.Type, DataboxType> _tempData;
			
			if (DB.TryGetValue(_tableID, out _tempDB))
			{	
				if (DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				{
					if (DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _tempData))
					{
						if (_tempData.TryGetValue(typeof(T), out _data))
						{
							_tempData[typeof(T)] = (T)_value;
							return true;
						}
					}
				}
			}
			
			return false;
		}
		
		
		/// <summary>
		/// Get Data returns a value of type DatabaseType.
		/// _addDataIfNotExist: if true, Databox adds a new entry if the data doesn't exist.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <param name="_addDataIfNotExist"></param>
		/// <returns></returns>
		public T GetData<T>(string _tableID, string _entryID, string _valueID, bool _addDataIfNotExist) where T : DataboxType
		{
			return GetDataInternal<T>(_tableID, _entryID, _valueID, _addDataIfNotExist);
		}
		
		/// <summary>
		/// Get Data returns a value of type DatabaseType
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_tableID">Table ID of Database value</param>
		/// <param name="_entryID">Entry ID of Database value</param>
		/// <param name="_valueID">Value ID of Database value</param>
		/// <returns></returns>
		public T GetData<T>(string _tableID, string _entryID, string _valueID) where T : DataboxType
		{
			return GetDataInternal<T>(_tableID, _entryID, _valueID, true);
		}
		
		
		T GetDataInternal<T>(string _tableID, string _entryID, string _valueID, bool _addDataIfNotExist) where T : DataboxType
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			Dictionary<System.Type, DataboxType> _temp;
			DataboxType _data;
			
			if (!DB.TryGetValue(_tableID, out _tempDB))
				return null;
			
			if (!DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				return null;
			
			if (DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _temp))
			{
				if (_temp.TryGetValue(typeof(T), out _data))
				{
					return (T)_data;
				}
			}
			else
			{
				if (_addDataIfNotExist)
				{
					Debug.LogWarning("Databox: Data value " + _tableID + " / " + _entryID + " / " + _valueID + " does not exist, adding new value");
					var _instance = System.Activator.CreateInstance((typeof(T)));
					AddData(_tableID, _entryID, _valueID, _instance as DataboxType);
				
					return (T)_instance;
				}
				
			}
			
			return null;
		}
		
		/// <summary>
		/// Trys to get the data of a specific type, returns true or false if succeeded.
		/// _addDataIfNotExist: if true, Databox adds a new entry if the data doesn't exist.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <param name="_addDataIfNotExist"></param>
		/// <param name="_data"></param>
		/// <returns></returns>
		public bool TryGetData<T>(string _tableID, string _entryID, string _valueID, bool _addDataIfNotExist, out T _data) where T : DataboxType
		{
			return TryGetDataInternal<T>(_tableID, _entryID, _valueID, _addDataIfNotExist, out _data);
		}
		
		/// <summary>
		/// Trys to get the data of a specific type, returns true or false if succeeded.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <param name="_data"></param>
		/// <returns></returns>
		public bool TryGetData<T>(string _tableID, string _entryID, string _valueID, out T _data) where T : DataboxType
		{
			return TryGetDataInternal<T>(_tableID, _entryID, _valueID, true, out _data);
		}
		
		bool TryGetDataInternal<T>(string _tableID, string _entryID, string _valueID, bool _addDataIfNotExist, out T _data) where T : DataboxType
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			Dictionary<System.Type, DataboxType> _temp;
			DataboxType _dataType;
			_data = null;
			
			if (!DB.TryGetValue(_tableID, out _tempDB))		
				return false;
			
			if (!DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				return false;
			
			if (DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _temp))
			{
				if (_temp.TryGetValue(typeof(T), out _dataType))
				{
					_data = (T)_dataType;
					return true;
				}
			}
			else
			{
				if (_addDataIfNotExist)
				{
					Debug.LogWarning("Databox: Data value " + _tableID + " / " + _entryID + " / " + _valueID + " does not exist, adding new value");
					var _instance = System.Activator.CreateInstance((typeof(T)));
					AddData(_tableID, _entryID, _valueID, _instance as DataboxType);
					
					_data = (T)_instance;
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Used by DatabaseUIBinding script as it don't know what type of DatabaseType to retrieve.
		/// </summary>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		/// <returns></returns>
		public object GetDataUnknown(string _tableID, string _entryID, string _valueID)
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			Dictionary<System.Type, DataboxType> _temp;
			DataboxType _data;
			
			if (!DB.TryGetValue(_tableID, out _tempDB))
				return null;
			
			if (!DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				return null;
			
			if (DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _temp))
			{
				for (int i = 0; i < DataboxEditor.allTypes.Count; i ++)
				{
					if (_temp.TryGetValue(System.Type.GetType(DataboxEditor.allTypes[i]), out _data))
					{
						return _data;
					}
				}
			}
			
			return null;
		}
		
		
		/// <summary>
		/// Returns a dictionary with all entries from a table
		/// </summary>
		/// <param name="_fromTable"></param>
		/// <returns></returns>
		public OrderedDictionary<string, DatabaseEntry> GetEntriesFromTable(string _fromTable)
		{
			return DB[_fromTable].entries;
		}
		
		/// <summary>
		/// Returns a dictionary with all values from an entry in a table
		/// </summary>
		/// <param name="_fromTable"></param>
		/// <param name="_fromEntry"></param>
		/// <returns></returns>
		public Dictionary<string, Dictionary<Type, DataboxType>> GetValuesFromEntry(string _fromTable, string _fromEntry)
		{
			return DB[_fromTable].entries[_fromEntry].data;
		}

		
		
		/// <summary>
		/// Checks if an entry in a databox object exists. Returns true or false
		/// </summary>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <returns></returns>
		public bool EntryExists(string _tableID, string _entryID)
		{
			if (DB.ContainsKey(_tableID))
			{
				if (DB[_tableID].entries.ContainsKey(_entryID))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Checks if a value in a databox object exists. Returns true or false
		/// </summary>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <returns></returns>
		public bool ValueExists(string _tableID, string _entryID, string _valueID)
		{
			if (DB.ContainsKey(_tableID))
			{
				if (DB[_tableID].entries.ContainsKey(_entryID))
				{
					if (DB[_tableID].entries[_entryID].data.ContainsKey(_valueID))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
	
		/// <summary>
		/// Register an Object to a new Database. Used to have a init Database and a Runtime Database. 
		/// The Runtime database can then be saved as a save file.
		/// </summary>
		/// <param name="_dbToRegister"></param>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		/// <param name="_newEntryID"></param>
		/// <returns></returns>
		public bool RegisterToDatabase(DataboxObject _dbToRegister, string _tableID, string _entryID, string _newEntryID)
		{
			Database _tempDB;
			if (!DB.TryGetValue(_tableID, out _tempDB))
			{
				Debug.LogWarning("DB not found " + _tableID);
				return false;
			}
			else
			{
				if (!_dbToRegister.DB.TryGetValue(_tableID, out _tempDB))
				{
					_dbToRegister.DB.Add(_tableID, new Database());
				}
				
				DatabaseEntry _entry;
				if (DB[_tableID].entries.TryGetValue(_entryID, out _entry))
				{
					//Debug.Log(_tableID + " " + _newEntryID);
					DatabaseEntry _newEntry;
					if (!_dbToRegister.DB[_tableID].entries.TryGetValue(_newEntryID, out _newEntry))
					{
						_dbToRegister.DB[_tableID].entries.Add(_newEntryID, new DatabaseEntry());
						
						foreach (var d in DB[_tableID].entries[_entryID].data.Keys)
						{
							Dictionary<System.Type, DataboxType> _dc = new Dictionary<Type, DataboxType>();
							
							_dc = DeepCopy<Dictionary<System.Type, DataboxType>>(DB[_tableID].entries[_entryID].data[d]);
							//foreach(var e in DB[_tableID].entry[_entryID].data[d].Keys)
							//{
							//	//var a = new DataboxType();
							//	var cc = DeepCopy(DB[_tableID].entry[_entryID].data[d][e]); //DB[_tableID].entry[_entryID].data[d][e];
							//	_dc.Add(cc.GetType(), cc);
							//}
							
							_dbToRegister.DB[_tableID].entries[_newEntryID].data.Add(d, _dc);
						}
					}
					
					return true;
				}
				else
				{
					Debug.LogWarning("Entry " + _entryID + " not found");
					return false;
				}
			}
			
			
		}

		/// <summary>
		/// Add table
		/// </summary>
		/// <param name="_dbTableName"></param>
		public void AddDatabaseTable(string _dbTableName)
		{
			Database _tempDB;
			if (DB == null)
			{
				DB = new OrderedDictionary<string, Database>();
			}
			
			if (!DB.TryGetValue(_dbTableName, out _tempDB))
			{
				DB.Add(_dbTableName, new Database());
			}
		}
		
		/// <summary>
		/// Remove table
		/// </summary>
		/// <param name="_dbTableName"></param>
		public void RemoveDatabaseTable(string _dbTableName)
		{
			Database _tempDB;
			
			if (DB.TryGetValue(_dbTableName, out _tempDB))
			{
				DB.Remove(_dbTableName);
			}
		}
		
		/// <summary>
		/// Remove complete entry
		/// </summary>
		/// <param name="_tableID"></param>
		/// <param name="_entryID"></param>
		public void RemoveEntry(string _tableID, string _entryID)
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			if (DB.TryGetValue(_tableID, out _tempDB))
			{
				if (DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				{
					DB[_tableID].entries.Remove(_entryID);
				}
			}
		}
		
		/// <summary>
		/// Remove value from entry
		/// </summary>
		/// <param name="_dbName"></param>
		/// <param name="_entryID"></param>
		/// <param name="_valueID"></param>
		public void RemoveValue(string _tableID, string _entryID, string _valueID)
		{
			Database _tempDB;
			DatabaseEntry _tempEntry;
			Dictionary <System.Type, DataboxType> _tempData;
			
			if (DB.TryGetValue(_tableID, out _tempDB))
			{
				if (DB[_tableID].entries.TryGetValue(_entryID, out _tempEntry))
				{
					if (DB[_tableID].entries[_entryID].data.TryGetValue(_valueID, out _tempData))
					{
						DB[_tableID].entries[_entryID].data.Remove(_valueID);
					}
				}
			}
		}
		
		// Rename Value
		public void RenameValue(string _fromTable, string _fromEntries, string fromKey, string toKey)
		{
			var dic = DB[_fromTable].entries[_fromEntries].data;
			
			var value =  dic[fromKey];
			dic.Remove(fromKey);
			dic[toKey] = value;
		}
		
		/// <summary>
		/// Clear complete database
		/// </summary>
		public void ClearDatabase()
		{
			DB = new OrderedDictionary<string, Database>();
		}
		
		/// <summary>
		/// Reset table to initial values
		/// </summary>
		/// <param name="_table"></param>
		public void ResetToInitValues(string _table)
		{
			foreach(var entry in DB[_table].entries.Keys)
			{
				foreach (var data in DB[_table].entries[entry].data.Keys)
				{
					Dictionary <System.Type, DataboxType> _tempData;
					if (DB[_table].entries[entry].data.TryGetValue(data, out _tempData))
					{
						for (int i = 0; i < DataboxEditor.allTypes.Count; i ++)
						{
							DataboxType _data;
							if (_tempData.TryGetValue(System.Type.GetType(DataboxEditor.allTypes[i]), out _data))
							{
								_data.Reset();
							}
						}
					}
				}
			}

		}
		
		// OBSOLETE
		/// <summary>
		/// Should only be called by the Databox editor script
		/// </summary>
		//public void SaveDatabaseEditor()
		//{
		//	if (automaticSave && !Application.isPlaying)
		//	{
		//		SaveDatabase();
		//	}
		//}
		
		/// <summary>
		/// Save database asynchronous
		/// use with StartCoroutine();
		/// </summary>
		public IEnumerator SaveDatabaseAsync()
		{
			Task saveTask = new Task(() => { SaveDatabaseAsyncInternal(fileName);}); //.Start();
			saveTask.Start();
			
			while (!saveTask.IsCompleted)
			{
				yield return null;
			}
		
			if (OnDatabaseSaved != null && Application.isPlaying)
			{
				OnDatabaseSaved();
			}
		}
		
		/// <summary>
		/// Save Database with custom file name and path. It still uses the selected Application path from the config menu
		/// use with StartCoroutine();
		/// </summary>
		/// <param name="_fileName"></param>
		/// <returns></returns>
		public IEnumerator SaveDatabaseAsync(string _fileName)
		{
			Task saveTask = new Task(() => { SaveDatabaseAsyncInternal(_fileName);}); //.Start();
			saveTask.Start();
			
			while (saveTask.IsCompleted)
			{
				yield return null;
			}
		
			if (OnDatabaseSaved != null && Application.isPlaying)
			{
				OnDatabaseSaved();
			}
		}
		
		// Async saving
		async void SaveDatabaseAsyncInternal(string _fileName)
		{		
			await Task.Run(() =>
			{
				SaveDatabaseInternal(false, _fileName);
			});
		}

		/// <summary>
		/// Save Database with current configuration settings
		/// </summary>
		public void SaveDatabase()
		{
			SaveDatabaseInternal(true, fileName);
		}
		
		/// <summary>
		/// Save Database with custom file name and path. It still uses the selected Application path from the config menu
		/// </summary>
		/// <param name="_fileName"></param>
		public void SaveDatabase(string _fileName)
		{
			SaveDatabaseInternal(true, _fileName);
		}
		
		void SaveDatabaseInternal(bool _callEvent, string _fileName)
		{
			
			var _savePath = ReturnSavePath(_fileName);
			
			if (selectedApplicationPath < 3)
			{
				if (string.IsNullOrEmpty(_savePath))
				{
					errors = ErrorType.NoDirectory;
					return;
				}
			
				if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_savePath) ))
				{
					errors = ErrorType.NoDirectory;
					return;
				}
				
				if (string.IsNullOrEmpty(_fileName))
				{
					errors = ErrorType.NoFileName;
					return;
				}
			}
			
			errors = ErrorType.None;
			
			
			switch(serializer)
			{
				case Serializer.FullSerializer:
					SaveWithFullSerializer(_savePath, _callEvent);
					break;
				case Serializer.OdinSerializer:
#if NET_4_6
					SaveWithOdinSerializer(_savePath, _callEvent);
#endif
					break;
			}
			
		}
		
#if NET_4_6
		void SaveWithOdinSerializer(string _savePath, bool _callEvent)
		{
			DataFormat _format = DataFormat.Binary;
			bool _isJsonLegacy = false;
			
			switch (dataFormat)
			{
				case OdinDataFormat.binary:
					_format = DataFormat.Binary;
					break;
				case OdinDataFormat.json_legacy:
					_isJsonLegacy = true;
					_format = DataFormat.JSON;
					break;
				case OdinDataFormat.json:
					_format = DataFormat.JSON;
					break;
			}
			
			byte[] bytes = SerializationUtility.SerializeValue(DB, _format);
			
		
			
			//encrypt to XOR
			if (encryption)
			{
				//string base64 = Convert.ToBase64String(bytes);
				var _j = ByteXOREncryptDecrypt(bytes, encryptionKey);
				//bytes = Convert.FromBase64String(_j);
				bytes = _j;
			}
			
			if (selectedApplicationPath < 3)
			{
				if (selectedApplicationPath == 1 && !Application.isEditor && Application.platform == RuntimePlatform.Android)
				{
					Debug.LogWarning("Databox: Saving to Streaming Assets folder is not supported on this platform");
					return;
				}
				else if (selectedApplicationPath == 2 && !Application.isEditor)
				{
					Debug.LogWarning("Databox: Saving to Resources folder during runtime is not supported");
					return;
				}
				else
				{
					//File.WriteAllBytes(_savePath, bytes);
					
					string _saveString = "";
					if (_format == DataFormat.JSON && !_isJsonLegacy)
					{
						_saveString = System.Text.Encoding.UTF8.GetString(bytes);
					}
					else
					{
						_saveString = Convert.ToBase64String(bytes);
					}
				
					
					
					//System.IO.File.WriteAllText(_savePath, _j);
					
					//using (var fs = new FileStream(_savePath, FileMode.Create, FileAccess.Write, FileShare.Write))
					////using (var writer = new StringWriter(fs))
					//{
					//	//writer.Write(_string);
					//	fs.Write(_string);
					//}
					
					System.IO.File.WriteAllText(_savePath, _saveString);
				}
			}
			else
			{
				// save to player prefs
				PlayerPrefs.SetString(_savePath, Convert.ToBase64String(bytes));
			}
			
			
			if (debugMode)
			{
				Debug.Log("Databox: Database saved");
			}
			
		
			if (_callEvent && OnDatabaseSaved != null && Application.isPlaying)
			{
				OnDatabaseSaved();
			}
			
		}
#endif

		void SaveWithFullSerializer(string _savePath, bool _callEvent)
		{
			//full serializer
			fsSerializer _serializer = new fsSerializer();
			fsData data;
			_serializer.TrySerialize( DB, out data);
			
			// emit the data via JSON
			string _j = "";
			if (!compressJson)
			{
				_j = fsJsonPrinter.PrettyJson(data);
			}
			else
			{
				_j = fsJsonPrinter.CompressedJson(data);
			}
			
			//encrypt to XOR
			if (encryption)
			{
				_j = XOREncryptDecrypt(_j, encryptionKey);
			}
			
			if (selectedApplicationPath < 3)
			{
				if (selectedApplicationPath == 1 && !Application.isEditor && Application.platform == RuntimePlatform.Android)
				{
					Debug.LogWarning("Databox: Saving to Streaming Assets folder is not supported on this platform");
					return;
				}
				else if (selectedApplicationPath == 2 && !Application.isEditor)
				{
					Debug.LogWarning("Databox: Saving to Resources folder during runtime is not supported");
					return;
				}
				else
				{
					System.IO.File.WriteAllText(_savePath, _j);
				}
			}
			else
			{
				// save to player prefs
				PlayerPrefs.SetString(_savePath, _j);
			}
			
			if (debugMode)
			{
				Debug.Log("Databox: Database saved");
			}
			
			if (_callEvent && OnDatabaseSaved != null && Application.isPlaying)
			{
				OnDatabaseSaved();
			}
			
		}
		
		
		/// <summary>
		/// Load database asynchronous
		/// use with StartCoroutine();
		/// </summary>
		public IEnumerator LoadDatabaseAsync()
		{
			Task loadTask =	new Task(() => { LoadDatabaseAsyncInternal(fileName);});
			loadTask.Start();
			
			while (!loadTask.IsCompleted)
			{
				yield return null;
			}
			
			if (OnDatabaseLoaded != null && Application.isPlaying)
			{
				OnDatabaseLoaded();
			}
		}
		
		/// <summary>
		/// Load database asynchronous with custom file name. 
		/// use with StartCoroutine();
		/// </summary>
		/// <param name="_fileName"></param>
		public IEnumerator LoadDatabaseAsync(string _fileName)
		{
			Task loadTask =	new Task(() => { LoadDatabaseAsyncInternal(_fileName);});
			loadTask.Start();
			
			while (!loadTask.IsCompleted)
			{
				yield return null;
			}
			
			if (OnDatabaseLoaded != null && Application.isPlaying)
			{
				OnDatabaseLoaded();
			}
		}
		
		
		async void LoadDatabaseAsyncInternal( string _fileName)
		{
			await Task.Run(() =>
			{
				LoadDatabaseInternal(false, _fileName);
			});
		}
		
		
		/// <summary>
		/// Load database
		/// </summary>
		public void LoadDatabase()
		{
			LoadDatabaseInternal(true, fileName);
		}
		
		/// <summary>
		/// Load database with custom filename/path
		/// </summary>
		/// <param name="_fileName"></param>
		public void LoadDatabase(string _fileName)
		{
			LoadDatabaseInternal(true, _fileName);
		}
		
		/// <summary>
		/// Load database. Set call event to true or false whether you want to 
		/// call OnDatabaseLoaded event or not.
		/// </summary>
		/// <param name="_callEvent"></param>
		public void LoadDatabase(bool _callEvent)
		{
			LoadDatabaseInternal(_callEvent, fileName);
		}
		
		void LoadDatabaseInternal(bool _callEvent, string _fileName)
		{
			var _savePath = ReturnSavePath(_fileName);
		
			if (string.IsNullOrEmpty(_savePath))
			{
				if (debugMode)
				{
					Debug.LogWarning ("Databox: Save path is empty");
				}
				
				return;
			}
			
			
			if (selectedApplicationPath < 2 && Application.isEditor && (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.WebGLPlayer))
			{
				if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(_savePath)))
				{
					errors = ErrorType.NoDirectory;
					return;
				}
				
				errors = ErrorType.None;
				
				if (!System.IO.File.Exists(_savePath))
				{
					return;
				}
			}
			
			
			DataboxEditor.GetDataTypes(out DataboxEditor.allTypes, out DataboxEditor.allTypesNames);
			
			switch(serializer)
			{
				case Serializer.FullSerializer:
					try
					{
						LoadWithFullSerializer(_savePath, _callEvent);
					}
					catch{}
					break;
				case Serializer.OdinSerializer:
#if NET_4_6
					try
					{
						LoadWithOdinSerializer(_savePath, _callEvent);
					}
					catch{}
#endif
					break;
			}
		
		}
		
#if NET_4_6
		void LoadWithOdinSerializer(string _savePath, bool _callEvent)
		{

			DataFormat _format = DataFormat.Binary;
			bool _isJsonLegacy = false;
			switch (dataFormat)
			{
			case OdinDataFormat.binary:
				_format = DataFormat.Binary;
				break;
			case OdinDataFormat.json_legacy:
				_isJsonLegacy = true;
				_format = DataFormat.JSON;
				break;
			case OdinDataFormat.json:
				_format = DataFormat.JSON;
				break;
			}
			
			
			byte[] bytes = new byte[1];
			bool _alreadySerialized = false;
			if (selectedApplicationPath < 2)
			{
				if (selectedApplicationPath == 1 && !Application.isEditor && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer))
				{

					DataboxLoadCoroutine.Instance.LoadCoroutine(_savePath, this, _callEvent);
					return;						
				}
				else
				{
					//bytes = File.ReadAllBytes(_savePath);
					var _s = File.ReadAllText(_savePath);
					
					if (_format == DataFormat.JSON && !_isJsonLegacy)
					{
						bytes = System.Text.Encoding.UTF8.GetBytes(_s);
					}
					else
					{
						bytes = Convert.FromBase64String(_s);
					}
				
					if (encryption)
					{
						bytes = ByteXOREncryptDecrypt(bytes, encryptionKey);
					}
				
					//using (var fs = new FileStream(_savePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					//{
					//	using (var reader = new BinaryReader(fs))
					//	{
					//		DB = SerializationUtility.DeserializeValue<OrderedDictionary<string, Database>>(reader.BaseStream, _format);
					//	}
					//}
					
					Stream _stream = new MemoryStream(bytes);

					using (var reader = new BinaryReader(_stream))
					{
						DB = SerializationUtility.DeserializeValue<OrderedDictionary<string, Database>>( reader.ReadBytes((int)_stream.Length), _format);
					}
					
					_alreadySerialized = true;
					
				}
			}
			else if (selectedApplicationPath == 2)
			{
				if (!Application.isEditor)
				{
					// file is in resources folder
					var _s = Resources.Load<TextAsset>(_savePath).text;
					
					if (_format == DataFormat.JSON && !_isJsonLegacy)
					{
						bytes = System.Text.Encoding.UTF8.GetBytes(_s);
					}
					else
					{
						bytes = Convert.FromBase64String(_s);
					}
				}
				else
				{
					var _s = System.IO.File.ReadAllText(_savePath);
					
					if (_format == DataFormat.JSON && !_isJsonLegacy)
					{
						bytes = System.Text.Encoding.UTF8.GetBytes(_s);
					}
					else
					{
						bytes = Convert.FromBase64String(_s);
					}
				}
			}
			else
			{
				// load from player prefs
				var _j = PlayerPrefs.GetString(_savePath);
				bytes = Convert.FromBase64String(_j);
			}
			
			if (!_alreadySerialized)
			{
				if (encryption)
				{
					bytes = ByteXOREncryptDecrypt(bytes, encryptionKey);	
				}
				
				
				DB = SerializationUtility.DeserializeValue<OrderedDictionary<string, Database>>(bytes, _format);
			}
			
			
			if (_callEvent && OnDatabaseLoaded != null && Application.isPlaying)
			{
				OnDatabaseLoaded();
			}
			
			databaseLoaded = true;
			
			if (debugMode)
			{
				Debug.Log("Databox: Database loaded");
			}
		}
#endif

		byte[] ReadAllBytes(string fileName)
		{
			byte[] buffer = null;
			using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				buffer = new byte[fs.Length];
				fs.Read(buffer, 0, (int)fs.Length);
			}
			return buffer;
		} 
		
		void LoadWithFullSerializer(string _savePath, bool _callEvent)
		{
			string _j = "";
			if (selectedApplicationPath < 2)
			{
				if (selectedApplicationPath == 1 && !Application.isEditor && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WebGLPlayer))
				{

					DataboxLoadCoroutine.Instance.LoadCoroutine(_savePath, this, _callEvent);
					return;						
				}
				else
				{
					_j = System.IO.File.ReadAllText(_savePath);
				}
			}
			else if (selectedApplicationPath == 2)
			{
				if (!Application.isEditor)
				{
					// file is in resources folder
					_j = Resources.Load<TextAsset>(_savePath).text;
				}
				else
				{
					_j = System.IO.File.ReadAllText(_savePath);
				}
			}
			else
			{
				// load from player prefs
				_j = PlayerPrefs.GetString(_savePath);
			}
			
			
			if (encryption)
			{
				_j = XOREncryptDecrypt(_j, encryptionKey);
			}
			
			//full serializer
			fsSerializer _serializer = new fsSerializer();
			fsData data = fsJsonParser.Parse(_j);
			
			// step 2: deserialize the data
			object deserialized = null;
			_serializer.TryDeserialize(data, typeof(OrderedDictionary<string, Database>), ref deserialized);
			
			var _a = (OrderedDictionary<string, Database>)deserialized as OrderedDictionary<string, Database>;
			DB = _a;
			
			if (_callEvent && OnDatabaseLoaded != null && Application.isPlaying)
			{
				OnDatabaseLoaded();
			}

			databaseLoaded = true;
		
			if (debugMode)
			{
				Debug.Log("Databox: Database loaded");
			}
		}
		
		
		public string ReturnJson(OrderedDictionary<string, Database> _db)
		{
			//full serializer
			fsSerializer _serializer = new fsSerializer();
			fsData data;
			_serializer.TrySerialize( _db, out data);
	
			// emit the data via JSON
			string _j = "";
			if (!compressJson)
			{
				_j = fsJsonPrinter.PrettyJson(data);
			}
			else
			{
				_j = fsJsonPrinter.CompressedJson(data);
			}
			
			return _j;
		}
		
		public OrderedDictionary<string, Database> JsonToDB(string _json, string _version, bool _isNewVersion)
		{
			if (_isNewVersion)
			{
				cloudVersion = _version;
			}
			
			fsSerializer _serializer = new fsSerializer();
			fsData data = fsJsonParser.Parse(_json);
			
			// deserialize the data
			object deserialized = null;
			_serializer.TryDeserialize(data, typeof(OrderedDictionary<string, Database>), ref deserialized);
			
			return (OrderedDictionary<string, Database>)deserialized as OrderedDictionary<string, Database>;
		}
		
	
		public string ReturnSavePath(string _fileName)
		{
			var _savePath = "";
		
			if (!string.IsNullOrEmpty(_fileName))
			{
				switch (selectedApplicationPath)
				{
					case 0:
						_savePath = System.IO.Path.Combine(Application.persistentDataPath, _fileName);
						break;
					case 1:
						_savePath = System.IO.Path.Combine(Application.streamingAssetsPath, _fileName);
						break;
					case 2:
						if (!Application.isEditor)
						{
							var _dir = Path.GetDirectoryName(_fileName);
							_savePath = Path.Combine(_dir, System.IO.Path.GetFileNameWithoutExtension(_fileName));
						}
						else
						{
							_savePath = System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "Resources"), _fileName);
						}
						break;
					case 3:
						_savePath = _fileName;
						break;
				}
				
				if (debugMode)
				{
					Debug.Log("Databox: Save path - " + _savePath);
				}
			}
			
			return _savePath;
		}
		
		/// <summary>
		/// creates a deep copy when registering an object to a new database to loose all references
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public T DeepCopy<T>(T obj)
		{
			fsSerializer _serializer = new fsSerializer();
			fsData data;
			_serializer.TrySerialize( obj, out data);
			
			// emit the data via JSON
			var _j = fsJsonPrinter.CompressedJson(data);
			
			fsData data1 = fsJsonParser.Parse(_j);
			object deserialized = null;
			_serializer.TryDeserialize(data1, typeof(T), ref deserialized);
			
			return (T)deserialized;
		}
		
		
		/// <summary>
		/// XOR Encryption
		/// </summary>
		/// <param name="szPlainText"></param>
		/// <param name="szEncryptionKey"></param>
		/// <returns></returns>
		public byte[] ByteXOREncryptDecrypt(byte[] _bytes, int _encryptionKey) //string _plainText, int _encryptionKey)  
		{  
			
			//byte[] bytes = Convert.FromBase64String(_plainText);
			byte [] output = new byte[_bytes.Length];
			for(int i=0; i< _bytes.Length; i++)
			{
				output[i] = ((byte)(_bytes[i] ^ _encryptionKey));
			}
			
			return output;
			//return Convert.ToBase64String(output);
		}
	
		public string XOREncryptDecrypt(string _plainText, int _encryptionKey)
		{
			System.Text.StringBuilder szInputStringBuild = new System.Text.StringBuilder(_plainText);  
			System.Text.StringBuilder szOutStringBuild = new System.Text.StringBuilder(_plainText.Length);  
			char textCh;  
			for (int iCount = 0; iCount < _plainText.Length; iCount++)  
			{  
				textCh = szInputStringBuild[iCount];  
				textCh = (char)(textCh ^ _encryptionKey);  
				szOutStringBuild.Append(textCh);  
			}  
	
			
			return szOutStringBuild.ToString();  
		} 
		
		// Import from google
		/////////////////////


		// Import Google Sheet at runtime
		/// <summary>
		/// Import google sheet at runtime and append to existing database.
		/// </summary>
		public void ImportAndAppendGoogleSheet()
		{
			DataboxLoadCoroutine.Instance.ImportFromGoogle(this, DataboxGoogleSheetDownloader.ImportType.Append);
		}
		
		/// <summary>
		/// Import google sheet at runtime and replace existing database.
		/// </summary>
		public void ImportAndReplaceGoogleSheet()
		{
			DataboxLoadCoroutine.Instance.ImportFromGoogle(this, DataboxGoogleSheetDownloader.ImportType.Replace);
		}
		
		
		// CLOUD Methods
		/////////////////////
	
		/// <summary>
		/// Download file from cloud. Make sure all cloud settings are correct.
		/// Please note, when calling this method the local file will get overwritten no matter which version is newer.
		/// </summary>
		public void DownloadFromCloud()
		{
			if (databoxCloudService == null)
			{
				databoxCloudService = new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
				DataboxCloud.OnDownloadComplete += InternalOnDatabaseCloudDownloadComplete;
				DataboxCloud.OnDownloadFailed += InternalOnDatabaseCloudDownloadFailed;
				
				DataboxCloud.OnUploadComplete += InternalOnDatabaseCloudUploadComplete;
				DataboxCloud.OnUploadFailed += InternalOnDatabaseCloudUploadFailed; 
			}
			
			DataboxCloud.ForceDownloadRuntime(this);
		}
		
		/// <summary>
		/// Upload local file to the cloud.
		/// Please not, when calling this method the cloud version will get overwritten no matter which version is newer.
		/// </summary>
		public void UploadToCloud()
		{
			if (databoxCloudService == null)
			{
				databoxCloudService = new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
				DataboxCloud.OnDownloadComplete += InternalOnDatabaseCloudDownloadComplete;
				DataboxCloud.OnDownloadFailed += InternalOnDatabaseCloudDownloadFailed;
				
				DataboxCloud.OnUploadComplete += InternalOnDatabaseCloudUploadComplete;
				DataboxCloud.OnUploadFailed += InternalOnDatabaseCloudUploadFailed; 
			}
			
			DataboxCloud.ForceUploadRuntime(this);
		}
		
		void InternalOnDatabaseCloudDownloadComplete()
		{
			if (OnDatabaseCloudDownloaded != null)
			{
				OnDatabaseCloudDownloaded();
			}
		}
		
		void InternalOnDatabaseCloudDownloadFailed()
		{
			if (OnDatabaseCloudDownloadFailed != null)
			{
				OnDatabaseCloudDownloadFailed();
			}
		}
		
		void InternalOnDatabaseCloudUploadComplete()
		{
			if (OnDatabaseCloudUploaded != null)
			{
				OnDatabaseCloudUploaded();
			}
		}
		
		void InternalOnDatabaseCloudUploadFailed()
		{
			if (OnDatabaseCloudUploadFailed != null)
			{
				OnDatabaseCloudUploadFailed();
			}
		}
	}
}