using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Double")]
public class DoubleType : DataboxType {
	
	[SerializeField]
	private double _double;
	[SerializeField]
	public double InitValue;
	
	public double Value
	{
		get {return _double;}
		set
		{
			if (value == _double){return;}
			
			_double = value;
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public DoubleType (){}
	
	public DoubleType(double _d)
	{
		_double = _d;
	}
	
	// Draw GUI
	public override void DrawEditor()
	{
		_double = GUIDoubleField.DoubleField(_double);
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label ("Init Value:");
		GUI.color = Color.white;
			
		InitValue = GUIDoubleField.DoubleField(InitValue);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as DoubleType;
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
		System.Globalization.CultureInfo _ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
		_ci.NumberFormat.CurrencyDecimalSeparator = ".";
		
		
		Value = double.Parse(_value, System.Globalization.NumberStyles.Any, _ci);
		InitValue = Value;
	}
}
