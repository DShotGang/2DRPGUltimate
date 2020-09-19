#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using Databox.PlayMaker;
using Databox;
using System.Linq;

namespace Databox.PlayMaker.Editor
{
	[CustomActionEditor(typeof(RegisterToDatabase))]
	public class RegisterToDatabaseEditor : CustomActionEditor
	{
		public override void OnEnable(){}
		
		public override bool OnGUI()
		{
			var action = target as RegisterToDatabase;
	
			EditorGUI.BeginChangeCheck();
			
			using (new GUILayout.VerticalScope("Box"))
			{
				GUILayout.Label("From:", "boldLabel");
				
				EditField("databoxObject");
			
				if (!action.databoxObject.IsNone)
				{
					var _db = action.databoxObject.Value as DataboxObject;
					var _tables = _db.DB.Keys.ToList();
					string[] _t = new string[_tables.Count];
					for (int s = 0; s < _tables.Count; s ++)
					{
						_t[s] = _tables[s];
					}
			
			
					action.selectedTableIndex = EditorGUILayout.Popup("Table:", action.selectedTableIndex, _t);
					action.selectedTable = _tables[action.selectedTableIndex];
					
					if (!string.IsNullOrEmpty(action.selectedTable))
					{
						var _entries = _db.DB[action.selectedTable].entries.Keys.ToList();
						string[] _e = new string[_entries.Count];
						for (int e = 0; e < _entries.Count; e++)
						{
							_e[e] = _entries[e];
						}
					
			
						action.selectedEntryIndex = EditorGUILayout.Popup("Entry:", action.selectedEntryIndex, _e);
						action.selectedEntry = _entries[action.selectedEntryIndex];
					
					}
					
				}
			
			}
			
			using (new GUILayout.VerticalScope("Box"))
			{
				GUILayout.Label("To:", "boldLabel");
				
				EditField("toDataboxObject");
				
				
				if (!action.toDataboxObject.IsNone)
				{
					action.useOwnerInstanceID = GUILayout.Toggle(action.useOwnerInstanceID, "use owner instance id");
					
					if (!action.useOwnerInstanceID)
					{
						using (new GUILayout.HorizontalScope())
						{
							GUILayout.Label("Entry ID:");
							action.toEntryID = GUILayout.TextField(action.toEntryID);
						}
					}
				}
			}
	
			var isDirty = EditorGUI.EndChangeCheck();
			
			return isDirty || GUI.changed;
		}
	}
}
#endif