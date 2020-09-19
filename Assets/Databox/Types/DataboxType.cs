using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Databox
{
	/// <summary>
	/// Databox Type
	/// Inherit from DataboxType to make sure your class will be supported by Databox.
	/// Several virtual methods are available to integrate your class into Databox.
	/// </summary>
	[System.Serializable]
	public class DataboxType
	{
		public virtual void Reset(){} // used to reset variables back to its initial value
		public virtual void DrawEditor(){} // draw custom editor gui fields
		public virtual void DrawEditor(DataboxObject _databoxObject) {} // same as DrawEditor() but with additional DataboxObject parameter.
		public virtual void DrawInitValueEditor(){} // draw custom editor gui only for init values
		
		public virtual string Equal(DataboxType _value){return "";} // Used for the cloud sync comparison. Compare the input value with the current value and return a string if there's a change.
		
		public virtual void Convert(string _value){} // Convert csv string value to DataboxType value
		
		public delegate void ValueChanged(DataboxType _data);
		public ValueChanged OnValueChanged;
		
		public DataboxType (){}
	}
	
	
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
	public class DataboxTypeAttribute : Attribute
	{
		public string Name{get;set;}
	}
}