using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using Databox.Utils;

[System.Serializable]
[DataboxTypeAttribute(Name = "Vector4")]
public class Vector4Type : DataboxType {
	
	[SerializeField]
	private SerializableVector4 _vector4;
	[SerializeField]
	public SerializableVector4 InitValue;
	[SerializeField]
	public SerializableVector4 Value
	{
		get { return _vector4; }
		set
		{
			var _compare = new Vector4(value.x, value.y, value.z);
			var _with = new Vector4(_vector4.x, _vector4.y, _vector4.z, _vector4.w);
			if (_compare == _with){ return;}
			
			_vector4 = new SerializableVector4(value.x, value.y, value.z, value.w);
			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public Vector4Type () { }
	
	public Vector4Type (Vector4 _v)
	{
		_vector4 = new SerializableVector4(_v.x, _v.y, _v.z, _v.w);
	}
	
	public override void DrawEditor()
	{
		var _v4 = Value;
		
		var _x = _v4.x;
		var _y = _v4.y;
		var _z = _v4.z;
		var _w = _v4.w;

		using (new GUILayout.HorizontalScope())
		{
			//GUILayout.Space(130);
			GUILayout.Label("x:");
			_x = GUIFloatField.FloatField(_x);
			GUILayout.Label("y:");
			_y = GUIFloatField.FloatField(_y);
			GUILayout.Label("z:");
			_z = GUIFloatField.FloatField(_z);
			GUILayout.Label("w:");
			_w = GUIFloatField.FloatField(_w);
		}

		Value = new SerializableVector4(_x, _y, _z, _w);
	}
	
	public override void DrawInitValueEditor()
	{
		var _xInit = InitValue.x;
		var _yInit = InitValue.y;
		var _zInit = InitValue.z;
		var _wInit = InitValue.w;
		
		
		using (new GUILayout.HorizontalScope())
		{

			GUI.color = Color.yellow;
			GUILayout.Label("Init Value:", GUILayout.Width(120));
			GUI.color = Color.white;
				
			GUILayout.Label("x:");
			_xInit = GUIFloatField.FloatField(_xInit);
			GUILayout.Label("y:");
			_yInit = GUIFloatField.FloatField(_yInit);
			GUILayout.Label("z:");
			_zInit = GUIFloatField.FloatField(_zInit);
			GUILayout.Label("w:");
			_wInit = GUIFloatField.FloatField(_wInit);
		}
		
		InitValue = new SerializableVector4(_xInit, _yInit, _zInit, _wInit);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as Vector4Type;
		if (Value.x != _v.Value.x || Value.y != _v.Value.y || Value.z != _v.Value.z || Value.w != _v.Value.w)
		{
			// return original value and changed value
			return Value.x.ToString() + " , " + Value.y.ToString() + " , " + Value.z.ToString() + " , " + Value.w.ToString()  + " : " + _v.Value.x.ToString() + " , " + _v.Value.y.ToString() + " , " + _v.Value.z.ToString() + " , " + _v.Value.w.ToString();
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
		
		Value = new SerializableVector4(float.Parse(_chars[0], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[1], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[2], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[3], System.Globalization.NumberStyles.Any, _ci));
			
		InitValue = Value;
	}
}
