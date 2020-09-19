using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Databox
{
	/// <summary>
	/// Databox Type Editor
	/// </summary>
	[System.Serializable]
	[CustomEditor(typeof (DataboxType))]
	public class DataboxTypeEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}