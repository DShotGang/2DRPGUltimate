
////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
////!! USE THE UNITY PACKAGE MANAGER TO DOWNLOAD THE ADDRESSABLES PACKAGE BEFORE UNCOMMENTING THIS 
////!!
////!! Pleas note: Addressables are still in Beta. 
////!! Therefore it's possible that this script might spawn error messages.
////!!
////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AddressableAssets;
//#if UNITY_EDITOR
//using UnityEditor.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;
//using UnityEditor;
//#endif
//using UnityEngine.ResourceManagement;
//using UnityEngine.ResourceManagement.AsyncOperations;

//using System.IO;
//using Databox;
//using Databox.Utils;


//[System.Serializable]
//public class AddressableType : DataboxType
//{
//	public string assetPath;

//	Rect dropdownRect;
//	float popupWidth;
	
//	public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> Load()
//	{
//		try
//		{
//			var o = Addressables.LoadAssetAsync<GameObject>(assetPath);
//			return o;
//		}
//		catch
//		{ 
//			return new UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>();
//		}
//	}
	

//	public override void DrawEditor()
//	{
		
//		#if UNITY_EDITOR
//		using (new GUILayout.HorizontalScope("Box"))
//		{
		
//			var allAssets = new List<AddressableAssetEntry>();

//			try
//			{
				
			
				
//				GUILayout.Label("",GUILayout.Width(1));
//				dropdownRect = GUILayoutUtility.GetLastRect();
			
//				if (EditorGUILayout.DropdownButton(new GUIContent(assetPath), FocusType.Keyboard, GUILayout.ExpandWidth(true)))
//				{	
//					AddressableAssetSettingsDefaultObject.Settings.GetAllAssets(allAssets, false);
//					PopupWindow.Show(dropdownRect, new PopupContent(allAssets, this));
//				}
//				var x = GUILayoutUtility.GetLastRect();
//				if (x.width > 1)
//				{
//					popupWidth = x.width;
//				}			
		
//			}
//			catch
//			{
//				EditorGUILayout.HelpBox("No addressables found. Please make sure to have at least one prefab as addressable.", MessageType.Info);
//			}
//		}
//		#endif
//	}
	

//	#if UNITY_EDITOR
//	public class PopupContent : PopupWindowContent
//	{
//		AddressableType value;
//		List<AddressableAssetEntry> assets = new List<AddressableAssetEntry>();
//		string searchString = "";

//		public override Vector2 GetWindowSize()
//		{
//			return new Vector2(value.popupWidth, ((assets.Count + 1) * 22) + 60);
//		}

//		public override void OnGUI(Rect rect)
//		{	
//			GUILayout.Label("Addressables", EditorStyles.boldLabel);
			
//			using (new GUILayout.HorizontalScope())
//			{
//				GUI.SetNextControlName ("FilterAddressables");
//				searchString = GUILayout.TextField(searchString, "SearchTextField");
						
//				if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton")))
//				{
//					searchString = "";
//				}
//			}
			
//			if (GUILayout.Button("none"))
//			{
//				value.assetPath = "";
//				editorWindow.Close();
//			}
			
//			foreach(var entry in assets)
//			{
//				if (entry.address.ToLower().Contains(searchString.ToLower()) || string.IsNullOrEmpty(searchString))
//				{
//					using (new GUILayout.HorizontalScope())
//					{
//						var _icon = AssetDatabase.GetCachedIcon(entry.AssetPath) as Texture2D;
//						GUILayout.Label(_icon, GUILayout.Height(20));
//						if (GUILayout.Button(entry.address.ToString()))
//						{
//							//assetPa
//							value.assetPath = entry.address;
//							editorWindow.Close();
//						}
//					}
//				}
//			}
			
//		}

//		public override void OnOpen()
//		{
//			//Debug.Log("Popup opened: " + this);
//			EditorGUI.FocusTextInControl ("FilterAddressables");
//		}

//		public override void OnClose()
//		{
//			//Debug.Log("Popup closed: " + this);
//		}
		
//		public PopupContent(List<AddressableAssetEntry> _assets, AddressableType _value)
//		{
//			assets = new List<AddressableAssetEntry>(_assets);
//			//assets = _assets;
//			value = _value;
//		}
//	}
//	#endif

//}
