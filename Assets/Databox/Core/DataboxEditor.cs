using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System.Reflection;
using Databox.Utils;
using Databox.Dictionary;

namespace Databox.Ed
{
	/// <summary>
	/// DataboxObject editor. Unity Editor and Runtime
	/// </summary>
	#pragma warning disable 0414
	public class DataboxEditor {
		
		static DataboxObject database; 
		
		static Rect runtimeEditorWindow = new Rect(0, 40, Screen.width, Screen.height - 40);
		
		static int inspectorWidth = 200;
		static string selectedDatabaseTable = "";
		static int selectedDatabaseTableIndex = 0;
		static string dbTableName = "";
		//static string resetDBText = "";
		static string searchKey = "";	
		#if UNITY_EDITOR
		static string entryID = "";
	
		static string valueID = "";
		#endif
		static Vector2 scrollPosition;
		
		static string duplicateId;
		static string duplicateSelectedId;
		static bool duplicateEnabled = false;
		public static string duplicateToTable;
		
		// runtime editor settings
		static bool disableInspector = false;
		static bool disableDeleteOption = false;
		static bool disableConfigurationOption = false;
		static bool disableCloudOption = false;
		static bool disableImportCSVOption = false;


		public static int selectedTypeIndex = 0;
		public static List<string> allTypes = new List<string>();
		public static List<string> allTypesNames = new List<string>();
		
		static bool configOpen = false;
		static bool cloudConfigOpen = false;
		static bool importCSVConfigOpen = false;
		static bool resLoaded = false;
		static string[] applicationPath = new string[]{"Persistent Data Path", "Streaming Asset Path", "Resources", "Player Prefs"};
		
		static Color colBlue = new Color(125f/255f, 185f/255f, 190f/255f);
		
		#if UNITY_EDITOR
		static Texture2D logoheader;
		static Texture2D iconListView;
		static Texture2D iconFullView;
		static Texture2D iconDataType;
		static Texture2D iconInitValue;
		#endif
		
		static string selectedFoldout;
		static bool foldout;
		#if UNITY_EDITOR
		static string editorIconClose = "Toolbar Minus";
		static string editorIconNewWindow = "winbtn_win_rest";
		static string editorIconReset = "d_Refresh";
		static string editorIconCloud = "CloudConnect";
		static string editorLargeLabel = "BoldLabel";
		#endif
		static string rowBackground = "AnimationRowOdd";
		
		static GUIStyle elementButton;
		static GUIStyle elementButtonActive;
		static GUIStyle elementDraggingHandle;
		
		// Cloud comparison
		public static DataboxCloud.ChangeLog deletedCompare = new DataboxCloud.ChangeLog();
		public static DataboxCloud.ChangeLog newCompare = new DataboxCloud.ChangeLog();
		
		public static string cloudAgreement = "Databox/doorfortyfour is not liable for any loss of data or damages to your data when using the cloud service." + "\n" +
			"Please backup your database by any concerns. You can also use different unique upload IDs to upload different versions of your database.";
		public static bool cloudAgreementAccepted = false;
		
		static bool isDragging = false;
		static string dragEntry = "";
		static int dragAndDropIndex = 0;
		static int dragAndDropStartIndex = 0;
		#if UNITY_EDITOR
		static bool addToSelected = false;
		#endif
		static bool selectAllEntries = false;
		static bool lastSelectedAllEntries = false;
		static string newTableName = "";
		
		static bool renamingActive = false;
		static string oldRenamingEntry = "";
		static string newRenamingEntry = "";
		static int renamingIndex = -1;
		#if UNITY_EDITOR
		static double doubleClickTime = 0.3f;
		static double clickTime;
		static bool renamingOK = false;
		#endif
		static string searchResult;
		
		static float entriesCount = 0f;
		static int currentPage = 1;
		static int maxPage = 0;
		
		static string[] tableContextMenuOptions = new string[]{"", "Move forward", "Move backward", "Rename Table", "Duplicate Table", "Reset Table", "Delete Table" };
		static int selectedTableContextMenu = 0;
		static string renameTableID = "";
		static bool tableRenamingActive = false;

		static string valueRenameKey = "";
		static string oldValueName = "";
		static string newValueName = "";
		static bool renameValueError = false;
		
		// Runtime editor
		public static void DrawEditorRuntime(DataboxObject _db, GUISkin _skin, bool _disableInspector, bool _disableDeleteOption, bool _disableConfigurationOption, bool _disableCloudOption, bool _editorWindowOpen)
		{
			disableInspector = _disableInspector;
			disableDeleteOption = _disableDeleteOption;
			disableConfigurationOption = _disableConfigurationOption;
			disableCloudOption = _disableCloudOption;
			disableImportCSVOption = true; // disable import feature during runtime
			
			rowBackground = "Box";
			
			// List elemen styles
			var elementBackground = new GUIStyle("Box"){};
							
			elementButton = new GUIStyle("Button")
			{	
				alignment = TextAnchor.MiddleLeft,
			};
							
			elementButtonActive = new GUIStyle("Button")
			{
				alignment = TextAnchor.MiddleLeft,			
			};
			
			elementDraggingHandle = new GUIStyle("Box");
			
			
			database = _db;
			
			if (_skin != null)
			{
				GUI.skin = _skin;
			}
			
			if (_editorWindowOpen)
			{
				runtimeEditorWindow = new Rect(0, 20, Screen.width, Screen.height - 20);
				runtimeEditorWindow = GUILayout.Window(0, runtimeEditorWindow, DrawEditorRuntimeWindow, "");
			}
		}
		
		public static void DrawEditorRuntimeWindow(int _id)
		{
			using (new GUILayout.VerticalScope("Box"))
			{
				DrawToolbar();
				DrawSelectedDatabase(true);
			}
		}
		
		// Scriptable object editor
		public static void DrawEditor(DataboxObject _db)
		{
			if (!resLoaded)
			{
				resLoaded = true;
				LoadEditorResources();	
			}
			
			database = _db;
			disableInspector = false;
			disableDeleteOption = false;
			disableConfigurationOption = false;
			disableCloudOption = false;
			disableImportCSVOption = false;
			
			rowBackground = "AnimationRowOdd";
		
			// List elemen styles
			var elementBackground = new GUIStyle("RL Element"){};
							
			elementButton = new GUIStyle("Button")
			{
			
				hover = elementBackground.onHover,
				active = elementBackground.onActive,
								
				normal = new GUIStyleState 
				{ 
					background = elementBackground.onNormal.background,
					textColor = Color.white,
				},
				
				alignment = TextAnchor.MiddleLeft,
								
				overflow = new RectOffset(25, 131, 1, 3),
				padding = new RectOffset(10, 0, 0, 0)
			};
							
			elementButtonActive = new GUIStyle("Button")
			{
				normal = elementBackground.onActive,
				hover = elementBackground.onHover,
				active = elementBackground.onActive,
				alignment = TextAnchor.MiddleLeft,
								
				overflow = new RectOffset(30, 131, 0, 1),
				padding = new RectOffset(10, 0, 0, 0)
			};
			
			elementDraggingHandle = new GUIStyle("RL DragHandle");
	
			using (new GUILayout.VerticalScope("Window"))
			{
				DrawToolbar();
				DrawSelectedDatabase(false);
				DrawBottom();
			}
		}
		
		
		static void DrawToolbar()
		{
			
			using (new GUILayout.HorizontalScope("Box"))
			{
				if (GUILayout.Button("Load", GUILayout.Width(100)))
				{
					database.LoadDatabase(false);
				}
				
				if (GUILayout.Button("Save", GUILayout.Width(100)))
				{
					database.SaveDatabase();
				}
				
				#if UNITY_EDITOR
				if (GUILayout.Button("Generate KEYS", GUILayout.Width(110)))
				{
					var _staticStringFilePath = EditorUtility.SaveFilePanel("Save key file to:", "Assets", database.name + "_KEYS", "cs");
					
					if (_staticStringFilePath.Length != 0)
					{
						var _result = DataboxGenerateKeys.GenerateStaticStrings(database);
						System.IO.File.WriteAllText(_staticStringFilePath, _result);
						
						AssetDatabase.Refresh();
					}
					GUIUtility.ExitGUI();
				}
				#endif
				
				GUILayout.FlexibleSpace();
				
				if (!disableConfigurationOption)
				{
					configOpen = GUILayout.Toggle(configOpen, "Config", "Button", GUILayout.Width(100));
					
					if (configOpen)
					{
						cloudConfigOpen = false;
						importCSVConfigOpen = false;
					}
				}
				
				if (!disableCloudOption)
				{
					#if UNITY_EDITOR
					cloudConfigOpen = GUILayout.Toggle(cloudConfigOpen, EditorGUIUtility.IconContent(editorIconCloud), "Button", GUILayout.Width(100));
					#else
					cloudConfigOpen = GUILayout.Toggle(cloudConfigOpen, "Cloud", "Button", GUILayout.Width(100));
					#endif
					
					if (cloudConfigOpen)
					{
						configOpen = false;
						importCSVConfigOpen = false;
					}
				}
				
				if (!disableImportCSVOption)
				{
					importCSVConfigOpen = GUILayout.Toggle(importCSVConfigOpen, "Import", "Button", GUILayout.Width(100));
					
					if (importCSVConfigOpen)
					{
						cloudConfigOpen = false;
						configOpen = false;
					}
				}
				
				#if UNITY_EDITOR
				if (GUILayout.Button(EditorGUIUtility.IconContent(editorIconNewWindow)))
				{
					DataboxObjectEditorWindow.dbObject = database;
					DataboxObjectEditorWindow.Init();				
				}
				#endif
			}
			
			
			if (configOpen)
			{
				
				using (new GUILayout.VerticalScope("Window"))
				{
					
					using (new GUILayout.VerticalScope("Box"))
					{
						GUILayout.Label("File Name:");
						database.fileName = GUILayout.TextField(database.fileName);
						
						//EditorGUILayout.HelpBox("Please note, choosing the right save folder depends on your needs and platform. Here are some recommendations based on our experience \n" +
						//	"Best solution for loading data: StreamingAssets or Resources. \n" +
						//	"Best solution for save games: Persistent data path or PlayerPrefs.", MessageType.Info);
						
						database.selectedApplicationPath = GUILayout.SelectionGrid(database.selectedApplicationPath, applicationPath, 4);
						
						switch(database.selectedApplicationPath)
						{
							case 1:
								
								break;
							case 2:
								#if UNITY_EDITOR
								EditorGUILayout.HelpBox("Use it to load data during runtime. Saving to a Resources folder only works inside the Unity editor.", MessageType.Warning);
								
								if (!System.IO.Path.HasExtension(database.fileName))
								{
									EditorGUILayout.HelpBox("Please add a .json extension when using the resouces path folder", MessageType.Error);
								}
								else
								{
									if (System.IO.Path.GetExtension(database.fileName).ToLower() != ".json")
									{
										EditorGUILayout.HelpBox("Please add a .json extension when using the resouces path folder", MessageType.Error);
									}
								}
								#else
								GUILayout.Label("Use it to load data during runtime. Saving to a Resources folder only works inside the Unity editor. Also please make sure to provide a .Json extension to the file name.");
								#endif
								break;
							case 0:
							case 3:
								#if UNITY_EDITOR
								EditorGUILayout.HelpBox("Only use it if you want to use this DataboxObject to create save game files during runtime.", MessageType.Warning);
								#else
								GUILayout.Label("Only use it if you want to use this DataboxObject to create save game files during runtime.");								
								#endif
								break;
						}
						
						GUILayout.Label("Save Path:");
						
						if (!string.IsNullOrEmpty(database.fileName) && database.selectedApplicationPath >= 0)
						{
							GUI.color = colBlue;	
						}
						
						using (new GUILayout.HorizontalScope("Box"))
						{	
							GUI.color = Color.white;
							GUILayout.Label(database.savePath);
						}
						
					
					
						
						if (!string.IsNullOrEmpty(database.fileName))
						{
							switch (database.selectedApplicationPath)
							{
							case 0:
								database.savePath = System.IO.Path.Combine(Application.persistentDataPath, database.fileName);
								break;
							case 1:
								database.savePath = System.IO.Path.Combine(Application.streamingAssetsPath, database.fileName);
								break;
							case 2: 
								database.savePath = "Resources/" + database.fileName;
								break;
							case 3:
								database.savePath = "Key: " + database.fileName;
								break;
							}
						}
					}
					
					#if UNITY_EDITOR
					using (new GUILayout.VerticalScope("Box"))
					{
						database.automaticSave = GUILayout.Toggle(database.automaticSave, "Automatic Save (only in editor and when game is not running)");	
					}
					
					
					using (new GUILayout.VerticalScope("Box"))
					{
						database.serializer = (DataboxObject.Serializer)EditorGUILayout.EnumPopup("Serializer:", database.serializer);
					
					
					
						switch (database.serializer)
						{
							// FULL SERIALIZER CONFIGURATIONS
							case DataboxObject.Serializer.FullSerializer:
							
								using (new GUILayout.VerticalScope("Box"))
								{
									GUILayout.Label("FullSerializer:", "boldLabel");
									
									database.encryption = GUILayout.Toggle(database.encryption, "Encryption");	
									
									if (database.encryption)
									{
										database.encryptionKey = EditorGUILayout.IntField("Encryption Key", database.encryptionKey);
									}
							
							
									database.compressJson = GUILayout.Toggle(database.compressJson, "Compress Json");
								
								}
								
							break;
							
							// ODIN SERIALIZER CONFIGURATIONS
							case DataboxObject.Serializer.OdinSerializer:
								
								#if NET_4_6
								using (new GUILayout.VerticalScope("Box"))
								{
									#if UNITY_EDITOR
									GUILayout.Label("Odin Serializer:", "boldLabel");
									#else
									GUILayout.Label("Odin Serializer:");
									#endif
								
									database.encryption = GUILayout.Toggle(database.encryption, "Encryption");	
									
									if (database.encryption)
									{
										database.encryptionKey = EditorGUILayout.IntField("Encryption Key", database.encryptionKey);
									}
									
									database.dataFormat = (DataboxObject.OdinDataFormat)EditorGUILayout.EnumPopup("Data format:", database.dataFormat);
									
									if (database.dataFormat == DataboxObject.OdinDataFormat.json_legacy)
									{
										EditorGUILayout.HelpBox("Json legacy is the old json format which doesn't create nice json formatted files. (version 1.1.2p1)." +
											"\nAs of version 1.1.2p2 please use the new json format. If you have a file saved with the json_legacy, load it with json_legacy and save it with the new json format.", MessageType.Warning);
									}
								}
								#else
								using (new GUILayout.VerticalScope("Box"))
								{
									EditorGUILayout.HelpBox("To make use of Odin serializer please set API compatibility level to .NET 4.X in the player build settings.\nOtherwise, please remove the OdinSerializer folder in: Databox/Core/Serializers", MessageType.Warning);
								}
								#endif
								
								
								break;
						}
					
					}
					#endif
					
					
					using (new GUILayout.VerticalScope("Box"))
					{
						database.debugMode = GUILayout.Toggle(database.debugMode, "Debug Mode");
					}
					
					using (new GUILayout.VerticalScope("Box"))
					{
						GUILayout.Label("Entries per page");
						var _epp = database.entriesPerPage.ToString();
						_epp = GUILayout.TextField (_epp);
						database.entriesPerPage = int.Parse(_epp);
					}
									
				}
			}
			
			if (cloudConfigOpen)
			{
				DataboxCloudConfigEditor.DrawCloudConfigUI(database);		
			}
			
			#if UNITY_EDITOR
			if (importCSVConfigOpen)
			{
				DataboxImportCSVConfigEditor.DrawImportCSVConfigUI(database);
			}
			#endif
			
			
			if (string.IsNullOrEmpty(database.fileName))
			{
				GUI.enabled = false;
			}
			else
			{
				GUI.enabled = true;	
			}
			
			
			
			GUILayout.Space(5);
			
			using (new GUILayout.VerticalScope("Box"))
			{
				using (new GUILayout.HorizontalScope())
				{
					dbTableName = GUILayout.TextField(dbTableName);
					
					
					GUI.enabled = !string.IsNullOrEmpty(dbTableName);
					
					if (GUILayout.Button("Add Table", GUILayout.Width(100)))
					{
						database.AddDatabaseTable(dbTableName);
						
						if (database.automaticSave)
						{
							database.SaveDatabase();
						}
					}
					
					GUI.enabled = true;
				}
		
				
				GUILayout.Space(5);
			}
		}
		
		static void DrawTableSelection()
		{
			// Tables
			//////////////////////
			if (database.DB == null)
				return;
				
			var _toolbarOptions = database.DB.Keys.ToArray();
			var _tableI = 0;
			var _column = 6f;
			var _row = Mathf.CeilToInt(_toolbarOptions.Length / _column);
				
			using (new GUILayout.HorizontalScope())
			{
			using (new GUILayout.VerticalScope())
			{
				for (int i = 0; i < _row; i ++)
				{
							
					using (new GUILayout.HorizontalScope())
					{
						for (int j = 0; j < _column; j ++)
						{
							if (_tableI < _toolbarOptions.Length)
							{
								using (new GUILayout.HorizontalScope())
								{
									#if UNITY_EDITOR
									GUIContent tab = EditorGUIUtility.IconContent("tabbar on");

									var tabButton = new GUIStyle("Button")
									{	
										normal = new GUIStyleState 
										{ 
											background = tab.image as Texture2D,
											textColor = Color.white,
										},
													
										active = new GUIStyleState 
										{ 
											background = tab.image as Texture2D,
											textColor = Color.white,
										}
									};
									#else
									var tabButton = new GUIStyle("Button");
									#endif			
												
									if (_tableI == selectedDatabaseTableIndex)
									{
										GUI.color = colBlue;
									}
									else
									{
										GUI.color = Color.white;
									}

									if (renameTableID == database.DB.ElementAt(_tableI).Key && tableRenamingActive)
									{
										Event e = Event.current;
										if (e.type == EventType.KeyDown)
										{
											if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
											{
												RenameTable(newTableName);
												tableRenamingActive = false;
											}	
											else if (e.keyCode == KeyCode.Escape)
											{
												tableRenamingActive = false;
											}
										}
												
										newTableName = GUILayout.TextField(newTableName, GUILayout.Height(20));
											
									}
									else
									{
										
												
										#if UNITY_EDITOR
										if (GUILayout.Button(database.DB.ElementAt(_tableI).Key, tabButton, GUILayout.Height(20)))
										{
											selectedDatabaseTableIndex = _tableI;
											selectedDatabaseTable = database.DB.ElementAt(selectedDatabaseTableIndex).Key;
										}
										#else
										if (GUILayout.Button(database.DB.ElementAt(_tableI).Key, GUILayout.Height(20)))
										{
											selectedDatabaseTableIndex = _tableI;
											selectedDatabaseTable = database.DB.ElementAt(selectedDatabaseTableIndex).Key;
										}
										#endif
										
										
									}
											
									GUI.color = Color.white;
								}
								_tableI ++;
							}
						}
					}
				}
				
				
			}
			
				// Tables Options
				////////////////////
				
				#if UNITY_EDITOR
				if (!string.IsNullOrEmpty(selectedDatabaseTable))
				{
					var settings = EditorGUIUtility.IconContent("pane options");
					GUILayout.Label(settings.image, GUILayout.Width(25));
					var _rr = GUILayoutUtility.GetLastRect();
							
					selectedTableContextMenu = EditorGUI.Popup(new Rect(_rr.x, _rr.y+1, 20, 20),"", selectedTableContextMenu, tableContextMenuOptions, "Label");
										
					switch(selectedTableContextMenu)
					{
					case 0:
						break;
					case 1:
						//move forward
						tableRenamingActive = false;
						renameTableID = "";
						
						if (selectedDatabaseTableIndex < _toolbarOptions.Length - 1)
						{
							MoveTableForward();
						}
						selectedTableContextMenu = 0;
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}
						break;
					case 2:
						//move backward
						tableRenamingActive = false;
						renameTableID = "";
						
						if (selectedDatabaseTableIndex > 0)
						{
							MoveTableBack();
						}
						selectedTableContextMenu = 0;
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}								
						break;
					case 3:
						//rename
						tableRenamingActive = true;
						
						renameTableID = database.DB.ElementAt(selectedDatabaseTableIndex).Key;
						newTableName = renameTableID;
						selectedTableContextMenu = 0;
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}
						break;
					case 4:
						//duplicate
						tableRenamingActive = false;
						renameTableID = "";
						selectedTableContextMenu = 0;
														
						var _t = database.DB[selectedDatabaseTable];
						database.DB.Add(selectedDatabaseTable + "_" + Random.Range(0, 100) + "_copy", _t);
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}						
						break;
					case 5:
						tableRenamingActive = false;
						renameTableID = "";
						selectedTableContextMenu = 0;
						// reset
						if (EditorUtility.DisplayDialog("Reset table",
							"Are you sure you want to reset table: " + database.DB.ElementAt(selectedDatabaseTableIndex).Key + " to it's initial values?", "Yes", "No"))
						{
							database.ResetToInitValues(selectedDatabaseTable);
						}
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}
						break;
					case 6:
						//delete
						tableRenamingActive = false;
						renameTableID = "";
						selectedTableContextMenu = 0;
												
						if (EditorUtility.DisplayDialog("Delete table",
							"Are you sure you want to delete table: " + database.DB.ElementAt(selectedDatabaseTableIndex).Key, "Yes", "No"))
						{
							database.RemoveDatabaseTable(selectedDatabaseTable);
							selectedDatabaseTable = "";
							selectedDatabaseTableIndex = -1;
						}
						if (database.automaticSave)
						{
							database.SaveDatabase();	
							//EditorCoroutines.Execute(database.SaveDatabaseAsync());
						}
						else
						{
							database.errors = DataboxObject.ErrorType.UnsavedChanges;	
						}
						break;
					}
				}
				#endif
			}
		}
		
		static void DrawSelectedDatabase(bool _runtime)
		{
			
			DrawTableSelection();
			
			// Add new Database
			using (new GUILayout.VerticalScope())
			{
				using (new GUILayout.HorizontalScope())
				{
					if (!disableInspector && !string.IsNullOrEmpty(selectedDatabaseTable))
					{
						
						DrawDataInspector(_runtime);
						
					}
					
					DrawDataSheet();
				}
			}
		}
		
		static void DrawDataInspector(bool _runtime)
		{
			
			using (new GUILayout.VerticalScope("Box", database.showInspector ? GUILayout.Width(inspectorWidth) : GUILayout.Width(40)))
			{

				if (!database.showInspector)
				{
				
					database.showInspector = GUILayout.Toggle(database.showInspector, database.showInspector ? "<<" : ">>", "Button", GUILayout.Height(32));
					
					#if UNITY_EDITOR
					if (GUILayout.Button(database.collapsedView ? iconFullView : iconListView, GUILayout.Width(32), GUILayout.Height(32)))
					{
						database.collapsedView = !database.collapsedView;
					}
					#else
					if (GUILayout.Button(database.collapsedView ? "Full" : "List", GUILayout.Width(32), GUILayout.Height(32)))
					{
						database.collapsedView = !database.collapsedView;
					}
					#endif
					#if UNITY_EDITOR
					database.showFieldTypes = GUILayout.Toggle(database.showFieldTypes, iconDataType, "Button", GUILayout.Width(32), GUILayout.Height(32));
					database.showInitValues = GUILayout.Toggle(database.showInitValues, iconInitValue, "Button", GUILayout.Width(32), GUILayout.Height(32));
					#else
					database.showFieldTypes = GUILayout.Toggle(database.showFieldTypes, "Type", "Button", GUILayout.Width(32), GUILayout.Height(32));
					database.showInitValues = GUILayout.Toggle(database.showInitValues, "Init", "Button", GUILayout.Width(32), GUILayout.Height(32));
					#endif
				}
	
				if (database.showInspector)
				{
					// View options
					using (new GUILayout.VerticalScope("Box"))
					{	
						
						using (new GUILayout.HorizontalScope())
						{
							database.showInspector = GUILayout.Toggle(database.showInspector, database.showInspector ? "<<" : ">>", "Button", GUILayout.Height(32));
							
							#if UNITY_EDITOR
							if (GUILayout.Button(database.collapsedView ? iconFullView : iconListView, GUILayout.Width(32), GUILayout.Height(32)))
							{
								database.collapsedView = !database.collapsedView;
							}
							#else
							if (GUILayout.Button(database.collapsedView ?  "Full" : "List", GUILayout.Width(32), GUILayout.Height(32)))
							{
								database.collapsedView = !database.collapsedView;
							}
							#endif
							
							#if UNITY_EDITOR
							database.showFieldTypes = GUILayout.Toggle(database.showFieldTypes, iconDataType, "Button", GUILayout.Width(32), GUILayout.Height(32));
							database.showInitValues = GUILayout.Toggle(database.showInitValues, iconInitValue, "Button", GUILayout.Width(32), GUILayout.Height(32));
							#else
							database.showFieldTypes = GUILayout.Toggle(database.showFieldTypes, "Type", "Button", GUILayout.Width(32), GUILayout.Height(32));
							database.showInitValues = GUILayout.Toggle(database.showInitValues, "Init", "Button", GUILayout.Width(32), GUILayout.Height(32));
							#endif
							}
							
					}
					
					//GUILayout.Label("", GUI.skin.horizontalSlider);		
					
					//if (!disableDeleteOption)
					//{
					//	using (new GUILayout.VerticalScope("Box"))
					//	{
					//		GUILayout.Label("Type DELETE to delete table " + selectedDatabaseTable );
							
					//		using (new GUILayout.HorizontalScope())
					//		{
					//			deleteDBText = GUILayout.TextField(deleteDBText);
					//			if (GUILayout.Button("delete", GUILayout.Width(75)))
					//			{
					//				if (deleteDBText == "DELETE")
					//				{
					//					database.RemoveDatabaseTable(selectedDatabaseTable);
					//					selectedDatabaseTable = "";
					//					selectedDatabaseTableIndex = -1;
					//				}
					//			}
					//		}
							
							
					//	}
						
					//	GUILayout.Label("", GUI.skin.horizontalSlider);		
					//}
					
					//using (new GUILayout.VerticalScope("Box"))
					//{
					//	GUILayout.Label("Type RESET to reset " + selectedDatabaseTable + " back to init values");
						
					//	using (new GUILayout.HorizontalScope())
					//	{
					//		resetDBText = GUILayout.TextField(resetDBText);
					//		if (GUILayout.Button("reset", GUILayout.Width(75)))
					//		{
					//			if (resetDBText == "RESET")
					//			{
					//				database.ResetToInitValues(selectedDatabaseTable);
					//			}
					//		}
					//	}
					//}
					
					GUILayout.Label("", GUI.skin.horizontalSlider);
					
					if (!string.IsNullOrEmpty(searchKey))
					{
						GUI.color = Color.yellow;
					}
					else
					{
						GUI.color = Color.white;
					}
					
					using (new GUILayout.VerticalScope("Box"))
					{
						GUI.color = Color.white;
						
						GUILayout.Label ("SEARCH ENTRY");
						using (new GUILayout.HorizontalScope())
						{
							
							#if UNITY_EDITOR
							
							if (!_runtime)
							{
								GUI.SetNextControlName ("FilterTextField");
								searchKey = GUILayout.TextField(searchKey, "SearchTextField"); //, GUILayout.Width(195));
		
								if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton")))
								{
									searchKey = "";
								}
							}
							else
							{
								searchKey = GUILayout.TextField(searchKey); //, GUILayout.Width(195));
		
								if (GUILayout.Button("x", GUILayout.Width(20)))		
								{
									searchKey = "";
								}
							}
							
							#else
							searchKey = GUILayout.TextField(searchKey);
							if (GUILayout.Button("x", GUILayout.Width(20)))					
							{
								searchKey = "";
							}
							#endif
						}
						
						
					}
					
					GUILayout.Label("", GUI.skin.horizontalSlider);
					
					#if UNITY_EDITOR
					
					if ( !_runtime )
					{
						using (new GUILayout.VerticalScope("Box"))
						{
						
							if (selectedDatabaseTable != null)
							{
								GUILayout.Space(5);
								
								addToSelected = GUILayout.Toggle(addToSelected, "Add to selected");
									
								GUILayout.Space(5);
								
								if (!addToSelected)
								{
									GUILayout.Label("ENTRY ID");
									entryID =  GUILayout.TextField(entryID);
								}
							
								
								GUILayout.Label("VALUE ID");
								valueID =  GUILayout.TextField(valueID);
								GUILayout.Label("TYPE");
							

								var _dropdownRect = GUILayoutUtility.GetLastRect();
								
								if (selectedTypeIndex >= allTypes.Count)
								{
									selectedTypeIndex = 0;
								}
								
								if (allTypes == null || allTypes.Count == 0 || allTypesNames == null || allTypesNames.Count == 0)
								{
									GetDataTypes(out allTypes, out allTypesNames);
								}
								
								if (EditorGUILayout.DropdownButton(new GUIContent(allTypesNames[selectedTypeIndex]), FocusType.Keyboard, GUILayout.ExpandWidth(true)))
								{	
									PopupWindow.Show(_dropdownRect, new PopUps.PopupType(allTypesNames.ToList(), _dropdownRect, inspectorWidth));
								}
								
							
								GUILayout.Space(5);
								
								
								
								
								if (GUILayout.Button("Add value", GUILayout.Height(50)))
								{
									if (!addToSelected)
									{
										if (!string.IsNullOrEmpty(entryID) && !string.IsNullOrEmpty(valueID))
										{
											var _instance = System.Activator.CreateInstance(System.Type.GetType(allTypes[selectedTypeIndex]));
											//var _instance = ScriptableObject.CreateInstance(System.Type.GetType(allTypes[selectedTypeIndex]));
											//Debug.Log(selectedType + " " + type.Name.ToString());
											database.AddData(selectedDatabaseTable, entryID, valueID, _instance as DataboxType);
											
											if (database.automaticSave)
											{
												database.SaveDatabase();
												//EditorCoroutines.Execute(database.SaveDatabaseAsync());
											}
										}
									}
									else
									{
										if (!string.IsNullOrEmpty(valueID))
										{
											foreach (var entry in database.DB[selectedDatabaseTable].entries.Keys)
											{
												if (database.DB[selectedDatabaseTable].entries[entry].selected)
												{
													var _instance = System.Activator.CreateInstance(System.Type.GetType(allTypes[selectedTypeIndex]));
													database.AddData(selectedDatabaseTable, entry, valueID, _instance as DataboxType);
													
													if (database.automaticSave)
													{
														database.SaveDatabase();
														//EditorCoroutines.Execute(database.SaveDatabaseAsync());
													}
												}
											}
										}
									}
								}
							}
						}
					}
					
					GUILayout.FlexibleSpace();
					
					
					
					using (new GUILayout.VerticalScope("Box"))
					{
						GUILayout.Label(logoheader, GUILayout.Width(233));
						//GUILayout.Label ("version: 1.0");
						
						if (!_runtime)
						{
							if (GUILayout.Button("Documentation", "miniButton"))
							{
								Application.OpenURL("http://databox.doorfortyfour.com/documentation/");
							}
						}
					}
					
					#endif
				}
				else
				{
					#if UNITY_EDITOR
					GUILayout.FlexibleSpace();
					#endif	
				}
			}
		}
		
		
		// Draw all data entries of selected data table.
		// 
		static void DrawDataSheet()
		{
			if (string.IsNullOrEmpty(selectedDatabaseTable))
				return;
			
			
			
			
			using (new GUILayout.VerticalScope())
			{
				
		
				if (!string.IsNullOrEmpty(selectedDatabaseTable))
				{
					using (new GUILayout.HorizontalScope("Box"))
					{
						GUILayout.Space (5);
						selectAllEntries = GUILayout.Toggle(selectAllEntries, "",GUILayout.Width(20));
						
						if (selectAllEntries != lastSelectedAllEntries)
						{
							foreach(var entry in database.DB[selectedDatabaseTable].entries.Keys)
							{
								database.DB[selectedDatabaseTable].entries[entry].selected = selectAllEntries;
							}
							
							lastSelectedAllEntries = selectAllEntries;
						}
						
						//if (tableRenamingActive)
						//{
						//	newTableName = GUILayout.TextField(newTableName, GUILayout.MinWidth(100));
							
						//	if (GUILayout.Button("Rename"))
						//	{
						//		RenameTable(newTableName);
						//	}
						
						//}
						
						GUILayout.FlexibleSpace();

					}
				}
		
				using (var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition, "Box"))
				{
					
					scrollPosition = scrollViewScope.scrollPosition;
					
					//#if UNITY_EDITOR
					//EditorGUI.BeginChangeCheck ();
					//#endif
					
					var _count = 1;
					var _index = 0;
					
					
					try{
					foreach (var entry in database.DB[selectedDatabaseTable].entries.Keys)
					{
						if (_index < database.entriesPerPage * currentPage && _index >= (database.entriesPerPage * currentPage) - database.entriesPerPage)
						{
						
							if (!string.IsNullOrEmpty(searchKey))
							{
								//Debug.Log(searchKey + " " + key);
								if (!entry.ToLower().Contains(searchKey.ToLower()))
								{
									continue;
								}
							}
							
							//GUILayout.Space(2);
							
							#if UNITY_EDITOR
							if (rowBackground != "Box")
							{
								if (_count % 2 == 0)
								{
									rowBackground = "AnimationRowOdd";
								}
								else
								{
									rowBackground = "AnimationRowEven";
								}
							}
							#endif
							
							if (!database.collapsedView)
							{
								GUILayout.Space(5);
							}
							
							//#if UNITY_EDITOR
							//if (EditorGUI.EndChangeCheck ())
							//{
							//	database.errors = DataboxObject.ErrorType.UnsavedChanges;
								
							//	if (database.automaticSave)
							//	{
							//		//database.SaveDatabaseEditor();
							//		EditorCoroutines.Execute(database.SaveDatabaseAsync());
							//	}
							//}
							//#endif
							
							using (new GUILayout.VerticalScope(rowBackground, GUILayout.Height(29)))
							{
							
								GUI.color = Color.white;
								
								using (new GUILayout.HorizontalScope())
								{
									database.DB[selectedDatabaseTable].entries[entry].selected = GUILayout.Toggle(database.DB[selectedDatabaseTable].entries[entry].selected, "", GUILayout.Width(15));
									
								
									
									GUI.color = colBlue;
									GUILayout.Label(_count.ToString(), GUILayout.Width(40));
									GUI.color = Color.white;
									
								
									if (database.collapsedView)
									{
										
										if (entry == selectedFoldout)
										{
											#if UNITY_EDITOR
											GUILayout.Space(10);
											#endif								
		
											if (renamingActive && entry != oldRenamingEntry || !renamingActive)
											{
												if (GUILayout.Button(entry, elementButtonActive, GUILayout.Height(25)))
												{
													#if UNITY_EDITOR
													// double click check
													if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
														DoubleClick(entry, _index);
														
													clickTime = EditorApplication.timeSinceStartup;
													
													#endif
													if (!renamingActive)
													{
														foldout = !foldout;
														if (!foldout)
														{
															selectedFoldout = "";
														}
													}
													
												}
											}
											
										
																		
										}
										else
										{
											if (renamingActive && entry != oldRenamingEntry || !renamingActive)
											{
												if (GUILayout.Button(entry, elementButton, GUILayout.Height(25)))
												{
													#if UNITY_EDITOR
													// double click check
													if ((EditorApplication.timeSinceStartup - clickTime) < doubleClickTime)
														DoubleClick(entry, _index);
													
														clickTime = EditorApplication.timeSinceStartup;
													
													#endif
													
													if (!renamingActive)
													{
														if (selectedFoldout == entry)
														{
															selectedFoldout = "";
														}
														else
														{
															selectedFoldout = entry;
															foldout = true;
														}
													}
													
												}
											}
										
											Event _evt = Event.current;
											Rect _dropArea = GUILayoutUtility.GetLastRect();
											DragAndDrop(_dropArea, _evt, entry);
											
											
											
											
										}
										
										
										#if UNITY_EDITOR
										EditorGUI.BeginChangeCheck ();
										#endif
										
										#if UNITY_EDITOR
										DoubleClickGUI(entry);
										#endif
										
										#if UNITY_EDITOR
										if (EditorGUI.EndChangeCheck ())
										{
											database.errors = DataboxObject.ErrorType.UnsavedChanges;
											
											if (database.automaticSave)
											{
												database.SaveDatabase();
												//EditorCoroutines.Execute(database.SaveDatabaseAsync());
											}
										}
										#endif
									}
									else
									{
										#if UNITY_EDITOR
										if (Application.isEditor && Application.isPlaying)
										{
											GUILayout.Label(entry);
										}
										else		
										{
											GUILayout.Label(entry, editorLargeLabel); //EditorStyles.boldLabel);
										}
										#else
										GUILayout.Label(entry);
										#endif
									}
				
				
									#if UNITY_EDITOR
									EditorGUI.BeginChangeCheck ();
									#endif
									
									#if UNITY_EDITOR
									if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Duplicate"), GUILayout.Height(24), GUILayout.Width(30)))
									#else
									if (GUILayout.Button("d", GUILayout.Width(40)))
									#endif
									{
										duplicateEnabled = !duplicateEnabled;
										duplicateSelectedId = entry;
										duplicateToTable = selectedDatabaseTable;
									}
									
									if (!disableDeleteOption)
									{
										#if UNITY_EDITOR
										if (GUILayout.Button(EditorGUIUtility.IconContent(editorIconClose), GUILayout.Height(24), GUILayout.Width(24))) //GUILayout.Width(20), GUILayout.Height (20)))
										{
											database.RemoveEntry(selectedDatabaseTable, entry);
										}
										#else
										if (GUILayout.Button("x", GUILayout.Width(20)))
										{
											database.RemoveEntry(selectedDatabaseTable, entry);
										}
										#endif
									}
									
									#if UNITY_EDITOR
									if (EditorGUI.EndChangeCheck ())
									{
										database.errors = DataboxObject.ErrorType.UnsavedChanges;
										
										if (database.automaticSave)
										{
											database.SaveDatabase();
											//EditorCoroutines.Execute(database.SaveDatabaseAsync());
										}					
									}
									#endif
								}
								
								#if UNITY_EDITOR
								EditorGUI.BeginChangeCheck ();
								#endif
									
									
								if (duplicateEnabled && duplicateSelectedId == entry)
								{
								
									using (new GUILayout.HorizontalScope("Box"))
									{
										GUILayout.Label("New ID:", GUILayout.Width(100));
										duplicateId = GUILayout.TextField(duplicateId, GUILayout.MinWidth(200));
									
										GUILayout.FlexibleSpace();
										GUILayout.Label("");
										var _dropdownRect = GUILayoutUtility.GetLastRect();
										var _tables = database.DB.Keys.ToArray();
										
										#if UNITY_EDITOR
										if (EditorGUILayout.DropdownButton(new GUIContent(duplicateToTable), FocusType.Keyboard, GUILayout.MinWidth(200)))
										{	
											FieldInfo field = typeof(DataboxEditor).GetField("duplicateToTable");
											if (field != null)
											{
												PopupWindow.Show(_dropdownRect, new PopUps.PopupShowStringList(_tables.ToList(), _dropdownRect, typeof(DataboxEditor), field));
											}
										}
										#else
										duplicateToTable = GUILayout.TextField(duplicateToTable);
										#endif

										if (GUILayout.Button("DUPLICATE", GUILayout.Width(200)))
										{
											duplicateEnabled = false;
											
											var _o = database.DeepCopy<DataboxObject.DatabaseEntry>(database.DB[selectedDatabaseTable].entries[entry]);
											database.DB[duplicateToTable].entries.Add(duplicateId, _o);
										}
									}
								}
								
							
								if ((database.collapsedView && selectedFoldout == entry) || !database.collapsedView)
								{
								
									foreach (var dataKey in database.DB[selectedDatabaseTable].entries[entry].data.Keys)
									{
										var _databaseType = database.DB[selectedDatabaseTable].entries[entry].data[dataKey];
										
										using (new GUILayout.VerticalScope())
										{
											for (int i = 0; i < allTypes.Count; i ++)
											{	
												
												DataboxType _dataType;
												System.Type _type = null;
												try
												{
													_type = System.Type.GetType(allTypes[i]);
												}
												catch
												{
													Debug.LogError("Databox: Could not find type of " + allTypes[i]);
												}
												
												if (_databaseType.TryGetValue(_type, out _dataType))
												{
													if (database.showFieldTypes)
													{
														GUILayout.Label(_type.ToString(), "AssetLabel");
													}
													
													using (new GUILayout.HorizontalScope())
													{
														// indent values
														GUILayout.Space(50);
														using (new GUILayout.HorizontalScope("Box"))
														{
															if (dataKey != valueRenameKey)
															{
																//GUILayout.Label(dataKey + " : ", GUILayout.Width(100));
																if (GUILayout.Button(dataKey + " : ", "Label", GUILayout.Width(100)))
																{
																	valueRenameKey = dataKey;
																	newValueName = dataKey;
																	//valueRenamingActive = true;
																}
															}
															else
															{
																GUI.color = Color.yellow;
																using (new GUILayout.HorizontalScope("Box"))
																{
																	GUI.color = Color.white;
																	
																	oldValueName = dataKey;
																	newValueName = GUILayout.TextField(newValueName);
																	
																	if (renameValueError)
																	{
																		GUILayout.Label("Value ID already exists");
																	}
																	
																	if (GUILayout.Button("Ok", GUILayout.Width(60)))
																	{
																	
																		Dictionary<System.Type, DataboxType> renameOutCheck = new Dictionary<System.Type, DataboxType>();
																		if (!database.DB[selectedDatabaseTable].entries[entry].data.TryGetValue(newValueName, out renameOutCheck))
																		{
																			if (database.debugMode)
																			{
																				Debug.Log("rename " + oldValueName + " to: " + newValueName);
																			}
																		
																			database.RenameValue(selectedDatabaseTable, entry, oldValueName, newValueName);
																			//valueRenamingActive = false;
																			renameValueError = false;
																			valueRenameKey  = "";
																		}
																		else
																		{
																			renameValueError = true;
																		}
																	}
																	
																	if (GUILayout.Button("Cancel", GUILayout.Width(60)))
																	{
																		//valueRenamingActive = false;
																		renameValueError = false;
																		valueRenameKey  = "";
																	}
																}
															}
															
														
															
															using (new GUILayout.VerticalScope())
															{
																//using (new GUILayout.HorizontalScope())
																//{
																	_dataType.DrawEditor();																	
																	_dataType.DrawEditor(database);
																
																//}
																
																if (database.showInitValues)
																{
																	
																	using (new GUILayout.HorizontalScope("Box"))
																	{
																		//GUILayout.Space (10);
																		_dataType.DrawInitValueEditor();
																	}
																}
														
															}
															
															
															#if UNITY_EDITOR
															if (GUILayout.Button(EditorGUIUtility.IconContent(editorIconReset), GUILayout.Width (40)))
															#else
															if (GUILayout.Button("r", GUILayout.Width(40)))
															#endif
															{
																_dataType.Reset();
															}
					
															if (!disableDeleteOption)
															{
																#if UNITY_EDITOR
																if (GUILayout.Button(EditorGUIUtility.IconContent(editorIconClose), GUILayout.Width (20), GUILayout.Height (20)))
																{
																	database.RemoveValue(selectedDatabaseTable, entry, dataKey);
																}
																#else
																if (GUILayout.Button("x", GUILayout.Width(20)))
																{
																	database.RemoveValue(selectedDatabaseTable, entry, dataKey);
																}
																#endif
															}
														}
													}
												}
											}
										}
									}
								}
								
								#if UNITY_EDITOR
								if (EditorGUI.EndChangeCheck ())
								{
									database.errors = DataboxObject.ErrorType.UnsavedChanges;
										
									if (database.automaticSave)
									{
										database.SaveDatabase();
										//EditorCoroutines.Execute(database.SaveDatabaseAsync());
									}					
								}
								#endif
							}	
							
						
						}
						
						_count ++;
						_index ++;
						
						
					}
					
						GUILayout.FlexibleSpace();
					
					}
					catch(System.Exception e)
					{
						if(ExitGUIUtility.ShouldRethrowException(e))
						{
							throw;
						}
						//Debug.LogException(e);
					}
					
					//#if UNITY_EDITOR
					//if (EditorGUI.EndChangeCheck ())
					//{
					//	database.errors = DataboxObject.ErrorType.UnsavedChanges;
			
					//	if (database.automaticSave)
					//	{				
					//		//database.SaveDatabaseEditor();
					//		EditorCoroutines.Execute(database.SaveDatabaseAsync());
					//	}
					
					//}
					//#endif
				}
			
				
				// Page Navigation
				if (database.DB != null && database.DB.Keys.Count > 0)
				{
					DataboxObject.Database _db =  new DataboxObject.Database();
					if (database.DB.TryGetValue(selectedDatabaseTable, out _db))
					{
						entriesCount = database.DB[selectedDatabaseTable].entries.Keys.Count;
						maxPage = Mathf.CeilToInt(entriesCount / database.entriesPerPage);
					
						try
						{
							using (new GUILayout.HorizontalScope("Box"))
							{
								for (int i = 0; i < maxPage; i ++)
								{
									if (i == currentPage - 1)
									{
										GUILayout.Toggle(true, (i+1).ToString(), "Button", GUILayout.Width(30));
									}
									else
									{
										if (GUILayout.Button((i+1).ToString(), GUILayout.Width(30)))
										{
											currentPage = i+1;
										}
									}
								}
							
								GUILayout.FlexibleSpace();
							}
						}
						catch 
						{
						
						}
					}
				}
				
			
			}
				
		
		
		}
		
		
		
		// DATA MODIFICATION METHODS
		////////////////////////////
		
		// Move tables forward
		static void MoveTableForward()
		{
			var _table = database.DB[selectedDatabaseTable];
			database.DB.Remove(selectedDatabaseTable);
			
			database.DB.Insert(selectedDatabaseTableIndex + 1, selectedDatabaseTable, _table);
			
			selectedDatabaseTableIndex ++;
		}
		
		// Move table back
		static void MoveTableBack()
		{
			var _table = database.DB[selectedDatabaseTable];
			database.DB.Remove(selectedDatabaseTable);
			
			database.DB.Insert(selectedDatabaseTableIndex - 1, selectedDatabaseTable, _table);
			
			selectedDatabaseTableIndex --;
		}
		
		static void RenameTable(string _newName)
		{
			var _table = database.DB[selectedDatabaseTable];
			database.DB.Remove(selectedDatabaseTable);
			
			database.DB.Insert(selectedDatabaseTableIndex, _newName, _table);
			
			selectedDatabaseTable = _newName;
		}
		
		
		// handles drag and drop list entry
		static void DragAndDrop(Rect _dropArea, Event _evt, string _entry)
		{
			Profiler.BeginSample("Drag and Drop");
			
			Rect _originalArea = _dropArea;
			_dropArea = new Rect(_dropArea.x - 20, _dropArea.y, 15, _dropArea.height);
			
			#if UNITY_EDITOR
			var _dragDistance = 29f;
			#else
			var _dragDistance = 35f;
			#endif
			
			
			//GUI.Box(_dropArea, "");
			if (_dropArea.Contains(_evt.mousePosition) && !isDragging)
			{
				dragAndDropStartIndex = (int)(_evt.mousePosition.y / ((database.DB[selectedDatabaseTable].entries.Keys.Count * _dragDistance) / database.DB[selectedDatabaseTable].entries.Keys.Count));
				
				
				if (_evt.type == EventType.MouseDrag && !isDragging)
				{

					dragEntry = _entry;
					
					//if (!string.IsNullOrEmpty(dragEntry))
					//{
						isDragging = true;
					//}
					
					_evt.Use ();
				}
			
			}
																	
			if (isDragging)
			{
				GUI.Box(new Rect(_evt.mousePosition.x + 20, _evt.mousePosition.y - 10, _originalArea.width + 20, _dropArea.height + 10), dragEntry, elementButtonActive);
										
				// mouse position / (max height / total entry count)
				dragAndDropIndex = (int)(_evt.mousePosition.y / ((database.DB[selectedDatabaseTable].entries.Keys.Count * _dragDistance) / database.DB[selectedDatabaseTable].entries.Keys.Count));
			
				if (isDragging)
				{
					GUI.color = Color.black;
					GUI.Box(new Rect(0, dragAndDropIndex * _dragDistance, 5000, 5), "");	
					GUI.color = Color.white;
				}
			}
			
			if (_evt.type == EventType.MouseUp && isDragging)
			{			
				isDragging = false;
				
				if (dragAndDropStartIndex + 1 == dragAndDropIndex)
				{
					dragAndDropIndex = dragAndDropStartIndex;
				}
				else if (dragAndDropIndex - dragAndDropStartIndex >= 2)
				{
					dragAndDropIndex --;
				}
				
				Move();
			}
			
			// draw drag handle
			GUI.Box(new Rect(_dropArea.x , _dropArea.y + 10, 10, _dropArea.height - (_dropArea.height - 6)), "", elementDraggingHandle);
			
			Profiler.EndSample();
		}
		
		static void Move()
		{
			if (dragAndDropIndex < 0)
			{
				dragAndDropIndex = 0;
			}
			if (dragAndDropIndex >= database.DB[selectedDatabaseTable].entries.Keys.Count)
			{
				dragAndDropIndex = 	database.DB[selectedDatabaseTable].entries.Keys.Count - 1;
			}
			
			
			var _e = database.DB[selectedDatabaseTable].entries[dragEntry];
			database.DB[selectedDatabaseTable].entries.Remove(dragEntry);
			
			
			database.DB[selectedDatabaseTable].entries.Insert(dragAndDropIndex, dragEntry, _e);
			
			dragEntry = "";
			
			if (database.automaticSave)
			{
				//#if UNITY_EDITOR
				//EditorCoroutines.Execute(database.SaveDatabaseAsync());
				//#else
				database.SaveDatabase();
				//#endif
			}
			else
			{
				//database.errors = DataboxObject.ErrorType.UnsavedChanges;
			}
		}
		
		static void DoubleClick(string _entry, int _index)
		{
			renamingActive = !renamingActive;
			oldRenamingEntry = _entry;
			newRenamingEntry = _entry;
			renamingIndex = _index;
		}
		
		#if UNITY_EDITOR
		static void DoubleClickGUI(string _entry)
		{
			if (renamingActive && _entry == oldRenamingEntry)
			{
				using (new GUILayout.HorizontalScope(elementButton))
				{
					
					//GUILayout.Label("Renaming:", GUILayout.Width(65));
					
					EditorGUI.BeginChangeCheck();
					newRenamingEntry = GUILayout.TextField(newRenamingEntry);
					if (EditorGUI.EndChangeCheck()) 
					{
						DataboxObject.DatabaseEntry _f = null;
						if (database.DB[selectedDatabaseTable].entries.TryGetValue(newRenamingEntry, out _f))
						{
							renamingOK = false;
						}
						else
						{
							renamingOK = true;
						}
					}
				
											
					if (GUILayout.Button("cancel", GUILayout.Width(100)))
					{
						renamingActive = false;
					}
					
					GUI.enabled = renamingOK;
					if (GUILayout.Button("ok", GUILayout.Width(100)))
					{
					
						renamingOK = true;
						renamingActive = false;
						
						var _e = database.DB[selectedDatabaseTable].entries[_entry];
						database.DB[selectedDatabaseTable].entries.Remove(_entry);
						
						database.DB[selectedDatabaseTable].entries.Insert(renamingIndex, newRenamingEntry, _e);
					
					}
					GUI.enabled = true;
				}
			}
		}
		#endif
		
		static void DrawBottom()
		{
			#if UNITY_EDITOR
			
			if (string.IsNullOrEmpty(database.fileName) || string.IsNullOrEmpty(database.savePath))
			{
				EditorGUILayout.HelpBox("Please set a file name and a save path in the config menu before you can add data.", MessageType.Info);
			}
			
			try
			{
				switch (database.errors)
				{
				case DataboxObject.ErrorType.None:
					//
					break;
				case DataboxObject.ErrorType.NoDirectory:
					EditorGUILayout.HelpBox("Directory does not exist", MessageType.Warning);
					break;
				case DataboxObject.ErrorType.NoFileName:
					EditorGUILayout.HelpBox("No filename", MessageType.Warning);				
					break;
				case DataboxObject.ErrorType.UnsavedChanges:
					EditorGUILayout.HelpBox("* Unsaved changes", MessageType.Warning);
					break;
				}
			}catch{}

			#endif
		}

		static void LoadEditorResources()
		{
			#if UNITY_EDITOR
			var _path = System.IO.Path.Combine(GetRelativePath(), "GUI");
			logoheader = (Texture2D)AssetDatabase.LoadAssetAtPath(_path + "/" + "logoheader.png", typeof(Texture2D));
			iconListView = (Texture2D)AssetDatabase.LoadAssetAtPath(_path + "/" + "listView.png", typeof(Texture2D));
			iconFullView = (Texture2D)AssetDatabase.LoadAssetAtPath(_path + "/" + "fullView.png", typeof(Texture2D));
			iconDataType = (Texture2D)AssetDatabase.LoadAssetAtPath(_path + "/" + "dataTypeView.png", typeof(Texture2D));
			iconInitValue = (Texture2D)AssetDatabase.LoadAssetAtPath(_path + "/" + "initValueView.png", typeof(Texture2D));
			#endif
		}
		
		static string GetRelativePath()
		{
			string[] res = System.IO.Directory.GetFiles("Assets/", "DataboxObject.cs", System.IO.SearchOption.AllDirectories);
			if (res.Length == 0)
			{
				//Debug.LogError("error");
				return null;
			}
			string path = res[0].Replace("DataboxObject.cs", "").Replace("\\", "/");
			path = path.Replace("/Core", "");
			
			return path;
		}
		
		public static void GetDataTypes(out List<string> _allTypes, out List<string> _typeNames)
		{
			System.Type[] _types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
			System.Type[] _found = (from System.Type type in _types where type.IsSubclassOf(typeof(DataboxType)) select type).ToArray();
			
			_allTypes = new List<string>();
			_typeNames = new List<string>();
			
			for (int i = 0; i < _found.Length; i++)
			{
				var _attribute = _found[i].GetCustomAttributes(typeof(DataboxTypeAttribute), false);
				if (_attribute.Length > 0)
				{
					var _targetAttribute = _attribute.First() as DataboxTypeAttribute;	
					_typeNames.Add(_targetAttribute.Name);
				}
				else
				{
					_typeNames.Add(_found[i].FullName.ToString());
				}
				
				_allTypes.Add(_found[i].FullName.ToString());
			}
		}
		
		
		public static void Search (string _key, string _tableID)
		{
			searchKey = _key;
			selectedDatabaseTable = _tableID;
		}
		
	}
	#pragma warning restore 0414
}
