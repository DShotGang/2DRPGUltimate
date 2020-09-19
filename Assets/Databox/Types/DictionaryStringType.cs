using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Dictionary<string,string>")]
public class DictionaryStringType : DataboxType {
	
	[SerializeField]
	private Dictionary<string, string> _dicstring;
	[SerializeField]
	public Dictionary<string, string> InitValue;
	[SerializeField]
	public Dictionary<string, string> Value
	{
		get {return _dicstring;}
		set
		{
			if (value == _dicstring){return;}
			
			_dicstring = value;
			if (OnValueChanged != null)OnValueChanged(this);
		}
	}
	
	string keyValue;
	string stringValue;
	
	string initKeyValue;
	string initStringValue;
	
	
	public DictionaryStringType () 
	{
		_dicstring = new Dictionary<string, string>();
		InitValue = new Dictionary<string, string>();
		Value = _dicstring;
	}
	
	public DictionaryStringType(Dictionary<string, string> _dict)
	{
		_dicstring = _dict;
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.VerticalScope())
		{

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Key:");
				keyValue = GUILayout.TextField(keyValue);
				GUILayout.Label("Value:");
				stringValue = GUILayout.TextField(stringValue);
			}
			
			if (GUILayout.Button("add entry"))
			{
				_dicstring.Add(keyValue, stringValue);
			}
			
			foreach (var key in _dicstring.Keys)
			{
				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Label("Key:");
					GUILayout.Label(key);
					GUILayout.Label("Value:");
					GUILayout.Label(_dicstring[key]);
					
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						_dicstring.Remove(key);
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
			
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Key:");
				initKeyValue = GUILayout.TextField(initKeyValue);
				GUILayout.Label("Value:");
				initStringValue = GUILayout.TextField(initStringValue);
			}
			
			if (GUILayout.Button("add entry"))
			{
				InitValue.Add(initKeyValue, initStringValue);
			}
			
			foreach (var key in InitValue.Keys)
			{
				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Label("Key:");
					GUILayout.Label(key);
					GUILayout.Label("Value:");
					GUILayout.Label(InitValue[key]);
					
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						InitValue.Remove(key);
					}
				}
			}
			
		}
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = new Dictionary<string, string>(InitValue);
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as DictionaryStringType;
		string _changed = "";
		
		if (Value.Keys.Count != _v.Value.Keys.Count)
		{
			return "Dictionary changed";
		}
		else
		{
			foreach(var key in _v.Value.Keys)
			{
				if (_v.Value[key] != Value[key])
				{
					_changed += Value[key] + " : " + _v.Value[key] + "\n";
				}
			}
			return "";
		}
	}
}
