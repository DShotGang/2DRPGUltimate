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
	[HutongGames.PlayMaker.Tooltip("Get data from Databox object and store it to a PlayMaker variable.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/getdata")]     
	public class GetData : FsmStateAction
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
			GameObject,
			Material,
			Texture,
			Sprite,
			AudioClip
		}
		
		public DataType dataType;
		
		public FsmFloat storeResultFloat;
		public FsmInt storeResultInt;
		public FsmBool storeResultBool;
		public FsmColor storeResultColor;
		public FsmString storeResultString;
		public FsmVector2 storeResultVector2;
		public FsmVector3 storeResultVector3;
		public FsmQuaternion storeResultQuaternion;
		public FsmGameObject storeResultGameObject;
		public FsmMaterial storeResultMaterial;
		public FsmTexture storeResultTexture;
		public FsmObject storeResultSprite;
		public FsmObject storeResultAudioClip;
		
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
					storeResultInt.Value = _valueInt.Value;
					break;
				case DataType.Float:
					var _valueFloat = _db.GetData<FloatType>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultFloat.Value = _valueFloat.Value;
					break;
				case DataType.Bool:
					var _valueBool = _db.GetData<BoolType>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultBool.Value = _valueBool.Value;
					break;
				case DataType.String:
					var _valueString = _db.GetData<StringType>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultString.Value = _valueString.Value;
					break;
				case DataType.Color:
					var _valueColor = _db.GetData<ColorType>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultColor.Value = _valueColor.Value;
					break;
				case DataType.Quaternion:
					var _valueQuaternion = _db.GetData<QuaternionType>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultQuaternion.Value = _valueQuaternion.Value;
					break;
				case DataType.Vector2:
					var _valueVector2 = _db.GetData<Vector2Type>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultVector2.Value = _valueVector2.Value;
					break;
				case DataType.Vector3:
					var _valueVector3 = _db.GetData<Vector3Type>(data.selectedTable, data.selectedEntry, data.selectedValue);
					storeResultVector3.Value = _valueVector3.Value;
					break;
				case DataType.GameObject:
					var _valueGameObject = _db.GetData<ResourceType>(data.selectedTable, data.selectedEntry, data.selectedValue).Load() as GameObject;
					storeResultGameObject.Value = _valueGameObject;
					break;
				case DataType.Material:
					var _valueMaterial = _db.GetData<ResourceType>(data.selectedTable, data.selectedEntry, data.selectedValue).Load() as Material;
					storeResultMaterial.Value = _valueMaterial;
					break;
				case DataType.Texture:
					var _valueTexture = _db.GetData<ResourceType>(data.selectedTable, data.selectedEntry, data.selectedValue).Load() as Texture;
					storeResultTexture.Value = _valueTexture;
					break;
				case DataType.Sprite:
					var _valueSprite = _db.GetData<ResourceType>(data.selectedTable, data.selectedEntry, data.selectedValue).Load() as Sprite;
					storeResultSprite.Value = _valueSprite;
					break;
				case DataType.AudioClip:
					var _valueAudioClip = _db.GetData<ResourceType>(data.selectedTable, data.selectedEntry, data.selectedValue).Load() as AudioClip;
					storeResultAudioClip.Value = _valueAudioClip;
					break;
			}
		
			
			Finish();
		}
	}
}
#endif