using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    
    private int CrateHealth = 10;
    public GameObject crate;
    
    // Start is called before the first frame update
    void Start()
    {
        crate = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (CrateHealth <= 0)
        {
            Destroy(crate);
            Debug.Log("Crate Destroyed");
        }
    }



    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == "Projectile")
        {

            Hit();
            Invoke("HitEvent", 0.4f);


        }
    }

    private void Hit()
    {
        Debug.Log("Hit Crate");
        CrateHealth = CrateHealth - CharacterManager.Damage;
    }
}
