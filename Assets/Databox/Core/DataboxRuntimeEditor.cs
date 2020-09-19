using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox.Ed;

namespace Databox
{
	public class DataboxRuntimeEditor : MonoBehaviour {
		
		
		public DataboxObjectManager manager;
		
		public bool disableInspector;
		public bool disableDeleteOption;
		public bool disableConfigurationOption;
		public bool disableCloudOption;
		
		public GUISkin editorSkin;
		
		int selectedDB = 0;
		

		void OnGUI()
		{
			if (manager == null)
			{
				GUILayout.Label("No Databox manager assigned");
				return;
			}
			
			using (new GUILayout.HorizontalScope())
			{
				
				for (int d = 0; d < manager.dataObjects.Count; d ++)
				{
					if (editorSkin != null)
					{
						GUI.skin = editorSkin;
					}
				
				
					if (GUILayout.Button(manager.dataObjects[d].id))
					{
						selectedDB = d;
						manager.dataObjects[d].editorOpen = !manager.dataObjects[d].editorOpen;
					}
				}
			}
			
			for (int d = 0; d < manager.dataObjects.Count; d ++)
			{	
				if (manager.dataObjects[d].dataObject != null && selectedDB == d)
				{
					DataboxEditor.DrawEditorRuntime(
						manager.dataObjects[d].dataObject, 
						editorSkin, 
						disableInspector,
						disableDeleteOption,
						disableConfigurationOption,
						disableCloudOption,
						manager.dataObjects[d].editorOpen);
				}
			}
		}
	}
}