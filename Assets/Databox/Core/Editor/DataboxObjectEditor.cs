using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Databox;

namespace Databox.Ed
{
	[CustomEditor(typeof(DataboxObject))]
	public class DataboxObjectEditor : Editor {
		
		public override void OnInspectorGUI() 
		{
			DataboxObject dbObject = (DataboxObject)target;
			
			DataboxEditor.DrawEditor(dbObject);
			
			
			EditorUtility.SetDirty(dbObject);
		}
		
		[OnOpenAssetAttribute(1)]
		public static bool OpenDataboxObject(int instanceID, int line)
		{
			object _obj = EditorUtility.InstanceIDToObject(instanceID);
			var _db = _obj as DataboxObject;
		
			if (_db != null)
			{
				DataboxObjectEditorWindow.dbObject = _db;
				DataboxObjectEditorWindow.Init();		
			
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}