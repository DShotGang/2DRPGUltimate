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
	[HutongGames.PlayMaker.Tooltip("Reset a complete table back to it's initial values")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/resettable")]     
	public class ResetTable : FsmStateAction
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
			
			_db.ResetToInitValues(data.selectedTable);
			
			Finish();
		}
	}
}
#endif