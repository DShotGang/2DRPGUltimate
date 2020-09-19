using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseNearestEnemy : MonoBehaviour
{

 


    public readonly static HashSet<ChooseNearestEnemy> Pool = new HashSet<ChooseNearestEnemy>();

    private void OnEnable()
    {
        ChooseNearestEnemy.Pool.Add(this);
    }

    private void OnDisable()
    {
        ChooseNearestEnemy.Pool.Remove(this);
    }



    public static ChooseNearestEnemy FindClosestEnemy(Vector3 pos)
    {
        ChooseNearestEnemy result = null;
        float dist = float.PositiveInfinity;
        var e = ChooseNearestEnemy.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            float d = (e.Current.transform.position - pos).sqrMagnitude;
            if (d < dist)
            {
                result = e.Current;
                dist = d;
            }
        }
        return result;
    }

}

