using UnityEngine;
using System.Globalization;
using System.Collections.Generic;
 



public static class GUIFloatField
{

#if !UNITY_EDITOR
	private static int activeFloatField = -1;
	private static float activeFloatFieldLastValue = 0;
	private static string activeFloatFieldString = "";
#endif

/// <summary>
/// Float Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.FloatField
/// </summary>
	public static float FloatField (float value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.FloatField (value);
#else
   
		// Get rect and control for this float field for identification
		Rect pos = GUILayoutUtility.GetRect (new GUIContent (value.ToString ()), GUI.skin.label, new GUILayoutOption[] { GUILayout.ExpandWidth (false), GUILayout.MinWidth (40) });
		int floatFieldID = GUIUtility.GetControlID ("FloatField".GetHashCode (), FocusType.Keyboard, pos) + 1;
		if (floatFieldID == 0)
		return value;
   
		bool recorded = activeFloatField == floatFieldID;
		bool active = floatFieldID == GUIUtility.keyboardControl;
   
		if (active && recorded && activeFloatFieldLastValue != value)
		{ // Value has been modified externally
		activeFloatFieldLastValue = value;
		activeFloatFieldString = value.ToString ();
		}
   
		// Get stored string for the text field if this one is recorded
		string str = recorded? activeFloatFieldString : value.ToString ();
   
		// pass it in the text field
		string strValue = GUI.TextField (pos, str);
		//string strValue = GUILayout.TextField(str);
   
		// Update stored value if this one is recorded
		if (recorded)
		activeFloatFieldString = strValue;
   
		// Try Parse if value got changed. If the string could not be parsed, ignore it and keep last value
		bool parsed = true;
		if (strValue != value.ToString ())
		{
		float newValue;
		parsed = float.TryParse (strValue, out newValue);
		if (parsed)
		value = activeFloatFieldLastValue = newValue;
		}
   
		if (active && !recorded)
		{ // Gained focus this frame
		activeFloatField = floatFieldID;
		activeFloatFieldString = strValue;
		activeFloatFieldLastValue = value;
		}
		else if (!active && recorded)
		{ // Lost focus this frame
		activeFloatField = -1;
		if (!parsed)
		value = strValue.ForceParse ();
		}
   
		return value;
#endif
	}
 
	/// <summary>
	/// Float Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.FloatField
	/// </summary>
	public static float FloatField (GUIContent label, float value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.FloatField (label, value);
#else
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label, label != GUIContent.none? GUILayout.ExpandWidth (true) : GUILayout.ExpandWidth (false));
		value = FloatField (value);
		GUILayout.EndHorizontal ();
		return value;
#endif
	}
 
	/// <summary>
	/// Forces to parse to float by cleaning string if necessary
	/// </summary>
	public static float ForceParse (this string str)
	{
		// try parse
		float value;
		if (float.TryParse (str, out value))
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
		if (!float.TryParse (str, out value))
			Debug.LogError ("Could not parse " + str);
		return value;
	}
}