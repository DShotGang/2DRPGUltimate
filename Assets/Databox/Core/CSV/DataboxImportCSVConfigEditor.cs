#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Databox.Ed
{
	public class DataboxImportCSVConfigEditor
	{
		static string localFileTableName = "";
		static string localFilePath = "";
		static string lastFileName = "";
		static bool fileOK = false;
		
		static string[] importOptions = new string[]{"Import from GOOGLE", "Import LOCAL file"};
		static int selectedOption = -1;
		
		static Color colBlue = new Color(125f/255f, 185f/255f, 190f/255f);
		
		
		public static void DrawImportCSVConfigUI(DataboxObject database)
		{
			
			selectedOption = GUILayout.Toolbar(selectedOption, importOptions);
			
			if (selectedOption == 0)
			{
				
				using (new GUILayout.VerticalScope("Window"))
				{

					//GUILayout.Label("Import from GOOGLE");
					
					using (new GUILayout.HorizontalScope("Box"))
					{
						GUILayout.Label("Google Sheet URL:", GUILayout.Width(120));
						database.googleSheetUrl = GUILayout.TextField(database.googleSheetUrl);
						GUILayout.Label(new GUIContent("[?]", "Make sure public link sharing is enabled and enter the share link here."), GUILayout.Width(20));
						
						if (GUILayout.Button(">", GUILayout.Width(30)))
						{
							Application.OpenURL(database.googleSheetUrl);
						}
					}
			
					using (new GUILayout.VerticalScope("Box"))
					{
	
						GUILayout.Label(new GUIContent("WORKSHEETS [?]", "Each worksheet has an unique ID. You can find the ID in the URL at: gid='ID-NUMBER-HERE'"));
						
						
						if (GUILayout.Button("Add new worksheet ID"))
						{
							database.googleWorksheets.Add(new DataboxObject.GoogleWorksheet());
						}
						
						for (int i = 0; i < database.googleWorksheets.Count; i ++)
						{
							using (new GUILayout.HorizontalScope("Box"))
							{
								GUI.color = colBlue;
								GUILayout.Label((i + 1).ToString(), GUILayout.Width(20));
								GUI.color = Color.white;
	
								GUILayout.Label("Google Worksheet Name:", GUILayout.Width(170));
								database.googleWorksheets[i].name = GUILayout.TextField(database.googleWorksheets[i].name, GUILayout.Width(150));
								
								GUILayout.Label("Google Worksheet ID:");
								database.googleWorksheets[i].id = GUILayout.TextField(database.googleWorksheets[i].id);
								
	
								if (GUILayout.Button("x", GUILayout.Width(20)))
								{
									database.googleWorksheets.RemoveAt(i);
								}
							}
						}
					}
					
					if (string.IsNullOrEmpty(database.googleSheetUrl) || string.IsNullOrEmpty(database.googleSheetUrl) || database.googleWorksheets.Count == 0)
					{
						GUI.enabled = false;
					}
					else
					{
						GUI.enabled = true;
					}
					
					using (new GUILayout.HorizontalScope())
					{
						if (GUILayout.Button("Download & Append", GUILayout.Height(30)))
						{
							DataboxGoogleSheetDownloader.Download(database, DataboxGoogleSheetDownloader.ImportType.Append);
						}
						
						if (GUILayout.Button("Download & Replace", GUILayout.Height(30)))
						{
							
							if (EditorUtility.DisplayDialog("Replace database?", "Are you sure you want to replace the current database?", "Replace", "Cancel"))
							{
								DataboxGoogleSheetDownloader.Download(database, DataboxGoogleSheetDownloader.ImportType.Replace);
							}
							GUIUtility.ExitGUI();
						}
					}
					
					GUI.enabled = true;
					
					GUILayout.Label("Log:");
					GUILayout.Label(DataboxGoogleSheetDownloader.report);
				}
			}
	
			if (selectedOption == 1)
			{
			
				using (new GUILayout.VerticalScope("Window"))
				{

			
					if (GUILayout.Button("Select file..."))
					{
						localFilePath = EditorUtility.OpenFilePanel("Select CSV file", "", "");
						GUIUtility.ExitGUI();
					}
					
					if (localFilePath != lastFileName)
					{
						fileOK = System.IO.File.Exists(localFilePath);
					}
	
					if (fileOK)
					{
						EditorGUILayout.HelpBox("Selected file: " + localFilePath, MessageType.Info);
					}
					
					using (new GUILayout.HorizontalScope())
					{
						GUILayout.Label("Table name:", GUILayout.Width(100));
						localFileTableName = GUILayout.TextField(localFileTableName);
					}
					
					if (string.IsNullOrEmpty(localFileTableName) || string.IsNullOrEmpty(localFilePath))
					{
						GUI.enabled = false;
					}
					else
					{
						GUI.enabled = true;
					}
					
					using (new GUILayout.HorizontalScope())
					{
						if (GUILayout.Button("Append", GUILayout.Height(30)))
						{
							if (System.IO.File.Exists(localFilePath))
							{
								// open file at path
								var _csvString = ReadLocalFile(localFilePath);
								
								List<DataboxCSVConverter.Entry> entries = new List<DataboxCSVConverter.Entry>();
								DataboxCSVConverter.ConvertCSV(_csvString, out entries);
							
								DataboxCSVConverter.AppendToDB(database, localFileTableName, entries);
							}
						}
						
						if (GUILayout.Button("Replace", GUILayout.Height(30)))
						{
							if (EditorUtility.DisplayDialog("Replace database?", "Are you sure you want to replace the current database?", "Replace", "Cancel"))
							{
								if (System.IO.File.Exists(localFilePath))
								{
									// open file at path
									var _csvString = ReadLocalFile(localFilePath);
									
									List<DataboxCSVConverter.Entry> entries = new List<DataboxCSVConverter.Entry>();
									DataboxCSVConverter.ConvertCSV(_csvString, out entries);
									
									DataboxCSVConverter.ReplaceDB(database, localFileTableName, entries);
								}
							}
							GUIUtility.ExitGUI();
						}
					}
				
					GUI.enabled = true;
				}
			}
			
		}
		
		static string ReadLocalFile(string _path)
		{		
			var _sr = new System.IO.StreamReader(_path);
			
			var _out = _sr.ReadToEnd();
			
			_sr.Close();
			
			return _out;
		}
	}
}
#endif