using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImpactExplosion : MonoBehaviour
{


    public GameObject Projectile;
    public GameObject Explosion;
    

    int BP = 2;
    public int ExplosionSizeMultiplier = 1 / 2;




    // Start is called before the first frame update
    void Start()
    {
        ExplosionSizeMultiplier = CharacterManager.Wisdom / 5 + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }


    private void OnCollisionStay2D(Collision2D collider)
    {
        

        if (collider.transform.tag == "Destroyable"  || collider.transform.tag == "Floor" || collider.transform.tag == "Item" || collider. transform.tag == "Enemy")
        {

            GameObject a = Instantiate(Explosion) as GameObject;
            a.transform.localScale = new Vector2(ExplosionSizeMultiplier, ExplosionSizeMultiplier);
            a.transform.position = Projectile.transform.position;
            Destroy(Projectile);
        }

        if (collider.transform.tag == "Enemy")
        {

            Debug.Log("Hit Enemy");
            GameObject a = Instantiate(Explosion) as GameObject;
            a.transform.localScale = new Vector2(BP / 2, BP / 2);
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
