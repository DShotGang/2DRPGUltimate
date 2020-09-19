using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using Databox.Utils;

[System.Serializable]
[DataboxTypeAttribute(Name = "Vector2")]
public class Vector2Type : DataboxType {
	
	[SerializeField]
	private SerializableVector2 _vector2;
	[SerializeField]
	public SerializableVector2 InitValue;
	[SerializeField]
	public SerializableVector2 Value
	{
		get { return _vector2; }
		set
		{
			var _compare = new Vector2(value.x, value.y);
			var _with = new Vector2(_vector2.x, _vector2.y);
			if (_compare == _with){ return;}
			
			_vector2 = new SerializableVector2(value.x, value.y);
			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public Vector2Type () { }
	
	public Vector2Type(Vector2 _v)
	{
		_vector2 = _v;
	}
	
	public override void DrawEditor()
	{
		var _v2 = Value;
		
		var _x = _v2.x;
		var _y = _v2.y;

		using (new GUILayout.HorizontalScope())
		{
			//GUILayout.Space(130);
			GUILayout.Label("x:");
			_x = GUIFloatField.FloatField(_x);
			
			GUILayout.Label("y:");
			_y = GUIFloatField.FloatField(_y);

		}

		Value = new SerializableVector2(_x, _y);
	}
	
	public override void DrawInitValueEditor()
	{
		var _xInit = InitValue.x;
		var _yInit = InitValue.y;
		
		
		using (new GUILayout.HorizontalScope())
		{

			GUI.color = Color.yellow;
			GUILayout.Label("Init Value:", GUILayout.Width(120));
			GUI.color = Color.white;
				
			GUILayout.Label("x:");
			_xInit = GUIFloatField.FloatField(_xInit);
			GUILayout.Label("y:");
			_yInit = GUIFloatField.FloatField(_yInit);
		}
		
		InitValue = new SerializableVector2(_xInit, _yInit);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as Vector2Type;
		if (Value.x != _v.Value.x || Value.y != _v.Value.y)
		{
			// return original value and changed value
			return Value.x.ToString() + " , " + Value.y.ToString()  + " : " + _v.Value.x.ToString() + " , " + _v.Value.y.ToString();
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
		
		string[] _chars = _value.Split(char.Parse(","));
		
		Value = new SerializableVector2(float.Parse(_chars[0], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[1], System.Globalization.NumberStyles.Any, _ci));
			
		InitValue = Value;
	}
}
