using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Bool")]
public class BoolType : DataboxType {
	
	[SerializeField]
	private bool _bool;
	[SerializeField]
	public bool InitValue;
	public bool Value
	{
		get {return _bool;}
		set
		{
			if (value == _bool){return;	}
			
			_bool = value;
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	
	public BoolType(){}
	
	public BoolType(bool _b)
	{
		_bool = _b;
	}
	
	public override void DrawEditor()
	{		
		Value = GUILayout.Toggle(Value, "");	
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label("Init Value:");
		GUI.color = Color.white;
		
		InitValue = GUILayout.Toggle(InitValue, "");
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as BoolType;
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
		var _v = _value.ToLower();
		if (_v.Contains( "true"))
		{
			Value = true;
		}
		else
		{
			Value = false;
		}
		
		InitValue = Value;
	}
}
