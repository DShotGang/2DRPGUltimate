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
	[HutongGames.PlayMaker.Tooltip("Reset value to it's initial value")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/resetvalue")]     
	public class ResetValue : FsmStateAction
	{
		
		[ObjectType(typeof(DataboxObject))]
		[RequiredField]  
		[UIHint(UIHint.Variable)]
		public FsmObject databoxObject;
		
		public Data data = new Data();

		public override void OnEnter()
		{
			base.OnEnter();
			
			var _db = databoxObject.Value as DataboxObject;
			
			if (data.useOwnerInstanceID)
			{
				data.selectedEntry = Owner.gameObject.GetInstanceID().ToString();
			}
			
			var _val = _db.GetDataUnknown(data.selectedTable, data.selectedEntry, data.selectedValue) as DataboxType;
			
			_val.Reset();
			
			Finish();
		}
	}
}
#endif