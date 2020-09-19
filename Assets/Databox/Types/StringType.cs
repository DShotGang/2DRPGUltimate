using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "String")]
public class StringType : DataboxType {
	
	[SerializeField]
	private string _string;
	[SerializeField]
	public string InitValue;
	public string Value
	{
		get {return _string;}
		set
		{
			if (value == _string){return;}
			
			_string = value;			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public StringType(){}
	
	public StringType(string _s)
	{
		_string = _s;
	}
	
	public override void DrawEditor()
	{
		Value = GUILayout.TextField(Value);
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label("Init Value:");
		GUI.color = Color.white;
		
		InitValue = GUILayout.TextField(InitValue);
	} 
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as StringType;
		if (!Value.Equals(_v.Value))
		{
			// return original value and changed value
			return Value + " : " + _v.Value;
		}
		else
		{
			return "";
		}
	}
	
	public override void Convert(string _value)
	{
		Value = _value;
		InitValue = Value;
	}
}
