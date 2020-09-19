using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Int")]
public class IntType : DataboxType {
	
	[SerializeField]
	private int _int;
	[SerializeField]
	public int InitValue;
	
	public int Value
	{
		get {return _int;}
		set
		{
			if (value == _int){return;}
			
			_int = value;
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public IntType()
	{
		InitValue = 0;
	}
	
	public IntType(int _i)
	{
		_int = _i;
	}
	
	public override void DrawEditor()
	{
		_int = GUIIntField.IntField(_int);
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label ("Init Value:");
		GUI.color = Color.white;
			
			
		InitValue = GUIIntField.IntField(InitValue);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as IntType;
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
	
	// Convert the CSV string to an integer value
	public override void Convert(string _value)
	{
		Value = int.Parse(_value);
		InitValue = Value;
	}
}
