#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Databox;
using Databox.PlayMaker;

namespace Databox.PlayMaker.Editor
{
	public static class DrawDataboxSelectionPopup
	{
		static string table;
		static string value;
		
		public enum DrawType
		{
			tableEntryValue,
			tableEntry,
			table
		}
		
		public static void Draw(DataboxObject databoxObject, DrawType drawType,
			Data data, out Data outData)
		{
			outData = data;
			
			outData.selectedTable = data.selectedTable;
			outData.selectedEntry = data.selectedEntry;
			outData.selectedValue = data.selectedValue;
			
			outData.selectedTableIndex = data.selectedTableIndex;
			outData.selectedEntryIndex = data.selectedEntryIndex;
			outData.selectedValueIndex = data.selectedValueIndex;
			
			outData.nonExistent = GUILayout.Toggle(outData.nonExistent, "non existent entry");
			
			if (!outData.nonExistent)
			{
				if (drawType == DrawType.table || drawType == DrawType.tableEntry || drawType == DrawType.tableEntryValue)
				{
					var _db = databoxObject;
					var _tables = _db.DB.Keys.ToList();
					string[] _t = new string[_tables.Count];
					for (int s = 0; s < _tables.Count; s ++)
					{
						_t[s] = _tables[s];
					}
					
					using (new GUILayout.VerticalScope("Box"))
					{
						outData.selectedTableIndex = EditorGUILayout.Popup("Table:", outData.selectedTableIndex , _t);
						outData.selectedTable = _tables[outData.selectedTableIndex];
					}
					
			
			
					if (!string.IsNullOrEmpty(outData.selectedTable) &&
					(drawType == DrawType.tableEntry || drawType == DrawType.tableEntryValue))
					{
						var _entries = _db.DB[outData.selectedTable].entries.Keys.ToList();
						string[] _e = new string[_entries.Count];
						for (int e = 0; e < _entries.Count; e++)
						{
							_e[e] = _entries[e];
						}
							
						using (new GUILayout.VerticalScope("Box"))
						{
							
							GUILayout.Label("Entry:");
							
							using (new GUILayout.HorizontalScope())
							{
								outData.useOwnerInstanceID = GUILayout.Toggle(outData.useOwnerInstanceID, "use owner instance id");
								
								
								if (!outData.useOwnerInstanceID)
								{
									outData.selectedEntryIndex = EditorGUILayout.Popup("", outData.selectedEntryIndex, _e);
								}
								else
								{
									GUILayout.Label("Instance ID");
								}
							}
						}
						
						outData.selectedEntry = _entries[outData.selectedEntryIndex];
						
						if (!string.IsNullOrEmpty(outData.selectedEntry) &&
						drawType == DrawType.tableEntryValue)
						{
							var _values = _db.DB[outData.selectedTable].entries[outData.selectedEntry].data.Keys.ToList();
							string[] _v = new string[_values.Count];
							for (int v = 0; v < _values.Count; v ++)
							{
								_v[v] = _values[v];
							}
							
							using (new GUILayout.VerticalScope("Box"))
							{
								outData.selectedValueIndex = EditorGUILayout.Popup("Value:", outData.selectedValueIndex, _v);
								outData.selectedValue = _values[outData.selectedValueIndex];
							}
						}
					}
				}
			
				
			}
			else
			{
				if (drawType == DrawType.tableEntryValue || drawType == DrawType.tableEntry || drawType == DrawType.table)
				{
					using (new GUILayout.HorizontalScope("Box"))
					{
						GUILayout.Label("Table:");
						outData.selectedTable = GUILayout.TextField(outData.selectedTable);
					}
					
					if (drawType == DrawType.tableEntryValue || drawType == DrawType.tableEntry)
					{
						using (new GUILayout.HorizontalScope("Box"))
						{
							GUILayout.Label("Entry:");
							
							using (new GUILayout.HorizontalScope())
							{
								outData.useOwnerInstanceID = GUILayout.Toggle(outData.useOwnerInstanceID, "use owner instance id");
								
								if (!outData.useOwnerInstanceID)
								{
									outData.selectedEntry = GUILayout.TextField(outData.selectedEntry);
								}
								else
								{
									GUILayout.Label("Instance ID");
								}
							}
						}
						
						if (drawType == DrawType.tableEntryValue)
						{
							using (new GUILayout.HorizontalScope("Box"))
							{
								GUILayout.Label("Value:");
								outData.selectedValue = GUILayout.TextField(outData.selectedValue);
							}
						}
					}
				}
			}
			
		}
	}
}
#endif