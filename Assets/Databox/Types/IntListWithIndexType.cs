using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "List<int> + Index")]
public class IntListWithIndexType : DataboxType {
	
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
	
	public IntListWithIndexType () 
	{
		_lsint = new List<int>();
		InitValue = new List<int>();
		Value = _lsint;
	}
	
	public IntListWithIndexType(List<int> _ls)
	{
		_lsint = new List<int>(_ls);
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
				_lsint.Add(0);
			}
			
			for (int i = 0; i < _lsint.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					var _intTxt = _lsint[i].ToString();
					
					if (i == _selectedIndex)
					{
						GUI.color = new Color(180f/255f, 200f/255f, 170f/255f);
					}
					else
					{
						GUI.color = Color.white;
					}
					_intTxt = GUILayout.TextField(_intTxt);
					GUI.color = Color.white;
			
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
			
			
			var _sel = selectedIndexInit.ToString();
			GUILayout.Label("Selected index:");
			_sel = GUILayout.TextField(_sel);
			
			selectedIndexInit = int.Parse(_sel);
			
			
			if (GUILayout.Button("add init entry"))
			{
				InitValue.Add(0);
			}
				
				
			for (int i = 0; i < InitValue.Count; i ++)
			{
				using (new GUILayout.HorizontalScope())
				{
					var _intTxt = InitValue[i].ToString();
					
					if (i == selectedIndexInit)
					{
						GUI.color = Color.green;
					}
					else
					{
						GUI.color = Color.white;
					}
					_intTxt = GUILayout.TextField(_intTxt);
					GUI.color = Color.white;
					
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
		selectedIndex = selectedIndexInit;
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
