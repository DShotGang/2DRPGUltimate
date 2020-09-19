using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Databox.Ed
{
	[CustomEditor(typeof(DataboxObjectManager))]
	public class DataboxObjectManagerEditor : Editor
	{
		DataboxObjectManager manager;

		int selectedObject = 0;
		
		bool renamingActive = false;
		string newName = "";
		bool isDragging;
		
		public override void OnInspectorGUI() 
		{
			manager = (DataboxObjectManager)target;
		
		
			if (manager.dataObjects.Count == 0)
			{
				EditorGUILayout.HelpBox("Manage all of your DataboxObjects in one place. Simply drag your Databox Objects onto this field.", MessageType.Info);
			}
			
			DropAreaGUI();
			
			try{
				if (manager.dataObjects.Count > 0)
				{
					using (new GUILayout.HorizontalScope("Box"))
					{	
						using (new GUILayout.VerticalScope("TextArea"))
						{
							for (int i = 0; i < manager.dataObjects.Count; i ++)
							{
								if (i == selectedObject)
								{
									GUI.color = new Color(125f/255f, 185f/255f, 190f/255f);
								}
								else
								{
									GUI.color = Color.white;
								}
								
								if (GUILayout.Button(manager.dataObjects[i].id, GUILayout.Width(100), GUILayout.Height(30)))
								{
									selectedObject = i;
								}
								
								GUI.color = Color.white;
							}
						}
						
						using (new GUILayout.VerticalScope())
						{
							for (int i = 0; i < manager.dataObjects.Count; i ++)
							{
						
							
									if (i == selectedObject)
									{
										Toolbar(manager);
										
										GUILayout.Space(5);
											if (i < manager.dataObjects.Count)
								{
										DataboxEditor.DrawEditor(manager.dataObjects[i].dataObject);
										
										EditorUtility.SetDirty(manager.dataObjects[i].dataObject);
									}
								}
							}
						
						}
					}
				}
			}
				catch{}
			
			EditorUtility.SetDirty(manager);
		}
		
		void Toolbar(DataboxObjectManager _manager)
		{
			using (new GUILayout.HorizontalScope("Toolbar"))
			{
				if (!renamingActive)
				{
					GUILayout.Label(_manager.dataObjects[selectedObject].id);
					
					if (GUILayout.Button("Rename", "toolbarButton", GUILayout.Width(100)))
					{
						newName = _manager.dataObjects[selectedObject].id;
						renamingActive = true;
					}
				}
				else
				{
					newName = GUILayout.TextField(newName);
					
					if (GUILayout.Button("OK", "toolbarButton"))
					{
						_manager.dataObjects[selectedObject].id = newName;
						renamingActive = false;
					}
					
					if (GUILayout.Button("Cancel", "toolbarButton"))
					{
						renamingActive = false;
					}
				}
				
				if (GUILayout.Button("select", "toolbarButton", GUILayout.Width(100)))
				{
					Selection.activeObject = _manager.dataObjects[selectedObject].dataObject;
				}
				
				if (GUILayout.Button("-", "toolbarButton", GUILayout.Width(30)))
				{
					_manager.dataObjects.RemoveAt(selectedObject);
				}
			}
		}
		
		
		void DropAreaGUI()
		{
	
			Event _evt = Event.current;
			Rect _dropArea = GUILayoutUtility.GetRect(120f, 40f);
		
			if (isDragging)
			{
				GUI.color = Color.green;
			}
			else
			{
				GUI.color = Color.white;
			}
			GUI.Box (_dropArea, "Drag new Databox Object here");
			GUI.color = Color.white;
			switch (_evt.type)
			{
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (!_dropArea.Contains(_evt.mousePosition))
				{
					isDragging = false;
					return;
				}
				else
				{
					isDragging = true;
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					
					if (_evt.type == EventType.DragPerform)
					{
						isDragging = false;
						DragAndDrop.AcceptDrag();
						foreach (Object _dobj in DragAndDrop.objectReferences)
						{
							//obj = _dobj;
							if (_dobj.GetType() == typeof(DataboxObject))
							{
								manager.dataObjects.Add(new Databox.DataboxObjectManager.DataboxObjects(_dobj.name, _dobj as DataboxObject));
							}
						}
					}
					
				}
				break;
			}
		}
	}
}