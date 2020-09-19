#if DATABOX_PLAYMAKER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Databox.PlayMaker
{
	public class Data
	{
		
		public bool nonExistent;
		public bool useOwnerInstanceID;
			
		public string selectedTable;
		public string selectedEntry;
		public string selectedValue;
			
		public int selectedTableIndex;
		public int selectedEntryIndex;
		public int selectedValueIndex;
			
		public Data(){}
		
	}
}
#endif