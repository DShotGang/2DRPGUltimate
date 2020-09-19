#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Remove a single value from an entry in a databox object.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/removevalue")]    
	public class RemoveValue : FsmStateAction
	{
		[ActionSection("Databox Data")]
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
			
			_db.RemoveValue(data.selectedTable, data.selectedEntry, data.selectedValue);
			
			Finish();
		}
	}
}
#endif