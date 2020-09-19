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
	[CustomActionEditor(typeof(RemoveValue))]
	public class RemoveValueEditor : CustomActionEditor
	{
		public override void OnEnable(){}
		
		public override bool OnGUI()
		{
			var action = target as RemoveValue;
	
			EditorGUI.BeginChangeCheck();
			
			EditField("databoxObject");
			
		
			if (!action.databoxObject.IsNone)
			{
				
				DrawDataboxSelectionPopup.Draw(action.databoxObject.Value as DataboxObject, DrawDataboxSelectionPopup.DrawType.tableEntryValue, 
					action.data, out action.data);
				
			}
	
			var isDirty = EditorGUI.EndChangeCheck();
			
			return isDirty || GUI.changed;
		}
	}
}
#endif