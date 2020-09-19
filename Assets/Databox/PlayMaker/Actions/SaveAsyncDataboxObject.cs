#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using Databox;

namespace Databox.PlayMaker
{
	[ActionCategory("Databox")]
	[HutongGames.PlayMaker.Tooltip("Saves the selected Databox object to file.")]
	[HelpUrl("http://databox.doorfortyfour.com/documentation/playmaker/actions/savedataboxobject")]   
	public class SaveAsyncDataboxObject : FsmStateAction
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
				_db.OnDatabaseSaved += DatabaseSaved;
			}
		}
		
		public override void OnEnter()
		{
			base.OnEnter();
			
			var _db = databoxObject.Value as DataboxObject;
			if (!useCustomPath.Value)
			{
				StartCoroutine(_db.SaveDatabaseAsync());
			}
			else
			{
				StartCoroutine(_db.SaveDatabaseAsync(customPath.Value));
			}
		}
		
		private void DatabaseSaved()
		{
			Finish();
		}
	}
}
#endif