using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameobjectdisapear : MonoBehaviour
{
    public GameObject projectile;
    public GameObject projectile2;
    public GameObject projectile3;
    public float timeout = 3.0f;
    public float timer = 0.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timer > timeout)
        {
            GameObject.Destroy(projectile, 1);
        }
    }
}
