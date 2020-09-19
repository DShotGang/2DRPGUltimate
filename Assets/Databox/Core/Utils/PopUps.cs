using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Databox.Ed
{
	public class PopUps
	{
		#if UNITY_EDITOR
		public class PopupShowStringList : PopupWindowContent
		{
			List<string> selections = new List<string>();
			string searchTypeString = "";
			Rect rect = new Rect(0,0,0,0);
			Vector2 scrollPosition = Vector2.zero;
			string selected  = "";
			FieldInfo field;
			object targetObject;
			
			public override Vector2 GetWindowSize()
			{
				return new Vector2(200, 200);
			}
		
			public override void OnGUI(Rect rect)
			{	
				GUILayout.Label("Table", EditorStyles.boldLabel);
			
				
				using (new GUILayout.HorizontalScope())
				{
					GUI.SetNextControlName ("Filter");
					searchTypeString = GUILayout.TextField(searchTypeString, "SearchTextField");
					
					if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton")))
					{
						searchTypeString = "";
					}
				}
			
				var _index = 0;
				
				using (var scrollView = new GUILayout.ScrollViewScope(scrollPosition))
				{
					scrollPosition = scrollView.scrollPosition;
				
					foreach(var entry in selections)
					{
						//Debug.Log(searchTypeString);
						if (entry.ToLower().Contains(searchTypeString.ToLower()) || string.IsNullOrEmpty(searchTypeString))
						{
							using (new GUILayout.HorizontalScope())
							{
								if (GUILayout.Button(entry.ToString()))
								{
									//DataboxEditor.duplicateToTable = entry.ToString();
									searchTypeString = "";
									editorWindow.Close();
									selected = entry.ToString();
									
									field.SetValue(targetObject, entry.ToString());
								}
							}
						}
						
						_index ++;
					}
				}
			}
		
			public override void OnOpen()
			{
				//Debug.Log("Popup opened: " + this);
				EditorGUI.FocusTextInControl ("Filter");
			}
		
			public override void OnClose()
			{
				//Debug.Log("Popup closed: " + this);
			
			}
			
		
			public PopupShowStringList(List<string> _tables, Rect _rect, object _targetObject, FieldInfo _field)
			{
				selections = new List<string>(_tables);
				rect = _rect;
				field = _field;
				targetObject = _targetObject;
			}
		}


		public class PopupType : PopupWindowContent
		{
			List<string> types = new List<string>();
			string searchTypeString = "";
			Rect rect = new Rect(0,0,0,0);
			Vector2 scrollPosition = Vector2.zero;
			int inspectorWidth;
			int resultsFound = 0;
			int index2 = 0;
			
			public override Vector2 GetWindowSize()
			{
				return new Vector2(280, 200); //types.Count * 22);
			}

			public override void OnGUI(Rect rect)
			{	
				GUILayout.Label("Types", EditorStyles.boldLabel);
				
				var _index = 0;
				
				// Check for return key
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
				{
					if (resultsFound == 1)
					{
						if (!string.IsNullOrEmpty(searchTypeString))
						{						
							DataboxEditor.selectedTypeIndex = index2;
							searchTypeString = "";
							editorWindow.Close();
							return;
						}
					}
				}
				
				using (new GUILayout.HorizontalScope())
				{
					GUI.SetNextControlName ("FilterTypes");
					searchTypeString = GUILayout.TextField(searchTypeString, "SearchTextField");
					
					if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton")))
					{
						searchTypeString = "";
					}
				}
				
				var _r = 0;
				using (var scrollView = new GUILayout.ScrollViewScope(scrollPosition))
				{
					scrollPosition = scrollView.scrollPosition;
					
					foreach(var entry in types)
					{
						//Debug.Log(searchTypeString);
						if (entry.ToLower().Contains(searchTypeString.ToLower()) || string.IsNullOrEmpty(searchTypeString))
						{
							using (new GUILayout.HorizontalScope())
							{
								if (GUILayout.Button(entry.ToString()))
								{
									DataboxEditor.selectedTypeIndex = _index;
									searchTypeString = "";
									editorWindow.Close();
									
								}
							}	
							
							_r ++;	
							index2 = _index;
						}
						
						if (entry.ToLower().Contains(searchTypeString.ToLower()))
						{
							
						}
						
						_index ++;
					}
				}
				
				EditorGUI.FocusTextInControl ("FilterTypes");
				
				resultsFound = _r;
			}

			public override void OnOpen()
			{
				//Debug.Log("Popup opened: " + this);
				EditorGUI.FocusTextInControl ("FilterTypes");
			}

			public override void OnClose()
			{
				//Debug.Log("Popup closed: " + this);
			}
		
			public PopupType(List<string> _types, Rect _rect, int _inspectorWidth)
			{
				types = new List<string>(_types);
				rect = _rect;
				inspectorWidth = _inspectorWidth;
			}
		}
		#endif
	}
}