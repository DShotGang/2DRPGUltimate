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
	[CustomActionEditor(typeof(AddData))]
	public class AddDataEditor : CustomActionEditor
	{
		
		public override void OnEnable(){}
		
		public override bool OnGUI()
		{
			var action = target as AddData;
	
			EditorGUI.BeginChangeCheck();
			
			EditField("databoxObject");
			
		
			if (!action.databoxObject.IsNone)
			{
				using (new GUILayout.HorizontalScope("Box"))
				{
					GUILayout.Label("Table ID:");
					action.tableID = GUILayout.TextField(action.tableID);
				}
			
				
				using (new GUILayout.VerticalScope("Box"))
				{
					GUILayout.Label("Entry ID:");
						
					using (new GUILayout.HorizontalScope())
					{
						action.useOwnerInstanceID = GUILayout.Toggle(action.useOwnerInstanceID, "use owner instance id");
					
						if (!action.useOwnerInstanceID)
						{	
							
							action.entryID = GUILayout.TextField(action.entryID);
						}
						else
						{
							GUILayout.Label(" Instance ID");
						}
					}
				}
				
				using (new GUILayout.HorizontalScope("Box"))
				{
					GUILayout.Label("Value ID:");
					action.valueID = GUILayout.TextField(action.valueID);
				}
			
				using (new GUILayout.HorizontalScope("Box"))
				{
					GUILayout.Label("Type:");
					GUILayout.FlexibleSpace();
					action.dataType = (AddData.DataType)EditorGUILayout.EnumPopup(action.dataType);
				}   
			
			}
	
			var isDirty = EditorGUI.EndChangeCheck();
			
			return isDirty || GUI.changed;
		}
		
	}
}
#endif