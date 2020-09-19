using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Databox.Ed
{
	[CustomEditor(typeof(DataboxUIBinding))]
	public class DataboxUIBindingEditor : Editor {
	
		public override void OnInspectorGUI()
		{
			
			DrawDefaultInspector();
			
		}
	}
}