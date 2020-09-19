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
	[CustomActionEditor(typeof(SetData))]
	public class SetDataEditor : CustomActionEditor
	{
		
		public override void OnEnable(){}
		
		public override bool OnGUI()
		{
			var action = target as SetData;
	
			EditorGUI.BeginChangeCheck();
			
			using (new GUILayout.VerticalScope("Box"))
			{
				GUILayout.Label("From Variable:", "boldLabel");
				
				action.dataType = (SetData.DataType)EditorGUILayout.EnumPopup(action.dataType);
				EditField("from" + action.dataType.ToString());
			}
			
			using (new GUILayout.VerticalScope("Box"))
			{
			
				GUILayout.Label("To Databox:", "boldLabel");
				EditField("databoxObject");
				
				
				if (!action.databoxObject.IsNone)
				{
					DrawDataboxSelectionPopup.Draw(action.databoxObject.Value as DataboxObject, DrawDataboxSelectionPopup.DrawType.tableEntryValue, 
						action.data, out action.data);
				}
				
			}
	    
			var isDirty = EditorGUI.EndChangeCheck();
			
			return isDirty || GUI.changed;
		}
		
	}
}
#endif