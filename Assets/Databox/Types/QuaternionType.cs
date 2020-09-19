using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
using Databox.Utils;

[System.Serializable]
[DataboxTypeAttribute(Name = "Quaternion")]
public class QuaternionType : DataboxType {

	[SerializeField]
	private SerializableQuaternion _quaternion;
	[SerializeField]
	public SerializableQuaternion InitValue;
	public SerializableQuaternion Value
	{
		get {return _quaternion;}
		set
		{
			_quaternion = value;
			
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	public QuaternionType () { }
	
	public QuaternionType (Quaternion _q)
	{
		_quaternion = _q;
	}
	
	public QuaternionType (Vector4 _q)
	{
		_quaternion = new SerializableQuaternion(_q.x, _q.y, _q.z, _q.w);
	}
	
	public override void DrawEditor()
	{
		//var _q = Value;
		
		var _x = Value.x;
		var _y = Value.y;
		var _z = Value.z;
		var _w = Value.w;
		
	
		using (new GUILayout.HorizontalScope())
		{
			GUILayout.Label("x:");
			_x = GUIFloatField.FloatField(_x);
			GUILayout.Label("y:");
			_y = GUIFloatField.FloatField(_y);
			GUILayout.Label("z:");
			_z = GUIFloatField.FloatField(_z);
			GUILayout.Label("w:");
			_w = GUIFloatField.FloatField(_w);
		}
				
		// convert string to quaternion
		Value = new SerializableQuaternion(_x, _y, _z, _w);
		
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
		
		InitValue = new SerializableQuaternion(_xInit, _yInit, _zInit, _wInit);
	}
	
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
	}
	
	public override void Convert(string _value)
	{
		System.Globalization.CultureInfo _ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
		_ci.NumberFormat.CurrencyDecimalSeparator = ".";
		
		
		string[] _chars = _value.Split(char.Parse(","));
		Quaternion _q = new Quaternion(float.Parse(_chars[0], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[1], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[2], System.Globalization.NumberStyles.Any, _ci),
			float.Parse(_chars[3], System.Globalization.NumberStyles.Any, _ci));
			
		Value = _q;
		InitValue = Value;
	}
}