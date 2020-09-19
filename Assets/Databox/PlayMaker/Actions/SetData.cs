#if DATABOX_PLAYMAKER
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Set data from a PlayMaker variable to a selected Databox value.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/setdata")]     
	public class SetData : FsmStateAction
	{
		
		[ObjectType(typeof(DataboxObject))]
		[RequiredField]  
		[UIHint(UIHint.Variable)]
		public FsmObject databoxObject;
		
		public Data data = new Data();

		public enum DataType
		{
			Float,
			Int,
			Bool,
			String,
			Color,
			Quaternion,
			Vector2,
			Vector3,
		}
		
		public DataType dataType;
		
		public FsmFloat fromFloat;
		public FsmInt fromInt;
		public FsmBool fromBool;
		public FsmColor fromColor;
		public FsmString fromString;
		public FsmVector2 fromVector2;
		public FsmVector3 fromVector3;
		public FsmQuaternion fromQuaternion;
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			var _db = databoxObject.Value as DataboxObject;
			
			if (data.useOwnerInstanceID)
			{
				data.selectedEntry = Owner.gameObject.GetInstanceID().ToString();
			}
		
			switch (dataType)
			{
			case DataType.Int:
				var _valueInt = _db.GetData<IntType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueInt.Value = fromInt.Value;
				break;
			case DataType.Float:
				var _valueFloat = _db.GetData<FloatType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueFloat.Value = fromFloat.Value;
				break;
			case DataType.Bool:
				var _valueBool = _db.GetData<BoolType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueBool.Value = fromBool.Value;
				break;
			case DataType.String:
				var _valueString = _db.GetData<StringType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueString.Value = fromString.Value;
				break;
			case DataType.Color:
				var _valueColor = _db.GetData<ColorType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueColor.Value = fromColor.Value;
				break;
			case DataType.Quaternion:
				var _valueQuaternion = _db.GetData<QuaternionType>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueQuaternion.Value = fromQuaternion.Value;
				break;
			case DataType.Vector2:
				var _valueVector2 = _db.GetData<Vector2Type>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueVector2.Value = fromVector2.Value;
				break;
			case DataType.Vector3:
				var _valueVector3 = _db.GetData<Vector3Type>(data.selectedTable, data.selectedEntry, data.selectedValue);
				_valueVector3.Value = fromVector3.Value;
				break;
			}
		
			
			Finish();
		}
	}
}
#endif