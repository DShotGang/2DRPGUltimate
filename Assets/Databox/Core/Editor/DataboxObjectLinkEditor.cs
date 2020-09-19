using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Databox;

namespace Databox.Ed
{

	[CustomEditor(typeof(DataboxObjectLink))]
	public class DataboxObjectLinkEditor : Editor
	{
		public override void OnInspectorGUI() 
		{
			DataboxObjectLink dbObject = (DataboxObjectLink)target;
			
			GUI.color = new Color(125f/255f,215f/255f,220f/255f);
			using (new GUILayout.VerticalScope("Box"))
			{
				GUI.color = Color.white;
				using (new GUILayout.HorizontalScope("Box"))
				{
				GUILayout.Label("Databox object:");
				dbObject.database = EditorGUILayout.ObjectField(dbObject.database, typeof(DataboxObject), false) as DataboxObject;
				}
				using (new GUILayout.HorizontalScope("Box"))
				{
				GUILayout.Label("Table ID:");
				dbObject.tableID = GUILayout.TextField(dbObject.tableID);
				}
				using (new GUILayout.HorizontalScope("Box"))
				{
				GUILayout.Label("Object ID:");
				dbObject.objectID = GUILayout.TextField(dbObject.objectID);
				}
			}
			
			
			GUILayout.Label("Data", EditorStyles.boldLabel);
			using (new GUILayout.VerticalScope("Box"))
			{
				if (dbObject.database != null && !string.IsNullOrEmpty(dbObject.tableID) && !string.IsNullOrEmpty(dbObject.objectID))
				{
					if (!dbObject.database.DB.ContainsKey(dbObject.tableID))
						return;
					if (!dbObject.database.DB[dbObject.tableID].entries.ContainsKey(dbObject.objectID))
						return;
						
					foreach (var data in dbObject.database.DB[dbObject.tableID].entries[dbObject.objectID].data.Keys)
					{
						using (new GUILayout.HorizontalScope("Box"))
						{
							GUILayout.Label(data);
							foreach(var field in dbObject.database.DB[dbObject.tableID].entries[dbObject.objectID].data[data].Keys)
							{
								var _d = dbObject.database.DB[dbObject.tableID].entries[dbObject.objectID].data[data][field] as DataboxType;
								
								_d.DrawEditor();
							
							}
						}
					}
				
				
					if (GUILayout.Button("Save"))
					{
						dbObject.database.SaveDatabase();
					}
				}
				
			}
			
			EditorUtility.SetDirty(dbObject);
		}
	}
}