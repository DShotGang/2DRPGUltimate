using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "List<string> + Index")]
public class StringListWithIndexType : DataboxType {
	
	[SerializeField]
	private int _selectedIndex;
	[SerializeField]
	public int selectedIndexInit;
	public int selectedIndex
	{
		get {return _selectedIndex;}
		set
		{
			if (value == _selectedIndex){return;}
			
			_selectedIndex = value;
			if (OnValueChanged != null)OnValueChanged(this);
		}
	}
	
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
	
	public StringListWithIndexType () 
	{
		_lsstring = new List<string>();
		InitValue = new List<string>();
		Value = _lsstring;
	}
	
	public StringListWithIndexType(List<string> _ls)
	{
		_lsstring = new List<string>(_ls);
	}
	
	public override void DrawEditor()
	{
		using (new GUILayout.VerticalScope())
		{

			var _sel = selectedIndex.ToString();
			GUILayout.Label("Selected index:");
			_sel = GUILayout.TextField(_sel);
			
			selectedIndex = int.Parse(_sel);
			

			if (GUILayout.Button("add entry"))
			{
				_lsstring.Add("");
			}
			
			for (int i = 0; i < _lsstring.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					if (i == _selectedIndex)
					{
						GUI.color = Color.green;
					}
					else
					{
						GUI.color = Color.white;
					}
					
					_lsstring[i] = GUILayout.TextField(_lsstring[i]);
					GUI.color = Color.white;
					
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
			
			
			var _sel = selectedIndexInit.ToString();
			GUILayout.Label("Selected index:");
			_sel = GUILayout.TextField(_sel);
			
			selectedIndexInit = int.Parse(_sel);
			
			
			if (GUILayout.Button("add init entry"))
			{
				InitValue.Add("");
			}
				
				
			for (int i = 0; i < InitValue.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					if (i == selectedIndexInit)
					{
						GUI.color = Color.green;
					}
					else
					{
						GUI.color = Color.white;
					}
					
					InitValue[i] = GUILayout.TextField(InitValue[i]);
					GUI.color = Color.white;
						
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
		selectedIndex = selectedIndexInit;
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
