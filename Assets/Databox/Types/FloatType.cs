using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Float")]
public class FloatType : DataboxType {
	
	[SerializeField]
	private float _float;
	[SerializeField]
	public float InitValue;
	
	public float Value
	{
		get {return _float;}
		set
		{
			if (value == _float){return;}
			
			_float = value;
			if (OnValueChanged != null){ OnValueChanged(this);}
		}
	}
	
	public FloatType()
	{
		InitValue = 0f;
	}
	
	public FloatType(float _f)
	{
		_float = _f;
	}
	
	
	// Draw GUI
	public override void DrawEditor()
	{
		_float = GUIFloatField.FloatField(_float);
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label ("Init Value:");
		GUI.color = Color.white;

		
		InitValue = GUIFloatField.FloatField(InitValue);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as FloatType;
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
		
		Value = float.Parse(_value, System.Globalization.NumberStyles.Any, _ci);
		InitValue = Value;
	}
}
