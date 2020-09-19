using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Resource")]
public class ResourceType : DataboxType
{
	[SerializeField]
	public string resourcePath;
	[SerializeField]
	public string fullPath;
	[SerializeField]
	public string objType;
	[SerializeField]
	public string icon;
	
	Object obj;
	Object lastObj;
	
	bool _isDragging;
	
	public Object Load()
	{
		try
		{
			var o = Resources.Load(resourcePath, typeof(Object));
			return o;
		}
		catch
		{ 
			return null;
		}
	}
	
	public ResourceType (){}
	
	public ResourceType (string _path)
	{
		fullPath = _path;
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.HorizontalScope("Box"))
		{
			using (new GUILayout.VerticalScope())
			{
				if (!string.IsNullOrEmpty(icon))
				{
					#if UNITY_EDITOR
					GUILayout.Label(EditorGUIUtility.IconContent(icon), GUILayout.Width(40), GUILayout.Height(40));
					#endif
				}
				
				
			}
			
			#if UNITY_EDITOR
			if (obj != lastObj)
			{
				lastObj = obj;
				
				string _path = AssetDatabase.GetAssetPath(obj);
				
				
				fullPath = _path;
				
				_path = Path.Combine(Path.GetDirectoryName(_path), Path.GetFileNameWithoutExtension(_path));
			
				var _fullPathWithoutExtension = _path;
				
				var _ind = _path.IndexOf("Resources");
				resourcePath = _path.Substring(_ind+10, _fullPathWithoutExtension.Length-(_ind + 10));
				
				objType = obj.GetType().Name.ToString();
				
				switch(objType)
				{
					case "GameObject":
						icon = "d_Prefab Icon";
						break;
					case "Texture2D":
						icon = "Texture2D Icon";
						break;
					case "AudioClip":
						icon = "AudioClip Icon";
						break;
					case "MonoScript":
						icon = "cs Script Icon";
						break;
					case "TextAsset":
						icon = "TextAsset Icon";
						break;
					case "Material":
						icon = "Material Icon";
						break;
					case "AudioMixerController":
						icon = "AudioMixerGroup Icon";
						break;
					case "Shader":
						icon = "Shader Icon";
						break;
					case "AnimationClip":
						icon = "AnimationClip Icon";
						break;
					default:
						icon = "GameObject Icon";
						break;
				}
			}
			#endif
			
			using (new GUILayout.VerticalScope())
			{
				
				GUILayout.Label("Resource path: " + resourcePath);
				GUILayout.Label("Type: " + objType);
				
			}
			
			using (new GUILayout.VerticalScope())
			{
				using (new GUILayout.HorizontalScope())
				{
					#if UNITY_EDITOR
					DropAreaGUI();
					
					
					if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom"), GUILayout.Width(32), GUILayout.Height(32)))
					{
						Object _obj = AssetDatabase.LoadAssetAtPath(fullPath, typeof(Object));
						Selection.activeObject = _obj;
					}
					#endif
				}
			}
			
		}
	}
	
	#if UNITY_EDITOR
	public void DropAreaGUI()
	{
	
		Event _evt = Event.current;
		Rect _dropArea = GUILayoutUtility.GetRect(120f, 40f);
		
		if (_isDragging)
		{
			GUI.color = Color.green;
		}
		else
		{
			GUI.color = Color.white;
		}
		GUI.Box (_dropArea, "Drag new object here");
		GUI.color = Color.white;
		switch (_evt.type)
		{
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (!_dropArea.Contains(_evt.mousePosition))
				{
					_isDragging = false;
					return;
				}
				else
				{
					_isDragging = true;
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					
					if (_evt.type == EventType.DragPerform)
					{
						_isDragging = false;
						DragAndDrop.AcceptDrag();
						foreach (Object _dobj in DragAndDrop.objectReferences)
						{
							obj = _dobj;
						}
					}
					
				}
				break;
		}
	}
	#endif
	
	#if UNITY_EDITOR
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as ResourceType;
		if (resourcePath != _v.resourcePath)
		{
			// reload asset
			Reload();
			
			// return original value and changed value		
			return resourcePath.ToString() + " : " + _v.resourcePath.ToString();
		}
		else
		{
			return "";
		}
	}
	
	// Assign the CSV string to resource path
	public override void Convert(string _value)
	{
		resourcePath = _value;
		
		Reload();
	}
	

	void Reload()
	{
		if (!string.IsNullOrEmpty(resourcePath))
		{
			// load object and assign type icon
			var _o = Resources.Load(resourcePath, typeof(Object));
			
			if(_o == null)
				return;
				
			fullPath = AssetDatabase.GetAssetPath(_o);
			objType = _o.GetType().Name.ToString();
					
			switch(objType)
			{
				case "GameObject":
					icon = "d_Prefab Icon";
					break;
				case "Texture2D":
					icon = "Texture2D Icon";
					break;
				case "AudioClip":
					icon = "AudioClip Icon";
					break;
				case "MonoScript":
					icon = "cs Script Icon";
					break;
				case "TextAsset":
					icon = "TextAsset Icon";
					break;
				case "Material":
					icon = "Material Icon";
					break;
				case "AudioMixerController":
					icon = "AudioMixerGroup Icon";
					break;
				case "Shader":
					icon = "Shader Icon";
					break;
				case "AnimationClip":
					icon = "AnimationClip Icon";
					break;
				default:
					icon = "GameObject Icon";
					break;
			}
		}
	}
	#endif
	
}
