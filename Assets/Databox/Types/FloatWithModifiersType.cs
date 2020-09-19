using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Databox;

[System.Serializable]
[DataboxTypeAttribute(Name = "Float With Modifiers")]
public class FloatWithModifiersType : DataboxType {
	
	[SerializeField]
	private float _modifiedFloat;
	[SerializeField]
	private float _originalFloat;
	
	[SerializeField]
	public float InitValue;
	
	
	public float Value
	{
		get 
		{	
			// Iterate through all modifiers and check which one is enabled or not
			// modify Value based on modifier.
			float _tmpValue = _originalFloat;
			
		
			if (modifiers != null)
			{
				// Important! Sort modifiers based on operators. Multiplication/division before addition/substraction!
				List<KeyValuePair<string, FloatModifiers>> sortedModifiers = modifiers.ToList();

				sortedModifiers.Sort(
					delegate(KeyValuePair<string, FloatModifiers> pair1,
					KeyValuePair<string, FloatModifiers> pair2)
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
							case FloatModifiers.Type.multiply:
								_tmpValue *= sortedModifiers[i].Value.Value;
								break;
							case FloatModifiers.Type.divide:
								_tmpValue /= sortedModifiers[i].Value.Value;
								break;
							case FloatModifiers.Type.add:
								_tmpValue += sortedModifiers[i].Value.Value;
								break;
							case FloatModifiers.Type.subtract:
								_tmpValue -= sortedModifiers[i].Value.Value;
								break;
						}
					}
				}
			}

			_modifiedFloat = _tmpValue;
	
			return _modifiedFloat;
		}
		set
		{
			if (value == _modifiedFloat){return;}

			_originalFloat = value;
		
			if (OnValueChanged != null){OnValueChanged(this);}
		}
	}
	
	string tmpModifierName = "";
	
	public class FloatModifiers
	{
		public float Value;
		public bool enabled;
		public enum Type
		{
			multiply,
			divide,
			add,
			subtract
		}
		public Type type;
		
		public FloatModifiers(float _value, Type _type)
		{
			Value = _value;
			type = _type;
		}
	}
	
	// MODIFIERS
	[SerializeField]
	public Dictionary<string, FloatModifiers> modifiers; 
	
	/// <summary>
	/// Enable / Disable Modifier
	/// </summary>
	/// <param name="_id"></param>
	/// <param name="_enable"></param>
	public void Modifiers(string _id, bool _enable)
	{
		FloatModifiers _mod = null;
		
		if (modifiers.TryGetValue(_id, out _mod))
		{
			_mod.enabled = _enable;	
		}
	}
	
	
	public FloatWithModifiersType()
	{
		InitValue = 0;
		modifiers = new Dictionary<string, FloatModifiers>();
	}
	
	public FloatWithModifiersType(float _f)
	{
		_originalFloat = _f;
	}
	
	public override void DrawEditor()
	{
	
		_originalFloat = GUIFloatField.FloatField(_originalFloat);

		using (new GUILayout.VerticalScope("Box"))
		{
			#if UNITY_EDITOR
			EditorGUILayout.HelpBox("Modifiers can modify the base value based on their operators. Example: Attack base value = 5 -> Modifier: Shield adds +10 -> Result: 15. If a modifier is enabled it will return the modified base value.", MessageType.Info);
			#endif
			GUILayout.Label("New Modifier:");
			
			tmpModifierName = GUILayout.TextField(tmpModifierName);
			if (GUILayout.Button("add"))
			{
				modifiers.Add(tmpModifierName, new FloatModifiers(0, FloatModifiers.Type.add));
			}
		
			foreach (var key in modifiers.Keys)
			{
				using (new GUILayout.HorizontalScope("Box"))
				{	
					
				
					modifiers[key].enabled = GUILayout.Toggle(modifiers[key].enabled, "enabled");
					GUILayout.Label(key + " : " + _originalFloat.ToString());
				
					#if UNITY_EDITOR
					modifiers[key].type = (FloatWithModifiersType.FloatModifiers.Type)EditorGUILayout.EnumPopup(modifiers[key].type);
					#else
					GUILayout.Label(modifiers[key].type.ToString());
					#endif
					modifiers[key].Value = GUIFloatField.FloatField((float)modifiers[key].Value);
					

					
					
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
			
		InitValue = GUIFloatField.FloatField(InitValue);
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
		var _v = _changedValue as FloatWithModifiersType;
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
