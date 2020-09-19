using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Databox
{
	/// <summary>
	/// Database UI Binding component.
	/// Usage: add component to an UI component to allow connection between the ui and the database
	/// Supports: Text, InputField, Slider, Button, Toggle, Dropdown and RectTransform
	/// </summary>
	public class DataboxUIBinding : MonoBehaviour {
	
		public DataboxObject databox;
		
		public enum UIType
		{
			Text,
			InputField,
			Slider,
			Button,
			Toggle,
			Dropdown,
			RectTransform
		}
		
		public UIType uiType;
		
	
		public bool bindOnDatabaseLoad;
		
		public string tableID;
		public string entryID;
		public string valueID;	
		
		object data;
		
		Text text;
		InputField inputField;
		Slider slider;
		Button button;
		Toggle toggle;
		Dropdown dropdown;
		RectTransform rTransform;
		
		float rTransformWidth;
		
		void OnDisable()
		{
			if (bindOnDatabaseLoad)
			{
				databox.OnDatabaseLoaded -= BindInternal;
			
				if (data != null)
				{
					var _changeEvent = (DataboxType)data;
					_changeEvent.OnValueChanged -= OnValueChanged;
				}
			}
		}
		
		void Start()
		{
			
			if (databox == null)
			{
				Debug.LogError("Databox UI Binding error. No Databox object set");
				return;
			}
			
			if (bindOnDatabaseLoad)
			{
				databox.OnDatabaseLoaded += BindInternal;
			}
			
			if (bindOnDatabaseLoad && databox.databaseLoaded)
			{
				BindInternal();
			}
		}
		
		
		public void Bind(string _tableID, string _entryID, string _valueID)
		{
			if (databox != null)
			{
				tableID = _tableID;
				entryID = _entryID;
				valueID = _valueID;
				
				BindInternal();
			}
			else
			{
				Debug.LogWarning("Binding failed: " + gameObject.name);
			}
		}
		
		
		public void Bind(DataboxObject _databoxObject, string _tableID, string _entryID, string _valueID)
		{
			if (_databoxObject != null)
			{
				databox = _databoxObject;
				
				tableID = _tableID;
				entryID = _entryID;
				valueID = _valueID;
				
				BindInternal();
			}
			else
			{
				Debug.LogWarning("Binding failed: " + gameObject.name);
			}
		}

		public void Bind (DataboxObjectManager _manager, string _dbID, string _tableID, string _entryID, string _valueID)
		{
			databox = _manager.GetDataboxObject(_dbID);
			
			if (databox != null)
			{
				tableID = _tableID;
				entryID = _entryID;
				valueID = _valueID;
				
				BindInternal();
			}
			else
			{
				Debug.LogWarning("Binding failed: " + gameObject.name);
			}
		}
		
		void BindInternal()
		{
			data = databox.GetDataUnknown(tableID, entryID, valueID);
			
			var _changeEvent = (DataboxType)data;
			_changeEvent.OnValueChanged += OnValueChanged;
			
			switch (uiType)
			{
				case UIType.Slider:	
					slider = this.GetComponent<Slider>();
					slider.onValueChanged.AddListener(delegate { SetValueFloat(slider.value); });
					break;
				case UIType.InputField:
					inputField = this.GetComponent<InputField>();
					inputField.onValueChanged.AddListener(delegate { SetValueString(inputField.text); });
					break;
				case UIType.Toggle:
					toggle = this.GetComponent<Toggle>();
					toggle.onValueChanged.AddListener(delegate { SetValueBool(toggle.isOn); });
					break;	
				case UIType.Text:
					text = this.GetComponent<Text>();
					break;
				case UIType.Dropdown:
					dropdown = this.GetComponent<Dropdown>();
					dropdown.onValueChanged.AddListener(delegate {SetValueInt(dropdown.value); });
					break;
				case UIType.RectTransform:
					rTransform = this.GetComponent<RectTransform>();
					rTransformWidth = rTransform.sizeDelta.x;
					break;
			}
			
			// update on bind
			OnValueChanged((DataboxType)data);
		}
		
		void OnValueChanged(DataboxType _data)
		{
			// convert value
			var _type = _data.GetType().ToString();
			object _v = null;
			object _init = null;
			
			switch(_type)
			{
				case "FloatType":
					var _f = (FloatType)_data;
					_v = _f.Value;
					_init = _f.InitValue;
					break;
				case "IntType":
					var _i = (IntType)_data;
					_v = _i.Value;
					_init = _i.InitValue;
					break;
				case "StringType":
					var _s = (StringType)_data;
					_v = _s.Value;
					break;
				case "BoolType":
					var _b = (BoolType)_data;
					_v = _b.Value;
					break;
			}
			
			if (_v != null)
			{
				switch (uiType)
				{
					case UIType.Slider:	
						slider.value = (float)_v;
						break;
					case UIType.InputField:
						inputField.text = _v.ToString();
						break;
					case UIType.Toggle:
						toggle.isOn = (bool)_v;
						break;	
					case UIType.Text:
						text.text = _v.ToString();
						break;
					case UIType.Dropdown:
						dropdown.value = (int)_v;
						break;
					case UIType.RectTransform:
						rTransform.sizeDelta = new Vector2(((float)_v * rTransformWidth) / (float)_init, rTransform.sizeDelta.y);
						break;
				}
			}
		}
		
		void SetValueFloat(float _value)
		{
			var _f = (FloatType)data;
			// Round float value to 2 decimal places
			float _rounded =  Mathf.Round(_value*100.0f) / 100.0f;
			_f.Value = _rounded;
		}
		
		void SetValueInt(int _value)
		{
			var _i = (IntType)data;
			_i.Value = _value;
		}
		
		void SetValueString(string _string)
		{
			var _s = (StringType)data;
			_s.Value = _string;
		}
		
		void SetValueBool(bool _bool)
		{
			var _b = (BoolType)data;
			_b.Value = _bool;
		}
		
	}
}