#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Remove a complete table from a databox object")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/removetable")]    
	public class RemoveTable : FsmStateAction
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
			
			_db.RemoveDatabaseTable(data.selectedTable);
			
			Finish();
		}
	}
}
#endif