using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Color")]
public class ColorType : DataboxType {
	
	[SerializeField]
	private Color _color;
	[SerializeField]
	public Color InitValue;
	public Color Value 
	{
		get {return _color;}
		set
		{
			if (value == _color){return;}
			
			_color = value;			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public ColorType(){}

	public ColorType(Color _c)
	{
		_color = _c;	
	}
	
	public override void DrawEditor()
	{
		#if UNITY_EDITOR
		Value = EditorGUILayout.ColorField(Value);
		#else
		GUILayout.Label("EditorGUILayout.ColorField is not supported at runtime");
		GUI.color = Value;
		GUILayout.Box("");
		GUI.color = Color.white;
		#endif
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label("Init Value:");
		GUI.color = Color.white;
			
		#if UNITY_EDITOR
		InitValue = EditorGUILayout.ColorField(InitValue);
		#else
		GUILayout.Label("EditorGUILayout.ColorField is not supported at runtime");
		GUI.color = Value;
		GUILayout.Box("");
		GUI.color = Color.white;
		#endif 
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as ColorType;
		if (Value != _v.Value)
		{
			// return original value and changed value
			return Value.ToString() + " : " + _v.Value.ToString();
		}
		else
		{
			return "";
		}
	}
	
	public override void Convert(string _value)
	{		
		string[] _chars = _value.Split(char.Parse(","));
		Color _newColor = new Color(float.Parse(_chars[0])/255f,float.Parse(_chars[1])/255f,float.Parse(_chars[2])/255f,float.Parse(_chars[3])/255f);
		Value = _newColor;
		InitValue = Value;
	}
}
