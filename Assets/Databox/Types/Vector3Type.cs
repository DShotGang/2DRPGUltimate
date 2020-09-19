using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using Databox.Utils;

[System.Serializable]
[DataboxTypeAttribute(Name = "Vector3")]
public class Vector3Type : DataboxType {
	
	[SerializeField]
	private SerializableVector3 _vector3;
	[SerializeField]
	public SerializableVector3 InitValue;
	[SerializeField]
	public SerializableVector3 Value
	{
		get { return _vector3; }
		set
		{
			var _compare = new Vector3(value.x, value.y, value.z);
			var _with = new Vector3(_vector3.x, _vector3.y, _vector3.z);
			if (_compare == _with){ return;}
			
			_vector3 = new SerializableVector3(value.x, value.y, value.z);
			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public Vector3Type () { }
	
	public Vector3Type (Vector3 _v)
	{
		_vector3 = _v;
	}
	
	public override void DrawEditor()
	{
		var _v3 = Value;
		
		var _x = _v3.x;
		var _y = _v3.y;
		var _z = _v3.z;

		using (new GUILayout.HorizontalScope())
		{
			//GUILayout.Space(130);
			GUILayout.Label("x:");
			_x = GUIFloatField.FloatField(_x);
			
			GUILayout.Label("y:");
			_y = GUIFloatField.FloatField(_y);
			GUILayout.Label("z:");
			_z = GUIFloatField.FloatField(_z);
		}

		Value = new SerializableVector3(_x, _y, _z);
	}
	
	public override void DrawInitValueEditor()
	{
		var _xInit = InitValue.x;
		var _yInit = InitValue.y;
		var _zInit = InitValue.z;
		
		
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
		}
		
		InitValue = new SerializableVector3(_xInit, _yInit, _zInit);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as Vector3Type;
		if (Value.x != _v.Value.x || Value.y != _v.Value.y || Value.z != _v.Value.z)
		{
			// return original value and changed value
			return Value.x.ToString() + " , " + Value.y.ToString() + " , " + Value.z.ToString() + " : " + _v.Value.x.ToString() + " , " + _v.Value.y.ToString() + " , " + _v.Value.z.ToString();
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
		
		Value = new SerializableVector3(float.Parse(_chars[0], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[1], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[2], System.Globalization.NumberStyles.Any, _ci));
			
		InitValue = Value;
	}
}
