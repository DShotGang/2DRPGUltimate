using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using Databox;
using Databox.Dictionary;

namespace Databox.Ed
{
	public class DataboxCloudConfigEditor
	{
		
	    // Start is called before the first frame update
		public static void DrawCloudConfigUI(DataboxObject database)
	    {
		    using (new GUILayout.VerticalScope("Window"))
		    {
			    //if (GUILayout.Button("Remove Agreement"))
			    //{
			    //	EditorPrefs.DeleteKey("DataboxCloudAgreement");
			    //}
						
				#if UNITY_EDITOR
			    DataboxEditor.cloudAgreementAccepted = EditorPrefs.GetBool("DataboxCloudAgreement");
				#endif
						
			    if (!DataboxEditor.cloudAgreementAccepted)
			    {
				    using (new GUILayout.VerticalScope("Box"))
				    {
					    GUI.color = Color.yellow;
					    GUILayout.Label("Please accept this short agreement: ");
					    GUI.color = Color.white;
					    GUILayout.Label(DataboxEditor.cloudAgreement);
								
					    DataboxEditor.cloudAgreementAccepted = GUILayout.Toggle(DataboxEditor.cloudAgreementAccepted, "Accept");
								
						#if UNITY_EDITOR
					    if (DataboxEditor.cloudAgreementAccepted)
					    {
						    EditorPrefs.SetBool("DataboxCloudAgreement", DataboxEditor.cloudAgreementAccepted);
					    }
						#endif
				    }
			    }
			    else
			    {
							
				    using (new GUILayout.VerticalScope("Box"))
				    {
				    	// ONLY FOR DEBUG
				    	//GUILayout.Label("server time: " + DataboxCloud.serverTime);
					    //GUILayout.Label("server version: " + DataboxCloud.serverVersion);
					    //GUILayout.Label("database version: " + database.cloudVersion);
					    
					    GUILayout.Label("Cloud server url:");
					    database.cloudServer = GUILayout.TextField(database.cloudServer);
					    GUILayout.Label("Unique Id: (only integer numbers)");
					    //database.cloudId = GUILayout.TextField(database.cloudId);
					    if (!string.IsNullOrEmpty(database.cloudId))
					    {
						    var _cloudId = int.Parse(database.cloudId);  
						    database.cloudId = GUIIntField.IntField(_cloudId).ToString();
					    }
					    else
					    {
					    	database.cloudId = "0";
					    }
					    
					    if (string.IsNullOrEmpty(database.cloudId))
					    {
					    	#if UNITY_EDITOR
					    	EditorGUILayout.HelpBox("Please enter an unique id", MessageType.Warning);
					    	#else
					    	GUILayout.Label("Please enter an unique id");
					    	#endif
					    }
				    }
							
				    //database.cloudWarnings = (DataboxObject.CloudWarnings)EditorGUILayout.EnumPopup("", database.cloudWarnings);
							
							
				    if (!string.IsNullOrEmpty(database.cloudId))
				    {
					    using (new GUILayout.HorizontalScope("Box"))
					    {
						    if (GUILayout.Button("1. Check"))
						    {
									
								#if UNITY_EDITOR
								
					    		DataboxCloud.Check(database.cloudServer, database.cloudId, database);
							    
								#else
							    if (database.databoxCloudService == null)
							    {
											database.databoxCloudService = 	new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
							    }
										
							    DataboxCloud.CheckRuntime(database);
								#endif
										
							    database.cloudCheck = true;
					
						    }
									
						    if ((database.cloudWarnings == DataboxObject.CloudWarnings.ok ||
							    database.cloudWarnings == DataboxObject.CloudWarnings.warning) && database.cloudCheck)
						    {
							    GUI.enabled = true;
						    }
						    else
						    {
							    GUI.enabled = false;
						    }
						    
						    if (GUILayout.Button("2. Sync & Compare"))
						    {
								#if UNITY_EDITOR
							    DataboxCloud.Sync(database.cloudServer, database.cloudId, database);
							    //databoxCloudInstance.Sync(database.cloudServer, database.cloudId, database);
								#else
							    if (database.databoxCloudService == null)
							    {
											database.databoxCloudService = 	new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
							    }
										
							    DataboxCloud.SyncRuntime(database);
										#endif
							    database.cloudCheck = false;
						    }
						    
						    GUI.enabled = true;
										
						    if (database.oldDB == null)
						    {
							    GUI.enabled = false;
						    }
						    else
						    {
							    if (database.oldDB.Keys.Count > 0)
							    {
								    if (DataboxEditor.deletedCompare.tables.Count > 0 || DataboxEditor.deletedCompare.entries.Count > 0 ||
									    DataboxEditor.deletedCompare.fields.Count > 0 || DataboxEditor.deletedCompare.values.Count > 0 ||
									    DataboxEditor.newCompare.tables.Count > 0 || DataboxEditor.newCompare.entries.Count > 0 ||
									    DataboxEditor.newCompare.fields.Count > 0 || DataboxEditor.newCompare.values.Count > 0)
								    {
									    GUI.enabled = true;
								    }
								    else
								    {
									    GUI.enabled = false;
								    }
							    }
							    else
							    {
								    GUI.enabled = false;
						    	}
					    	}
								
						    if (GUILayout.Button("3. Revert"))
						    {
							    database.DB = new OrderedDictionary<string, DataboxObject.Database>();
							    database.DB = database.DeepCopy<OrderedDictionary<string, DataboxObject.Database>>(database.oldDB);
										
							    DataboxEditor.deletedCompare = new DataboxCloud.ChangeLog();
							    DataboxEditor.newCompare = new DataboxCloud.ChangeLog();
										
							    database.oldDB = new OrderedDictionary<string, DataboxObject.Database>();
										
							    database.cloudVersion = database.oldCloudVersion;
										
							    database.cloudStatus += "- Reverted back to before sync" + "\n";
						    }
						    GUI.enabled = true;
					    }
			    	
							
					    using (new GUILayout.HorizontalScope())
					    {
						    if ((database.cloudWarnings == DataboxObject.CloudWarnings.ok ||
							    database.cloudWarnings == DataboxObject.CloudWarnings.warning) && database.cloudCheck)
						    {
							    GUI.enabled = true;
						    }
						    else
						    {
							    GUI.enabled = false;
						    }
						    if (GUILayout.Button("Force Upload"))
						    {
										#if UNITY_EDITOR
							    DataboxCloud.ForceUploadEditor();
										#else
							    if (database.databoxCloudService == null)
							    {
											database.databoxCloudService = 	new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
							    }
										
							    DataboxCloud.ForceUploadRuntime(database);
										#endif
										
							    database.cloudCheck = false;
						    }
									
						    if (GUILayout.Button("Force Download"))
						    {
										#if UNITY_EDITOR
							    DataboxCloud.ForceDownloadEditor();
										#else
							    if (database.databoxCloudService == null)
							    {
											database.databoxCloudService = 	new GameObject("DataboxCloudService").AddComponent<DataboxCloud>() as DataboxCloud;
							    }
										
							    DataboxCloud.ForceDownloadRuntime(database);
										#endif
										
							    database.cloudCheck = false;
						    }
						    GUI.enabled = true;
					    }
				    
				    }
							
				    GUILayout.Label("Cloud status:");
							
				    switch (database.cloudWarnings)
				    {
				    case DataboxObject.CloudWarnings.ok:
					    GUI.color = Color.green;
					    break;
				    case DataboxObject.CloudWarnings.warning:
					    GUI.color = Color.yellow;
					    break;
				    case DataboxObject.CloudWarnings.error:
					    GUI.color = Color.red;
					    break;
				    }
				    using (new GUILayout.VerticalScope("Box"))
				    {
					    GUI.color = Color.white;
								
								
					    GUILayout.Label(database.cloudStatus);
				    }
							
				    // COMPARE GUI
				    GUILayout.Label ("Changes to cloud version:");
				    // DELETED
				    // Tables
				    if (DataboxEditor.deletedCompare != null)
				    {
					    GUI.color = Color.red;
					    for (int m = 0; m < DataboxEditor.deletedCompare.tables.Count; m ++)
					    {
						    //Debug.Log("deleted table: " + deletedCompare.tables[m]);
						    //database.cloudStatus += "deleted table: " + deletedCompare.tables[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("deleted table: " + DataboxEditor.deletedCompare.tables[m]);
						    }
					    }
					    // Entries
					    for (int m = 0; m < DataboxEditor.deletedCompare.entries.Count; m ++)
					    {
						    //Debug.Log("deleted entry: " + deletedCompare.entries[m]);						
						    //database.cloudStatus += "deleted entry: " + deletedCompare.entries[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("deleted entry: " + DataboxEditor.deletedCompare.entries[m]);
						    }
					    }
					    // Values
					    for (int m = 0; m < DataboxEditor.deletedCompare.fields.Count; m ++)
					    {
						    //Debug.Log("deleted field: " + deletedCompare.fields[m]);
						    //database.cloudStatus += "deleted field: " + deletedCompare.fields[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("deleted field: " + DataboxEditor.deletedCompare.fields[m]);
						    }
					    }
					    GUI.color = Color.white;
				    }		
							
				    if (DataboxEditor.newCompare != null)
				    {
					    // NEW
					    // Tables
					    GUI.color = Color.green;
					    for (int m = 0; m < DataboxEditor.newCompare.tables.Count; m ++)
					    {
						    //Debug.Log("new table: " + newCompare.tables[m]);
						    //database.cloudStatus += "new table: " + newCompare.tables[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("new table: " + DataboxEditor.newCompare.tables[m]);
						    }
					    }
					    // Entries
					    for (int m = 0; m < DataboxEditor.newCompare.entries.Count; m ++)
					    {
						    //Debug.Log("new entry: " + newCompare.entries[m]);
						    //database.cloudStatus += "new entry: " + newCompare.entries[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("new entry: " + DataboxEditor.newCompare.entries[m]);
						    }
					    }
					    // Values
					    for (int m = 0; m < DataboxEditor.newCompare.fields.Count; m ++)
					    {
						    //Debug.Log("new fields: " + newCompare.fields[m]);
						    //database.cloudStatus += "new field: " + newCompare.fields[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("new value: " + DataboxEditor.newCompare.fields[m]);
						    }
					    }
					    GUI.color = Color.white;
								
							
					    //Modified
					    GUI.color = Color.yellow;
					    for (int m = 0; m < DataboxEditor.newCompare.values.Count; m ++)
					    {
						    //Debug.Log("modified: " + newCompare.values[m]);
						    //database.cloudStatus += "modified value: " + newCompare.values[m] + "\n";
						    using (new GUILayout.VerticalScope("Box"))
						    {
							    GUILayout.Label("modified value: " + DataboxEditor.newCompare.values[m]);
						    }
					    }
					    GUI.color = Color.white;
				    }
							
				    if (DataboxEditor.deletedCompare == null && DataboxEditor.newCompare == null)
				    {
					    using (new GUILayout.VerticalScope("Box"))
					    {
						    GUILayout.Label("none");
					    }
				    }
			    }
		    }
	    }
	}
}
