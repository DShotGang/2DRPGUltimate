#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Load the database file of a DataboxObject")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/loaddataboxobject")]     
	public class LoadDataboxObject : FsmStateAction
	{
	
		[ObjectType(typeof(DataboxObject))]
		public FsmObject databoxObject;
		
		public FsmBool useCustomPath;
		public FsmString customPath;
		
		
		public override void Awake()
		{
			if (databoxObject.Value != null)
			{
				var _db = databoxObject.Value as DataboxObject;
				_db.OnDatabaseLoaded += DatabaseLoaded;
			}
		}
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			var _db = databoxObject.Value as DataboxObject;
			if (!useCustomPath.Value)
			{
				_db.LoadDatabase();
			}
			else
			{
				_db.LoadDatabase(customPath.Value);
			}
		}
		
		private void DatabaseLoaded()
		{
			//Debug.Log("db loaded");
			Finish();
		}
	}
}
#endif