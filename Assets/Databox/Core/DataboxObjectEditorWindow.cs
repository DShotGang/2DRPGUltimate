#if UNITY_EDITOR
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Databox.Ed
{
	// Opens a new inspector tab with selected Databox object.
	//
	public class DataboxObjectEditorWindow : EditorWindow {

		public static DataboxObject dbObject;
		
		public static void Init() 
		{
			// Retrieve the existing Inspector tab, or create a new one if none is open
			EditorWindow inspectorWindow = EditorWindow.GetWindow( typeof( Editor ).Assembly.GetType( "UnityEditor.InspectorWindow" ) );
			// Get the size of the currently window
			Vector2 size = new Vector2( inspectorWindow.position.width, inspectorWindow.position.height );
			// Clone the inspector tab (optionnal step)
			inspectorWindow = Instantiate( inspectorWindow );
			// Set min size, and focus the window
			inspectorWindow.minSize = size;
			inspectorWindow.Show();
			inspectorWindow.Focus();
			
			// lock the inspector window
			Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");
			PropertyInfo propertyInfo = type.GetProperty("isLocked");
			propertyInfo.SetValue(inspectorWindow, true, null);
			inspectorWindow.Repaint();
			
			//ActiveEditorTracker.sharedTracker.isLocked = true;
		}
		
		
		void OnGUI() 
		{
			if (dbObject != null)
			{
				DataboxEditor.DrawEditor(dbObject);
				EditorUtility.SetDirty(dbObject);
			}
		}
	}
}
#endif