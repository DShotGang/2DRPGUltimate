using UnityEngine;
using System.Globalization;
using System.Collections.Generic;
 



public static class GUIDoubleField
{
	
	#if !UNITY_EDITOR
	private static int activeDoubleField = -1;
	private static double activeDoubleFieldLastValue = 0;
	private static string activeDoubleFieldString = "";
	#endif

/// <summary>
	/// Double Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.DoubleField
/// </summary>
	public static double DoubleField (double value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.DoubleField (value);
#else
   
		// Get rect and control for this double field for identification
		Rect pos = GUILayoutUtility.GetRect (new GUIContent (value.ToString ()), GUI.skin.label, new GUILayoutOption[] { GUILayout.ExpandWidth (false), GUILayout.MinWidth (40) });
		int doubleFieldID = GUIUtility.GetControlID ("DoubleField".GetHashCode (), FocusType.Keyboard, pos) + 1;
		if (doubleFieldID == 0)
		return value;
   
		bool recorded = activeDoubleField == doubleFieldID;
		bool active = doubleFieldID == GUIUtility.keyboardControl;
   
		if (active && recorded && activeDoubleFieldLastValue != value)
		{ // Value has been modified externally
		activeDoubleFieldLastValue = value;
		activeDoubleFieldString = value.ToString ();
		}
   
		// Get stored string for the text field if this one is recorded
		string str = recorded? activeDoubleFieldString : value.ToString ();
   
		// pass it in the text field
		string strValue = GUI.TextField (pos, str);
   
		// Update stored value if this one is recorded
		if (recorded)
		activeDoubleFieldString = strValue;
   
		// Try Parse if value got changed. If the string could not be parsed, ignore it and keep last value
		bool parsed = true;
		if (strValue != value.ToString ())
		{
		double newValue;
		parsed = double.TryParse (strValue, out newValue);
		if (parsed)
		value = activeDoubleFieldLastValue = newValue;
		}
   
		if (active && !recorded)
		{ // Gained focus this frame
		activeDoubleField = doubleFieldID;
		activeDoubleFieldString = strValue;
		activeDoubleFieldLastValue = value;
		}
		else if (!active && recorded)
		{ // Lost focus this frame
		activeDoubleField = -1;
		if (!parsed)
		value = strValue.ForceDoubleParse ();
		}
   
		return value;
#endif
	}
 
	/// <summary>
	/// Double Field for ingame purposes. Behaves exactly like UnityEditor.EditorGUILayout.DoubleField
	/// </summary>
	public static double DoubleField (GUIContent label, double value)
	{
#if UNITY_EDITOR
		return UnityEditor.EditorGUILayout.DoubleField (label, value);
#else
		GUILayout.BeginHorizontal ();
		GUILayout.Label (label, label != GUIContent.none? GUILayout.ExpandWidth (true) : GUILayout.ExpandWidth (false));
		value = DoubleField (value);
		GUILayout.EndHorizontal ();
		return value;
#endif
	}
 
	/// <summary>
	/// Forces to parse to double by cleaning string if necessary
	/// </summary>
	public static double ForceDoubleParse (this string str)
	{
		// try parse
		double value;
		if (double.TryParse (str, out value))
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
		if (!double.TryParse (str, out value))
			Debug.LogError ("Could not parse " + str);
		return value;
	}
}