#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Create and add a new data value to a databox object.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/adddata")]     
	public class AddData : FsmStateAction
	{
		[ActionSection("Databox Data")]
		[ObjectType(typeof(DataboxObject))]
		[RequiredField]  
		[UIHint(UIHint.Variable)]
		public FsmObject databoxObject;
		
		public string tableID;
		public bool useOwnerInstanceID;
		public string entryID;
		public string valueID;
		public int selectedTableIndex;
		public int selectedEntryIndex;
		public int selectedValueIndex;
				
		public enum DataType
		{
			Float,
			Int,
			Bool,
			String,
			Color,
			Quaternion,
			Vector2,
			Vector3
		}
		
		public DataType dataType;
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			var _db = databoxObject.Value as DataboxObject;
			
			if (useOwnerInstanceID)
			{
				entryID = Owner.gameObject.GetInstanceID().ToString();
			}
			
			DataboxType _data = null;
			
			switch (dataType)
			{
				case DataType.Float:
					_data = new FloatType();
					break;
				case DataType.Int:
					_data = new IntType();
					break;
				case DataType.Bool:
					_data = new BoolType();
					break;
				case DataType.String:
					_data = new StringType();
					break;
				case DataType.Color:
					_data = new ColorType();
					break;
				case DataType.Quaternion:
					_data = new QuaternionType();
					break;
				case DataType.Vector2:
					_data = new Vector2Type();
					break;
				case DataType.Vector3:
					_data = new Vector3Type();
					break;
			}
			
			_db.AddData(tableID, entryID, valueID, _data);
			
			Finish();
		}
	}
}
#endif