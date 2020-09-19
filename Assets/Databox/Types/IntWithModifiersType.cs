using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Int With Modifiers")]
public class IntWithModifiersType : DataboxType {
	
	[SerializeField]
	private int _modifiedInt;
	[SerializeField]
	private int _originalInt;
	
	[SerializeField]
	public int InitValue;
	
	
	public int Value
	{
		get 
		{	
			// Iterate through all modifiers and check which one is enabled or not
			// modify Value based on modifier.
			int _tmpValue = _originalInt;
			
		
			if (modifiers != null)
			{
				// Important! Sort modifiers based on operators. Multiplication/division before addition/substraction!
				List<KeyValuePair<string, IntModifiers>> sortedModifiers = modifiers.ToList();

				sortedModifiers.Sort(
					delegate(KeyValuePair<string, IntModifiers> pair1,
					KeyValuePair<string, IntModifiers> pair2)
					{
						return pair1.Value.type.CompareTo(pair2.Value.type);
					}
				);
				
				for(int i = 0; i < sortedModifiers.Count; i++)
				{
					if (sortedModifiers[i].Value.enabled)
					{
						switch (sortedModifiers[i].Value.type)
						{
							case IntModifiers.Type.multiply:
								_tmpValue *= sortedModifiers[i].Value.Value;
								break;
							case IntModifiers.Type.divide:
								_tmpValue /= sortedModifiers[i].Value.Value;
								break;
							case IntModifiers.Type.add:
								_tmpValue += sortedModifiers[i].Value.Value;
								break;
							case IntModifiers.Type.subtract:
								_tmpValue -= sortedModifiers[i].Value.Value;
								break;
						}
					}
				}
			}

			_modifiedInt = _tmpValue;
	
			return _modifiedInt;
		}
		set
		{
			if (value == _modifiedInt){return;}

			_originalInt = value;
		
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	string tmpModifierName = "";
	
	public class IntModifiers
	{
		public int Value;
		public bool enabled;
		public enum Type
		{
			multiply,
			divide,
			add,
			subtract
		}
		public Type type;
		
		public IntModifiers(int _value, Type _type)
		{
			Value = _value;
			type = _type;
		}
	}
	
	// MODIFIERS
	[SerializeField]
	public Dictionary<string, IntModifiers> modifiers; 
	
	/// <summary>
	/// Enable / Disable Modifier
	/// </summary>
	/// <param name="_id"></param>
	/// <param name="_enable"></param>
	public void Modifiers(string _id, bool _enable)
	{
		IntModifiers _mod = null;
		
		if (modifiers.TryGetValue(_id, out _mod))
		{
			_mod.enabled = _enable;	
		}
	}
	
	
	public IntWithModifiersType()
	{
		InitValue = 0;
		modifiers = new Dictionary<string, IntModifiers>();
	}
	
	public IntWithModifiersType(int _i)
	{
		_originalInt = _i;
	}
	
	public override void DrawEditor()
	{
		#if UNITY_EDITOR
		EditorGUI.BeginChangeCheck();
		#endif
		
		var _intTxt = _originalInt.ToString();
		_intTxt = GUILayout.TextField(_intTxt);
		
		#if UNITY_EDITOR
		if (EditorGUI.EndChangeCheck())
		{
			int.TryParse(_intTxt, out _originalInt);
		}
		#endif
		
		using (new GUILayout.VerticalScope("Box"))
		{
			#if UNITY_EDITOR
			EditorGUILayout.HelpBox("Modifiers can modify the base value based on their operators. Example: Attack base value = 5 -> Modifier: Shield adds +10 -> Result: 15. If a modifier is enabled it will return the modified base value.", MessageType.Info);
			#endif
			GUILayout.Label("New Modifier:");
			
			tmpModifierName = GUILayout.TextField(tmpModifierName);
			if (GUILayout.Button("add"))
			{
				modifiers.Add(tmpModifierName, new IntModifiers(0, IntModifiers.Type.add));
			}
		
			foreach (var key in modifiers.Keys)
			{
				using (new GUILayout.HorizontalScope("Box"))
				{	
					
				
					modifiers[key].enabled = GUILayout.Toggle(modifiers[key].enabled, "enabled");
					GUILayout.Label(key + " : " + _originalInt);
				
					#if UNITY_EDITOR
					modifiers[key].type = (IntWithModifiersType.IntModifiers.Type)EditorGUILayout.EnumPopup(modifiers[key].type);
					#else
					GUILayout.Label(modifiers[key].type.ToString());
					#endif
					modifiers[key].Value = (int)GUIFloatField.FloatField((float)modifiers[key].Value);
					

					
					
					if (GUILayout.Button("x"))
					{
						modifiers.Remove(key);
					}
				}
			}
		}
	}
	
	public override void DrawInitValueEditor()
	{
		GUI.color = Color.yellow;
		GUILayout.Label ("Init Value:");
		GUI.color = Color.white;
			
		var _intInitTxt = InitValue.ToString();
		_intInitTxt = GUILayout.TextField(_intInitTxt);
		int.TryParse(_intInitTxt, out InitValue);
	}
	
	// Reset value back to initial value
	public override void Reset()
	{
		Value = InitValue;
		
		if (modifiers != null)
		{
			foreach(var key in modifiers.Keys)
			{
				modifiers[key].enabled = false;
			}
		}
	}
	
	// Important for the cloud sync comparison
	public override string Equal(DataboxType _changedValue)
	{
		var _v = _changedValue as IntWithModifiersType;
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
	
	// Convert the CSV string to an integer value
	public override void Convert(string _value)
	{
		Value = int.Parse(_value);
		InitValue = Value;
	}
}
