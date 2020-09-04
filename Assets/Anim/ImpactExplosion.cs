using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImpactExplosion : MonoBehaviour
{


    public GameObject Projectile;
    public GameObject Explosion;

    int BP;
    public int ExplosionSizeMultiplier;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        BP = CharacterManager.BattlePower / 237 * ExplosionSizeMultiplier;
    }


    private void OnCollisionStay2D(Collision2D collider)
    {
        

        if (collider.transform.tag == "Destroyable"  || collider.transform.tag == "Floor" || collider.transform.tag == "Item")
        {

            GameObject a = Instantiate(Explosion) as GameObject;
            a.transform.localScale = new Vector2(BP/2, BP/2);
            a.transform.position = Projectile.transform.position;
            Destroy(Projectile);
        }
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        
        
        //if (other.transform.tag == "Floor")
        //{
        //grounded = false;
        //}
    }



}
