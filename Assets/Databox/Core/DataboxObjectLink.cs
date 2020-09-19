using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple component which can be added to any gameobject to
// establish a link between the object and the Databox object

namespace Databox
{
	public class DataboxObjectLink : MonoBehaviour
	{
		public DataboxObject database;
		
		public string tableID;
		public string objectID;
	}
}