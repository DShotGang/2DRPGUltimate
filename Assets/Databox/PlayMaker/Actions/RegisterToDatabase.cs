#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Register a new entry to a Databox Object. Great for runtime generated objects.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/registertodatabase")]    
	public class RegisterToDatabase : FsmStateAction
	{
		// From
		[ObjectType(typeof(DataboxObject))]
		[RequiredField]  
		[UIHint(UIHint.Variable)]
		public FsmObject databoxObject;
		
		public string selectedTable;	
		public string selectedEntry;
		public int selectedTableIndex;
		public int selectedEntryIndex;
		
		// To
		[ObjectType(typeof(DataboxObject))]
		[RequiredField]  
		[UIHint(UIHint.Variable)]
		public FsmObject toDataboxObject;
		
		public bool useOwnerInstanceID;
		public string toEntryID;
		public int toEntryIndex;
		
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			var _fromDB = databoxObject.Value as DataboxObject;
			var _toDB = toDataboxObject.Value as DataboxObject;
			
			if (useOwnerInstanceID)
			{
				toEntryID = Owner.gameObject.GetInstanceID().ToString();
			}
			
			DataboxObject.DatabaseEntry _dbEntry;
			
			if (!_toDB.DB.ContainsKey(selectedTable))
			{
				_fromDB.RegisterToDatabase(_toDB, selectedTable, selectedEntry, toEntryID);
			}
			else
			{
				if (!_toDB.DB[selectedTable].entries.ContainsKey(toEntryID))
				{
					_fromDB.RegisterToDatabase(_toDB, selectedTable, selectedEntry, toEntryID);
				}
			}
			
			Finish();
		}
	}
}
#endif