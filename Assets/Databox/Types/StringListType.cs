using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "List<string>")]
public class StringListType : DataboxType {
	
	[SerializeField]
	private List<string> _lsstring;
	[SerializeField]
	public List<string> InitValue;
	
	public List<string> Value
	{
		get {return _lsstring;}
		set
		{
			if (value == _lsstring){return;}
			
			_lsstring = value;
			if (OnValueChanged != null)OnValueChanged(this);
		}
	}
	
	public StringListType () 
	{
		_lsstring = new List<string>();
		InitValue = new List<string>();
		Value = _lsstring;
	}
	
	public StringListType(List<string> _ls)
	{
		_lsstring = new List<string>(_ls);
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.VerticalScope())
		{

			if (GUILayout.Button("add entry"))
			{
				_lsstring.Add("");
			}
			
			for (int i = 0; i < _lsstring.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					
					_lsstring[i] = GUILayout.TextField(_lsstring[i]);
					
				
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						_lsstring.RemoveAt(i);
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
				InitValue.Add("");
			}
				
				
			for (int i = 0; i < InitValue.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					
					InitValue[i] = GUILayout.TextField(InitValue[i]);
						
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
		Value = new List<string>(InitValue);
	}
	
	public override void Convert(string _value)
	{
		string[] _chars = _value.Split(char.Parse(","));
		
		Value = new List<string>();
		
		for (int i = 0; i < _chars.Length; i ++)
		{
			Value.Add(_chars[i]);
		}
		
		InitValue = new List<string>(Value);
	}
}
