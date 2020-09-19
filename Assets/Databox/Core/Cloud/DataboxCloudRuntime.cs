using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Databox
{
public class DataboxCloudRuntime : MonoBehaviour
{
	
	public void ForceDownloadRuntime(DataboxObject _database)
	{		
		StartCoroutine(DataboxCloud.GetDataIE());
	}
}
}
