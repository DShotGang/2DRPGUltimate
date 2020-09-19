#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Databox;

/// <summary>
/// Add Databox - PlayMaker define symbol to the player settings
/// </summary>
namespace Databox.PlayMaker
{
	public class EnablePlayMakerIntegrations : MonoBehaviour
	{
	
		/// <summary>
		/// Symbols that will be added to the editor
		/// </summary>
		public static readonly string [] Symbols = new string[] {
			"DATABOX_PLAYMAKER"
		};
		
		/// <summary>
		/// Add databox define symbols.
		/// </summary>
		[MenuItem("Tools/Databox/Enable PlayMaker integration")]
		public static void Install ()
		{
			if (EditorUtility.DisplayDialog("Enable PlayMaker integration", "Do you want to enable the Databox - PlayMaker integration? Please make sure PlayMaker is available in your project.", "Yes", "No"))
			{
				string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
				List<string> allDefines = definesString.Split ( ';' ).ToList ();
				allDefines.AddRange ( Symbols.Except ( allDefines ) );
				PlayerSettings.SetScriptingDefineSymbolsForGroup (
				EditorUserBuildSettings.selectedBuildTargetGroup,
				string.Join ( ";", allDefines.ToArray () ) );
			}
		}
	
	}
}
#endif