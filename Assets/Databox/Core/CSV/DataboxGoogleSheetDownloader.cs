using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Databox;
using Databox.Utils;

namespace Databox.Ed
{
	public class DataboxGoogleSheetDownloader
	{
		public static DataboxGoogleSheetDownloader instance;
		public static string report { get; private set; }
		UnityWebRequest[] results;
		
		public enum ImportType
		{
			Append,
			Replace
		}
		
		static DataboxGoogleSheetDownloader()
		{
			if (instance == null)
			{
				instance = new DataboxGoogleSheetDownloader();
			}
		}
		
		public static void Download(DataboxObject database, ImportType _importType)
		{
			#if UNITY_EDITOR
			Databox.Utils.EditorCoroutines.Execute(DownloadIE(database, _importType));
			#endif
		}
		
		static IEnumerator DownloadIE(DataboxObject database, ImportType _importType)
		{
			
			DataboxCSVConverter.firstTimeReplace = true;
			
			report = "Start downloading from Google";
			
			for (int i = 0; i < database.googleWorksheets.Count; i ++)
			{
				var _url = database.googleSheetUrl;
				
				_url = FixURL( _url, database.googleWorksheets[i].id);
				
				//Debug.Log(_url);
				UnityWebRequest _download = UnityWebRequest.Get(_url);
				
				yield return _download.SendWebRequest();
				
				while (_download.isDone == false)
					yield return null;
					
				// handle an unknown internet error
				if (_download.isNetworkError || _download.isHttpError)
				{
					Debug.LogWarningFormat("Couldn't retrieve file at <{0}> and error message was: {1}", _download.url, _download.error);
					report += string.Format("\n- [ERROR] {0}: {1}", database.googleWorksheets[i].name, _download.error);
				}
				else
				{ 
					// make sure the fetched file isn't just a Google login page
					if (_download.downloadHandler.text.Contains("google-site-verification")) {
						Debug.LogWarningFormat("Couldn't retrieve file at <{0}> because the Google Doc didn't have public link sharing enabled", _download.url);
						report += string.Format("\n- [ERROR] {0}: This Google Docs share link does not have 'VIEW' access; make sure you enable link sharing.", _url, _download.url);
						continue;
					}
	
	
					//Debug.Log(_download.downloadHandler.text);
					report += string.Format("\n- {0} : DOWNLOADED SUCCESSFULLY", database.googleWorksheets[i].id);
					
	
					List<DataboxCSVConverter.Entry> entries = new List<DataboxCSVConverter.Entry>();
					DataboxCSVConverter.ConvertCSV(_download.downloadHandler.text, out entries);
					
					//for (int e = 0; e < entries.Count; e ++)
					//{
					//	Debug.Log(entries[e].entryName);
					//}
					
					yield return new WaitForSeconds(1f);
					
					
					switch (_importType)
					{
						case ImportType.Append:
							DataboxCSVConverter.AppendToDB(database, database.googleWorksheets[i].name, entries);
							break;
						case ImportType.Replace:
							DataboxCSVConverter.ReplaceDB(database, database.googleWorksheets[i].name, entries);
							break;
					}
					
				}
				
				_download.Dispose();
			}
		}
		
		public static string FixURL(string url, string gId)
		{
			// if it's a Google Docs URL, then grab the document ID and reformat the URL
			if (url.StartsWith("https://docs.google.com/document/d/"))
			{
				var docID = url.Substring( "https://docs.google.com/document/d/".Length, 44 );
				return string.Format("https://docs.google.com/document/export?gid={1}&format=txt&id={0}&includes_info_params=true", docID, gId);
			}
			if (url.StartsWith("https://docs.google.com/spreadsheets/d/"))
			{
				var docID = url.Substring( "https://docs.google.com/spreadsheets/d/".Length, 44 );
				return string.Format("https://docs.google.com/spreadsheets/export?gid={1}&format=csv&id={0}", docID, gId);
			}
	            
			return url;
		}
	}
}
