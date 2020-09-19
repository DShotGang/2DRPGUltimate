using UnityEngine;
using System.Globalization;
using System.Collections.Generic;
 



public static class GUIIntField
{

#if !UNITY_EDITOR
	private static int activeIntField = -1;
	private static int activeIntFieldLastValue = 0;
	private static string activeIntFieldString = "";
#endif

/// <summary>
	/// Int Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.IntField
/// </summary>
	public static int IntField (int value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.IntField (value);
#else
   
		// Get rect and control for this int field for identification
		Rect pos = GUILayoutUtility.GetRect (new GUIContent (value.ToString ()), GUI.skin.label, new GUILayoutOption[] { GUILayout.ExpandWidth (false), GUILayout.MinWidth (40) });
		int intFieldID = GUIUtility.GetControlID ("IntField".GetHashCode (), FocusType.Keyboard, pos) + 1;
		if (intFieldID == 0)
		return value;
   
		bool recorded = activeIntField == intFieldID;
		bool active = intFieldID == GUIUtility.keyboardControl;
   
		if (active && recorded && activeIntFieldLastValue != value)
		{ // Value has been modified externally
		activeIntFieldLastValue = value;
		activeIntFieldString = value.ToString ();
		}
   
		// Get stored string for the text field if this one is recorded
		string str = recorded? activeIntFieldString : value.ToString ();
   
		// pass it in the text field
		string strValue = GUI.TextField (pos, str);
		//string strValue = GUILayout.TextField(str);
   
		// Update stored value if this one is recorded
		if (recorded)
		activeIntFieldString = strValue;
   
		// Try Parse if value got changed. If the string could not be parsed, ignore it and keep last value
		bool parsed = true;
		if (strValue != value.ToString ())
		{
		int newValue;
		parsed = int.TryParse (strValue, out newValue);
		if (parsed)
		value = activeIntFieldLastValue = newValue;
		}
   
		if (active && !recorded)
		{ // Gained focus this frame
		activeIntField = intFieldID;
		activeIntFieldString = strValue;
		activeIntFieldLastValue = value;
		}
		else if (!active && recorded)
		{ // Lost focus this frame
		activeIntField = -1;
		if (!parsed)
		value = strValue.ForceParseInt ();
		}
   
		return value;
#endif
	}
 
	/// <summary>
	/// Int Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.IntField
	/// </summary>
	public static int IntField (GUIContent label, int value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.IntField (label, value);
#else
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label, label != GUIContent.none? GUILayout.ExpandWidth (true) : GUILayout.ExpandWidth (false));
		value = IntField (value);
		GUILayout.EndHorizontal ();
		return value;
#endif
	}
 
	/// <summary>
	/// Forces to parse to int by cleaning string if necessary
	/// </summary>
	public static int ForceParseInt (this string str)
	{
		// try parse
		int value;
		if (int.TryParse (str, out value))
			return value;
   
		// Clean string if it could not be parsed
		bool recordedDecimalPoint = false;
		List<char> strVal = new List<char> (str);
		for (int cnt = 0; cnt < strVal.Count; cnt++)
		{
			UnicodeCategory type = CharUnicodeInfo.GetUnicodeCategory (str[cnt]);
			if (type != UnicodeCategory.DecimalDigitNumber)
			{
				strVal.RemoveRange (cnt, strVal.Count-cnt);
				break;
			}
			else if (str[cnt] == '.')
			{
				if (recordedDecimalPoint)
				{
					strVal.RemoveRange (cnt, strVal.Count-cnt);
					break;
				}
				recordedDecimalPoint = true;
			}
		}
   
		// Parse again
		if (strVal.Count == 0)
			return 0;
		str = new string (strVal.ToArray ());
		if (!int.TryParse (str, out value))
			Debug.LogError ("Could not parse " + str);
		return value;
	}
}