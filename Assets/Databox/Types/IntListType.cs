using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "List<int>")]
public class IntListType : DataboxType {
	
	[SerializeField]
	private List<int> _lsint;
	[SerializeField]
	public List<int> InitValue;
	
	public List<int> Value
	{
		get {return _lsint;}
		set
		{
			if (value == _lsint){return;}
			
			_lsint = value;
			if (OnValueChanged != null)OnValueChanged(this);
		}
	}
	
	public IntListType () 
	{
		_lsint = new List<int>();
		InitValue = new List<int>();
		Value = _lsint;
	}
	
	public IntListType(List<int> _ls)
	{
		_lsint = new List<int>(_ls);
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.VerticalScope())
		{

			if (GUILayout.Button("add entry"))
			{
				_lsint.Add(0);
			}
			
			for (int i = 0; i < _lsint.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					var _intTxt = _lsint[i].ToString();
					_intTxt = GUILayout.TextField(_intTxt);
					var _i = 0;
					int.TryParse(_intTxt, out _i);
					_lsint[i] = _i;
					
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						_lsint.RemoveAt(i);
					}
				}
			}
		}
	}
	
	public override void DrawInitValueEditor()
	{
		using (new GUILayout.VerticalScope())
		{
			GUI.color = Color.yellow;
			GUILayout.Label ("Init Value:");
			GUI.color = Color.white;
			if (GUILayout.Button("add init entry"))
			{
				InitValue.Add(0);
			}
				
				
			for (int i = 0; i < InitValue.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					var _intTxt = InitValue[i].ToString();
					_intTxt = GUILayout.TextField(_intTxt);
					var _i = 0;
					int.TryParse(_intTxt, out _i);
					InitValue[i] = _i;
						
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						InitValue.RemoveAt(i);
					}
				}
			}
		}
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = new List<int>(InitValue);
	}
	
	public override void Convert(string _value)
	{
		string[] _chars = _value.Split(char.Parse(","));
		
		Value = new List<int>();
		
		for (int i = 0; i < _chars.Length; i ++)
		{
			Value.Add(int.Parse(_chars[i]));
		}
		
		InitValue = new List<int>(Value);
	}
}
