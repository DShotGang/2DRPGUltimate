using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFollow : MonoBehaviour
{

    public bool Alive = true; // Starts alive and creates Alive bool variable
    public int EnemyHealth;
    public int EnemyLevel = 1;
    public int EnemyDamage = 10;

    public float speed = 4;
    public float stoppingDistance = 2;

    private Transform Enemy;
    private Transform target;
    private Animator anim;

    bool beingHit;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
        Enemy = GetComponent<Transform>();
        Alive = true;
        beingHit = false;
        EnemyLevel = CharacterManager.Level / 2 + 1;
        EnemyHealth = EnemyLevel * 4 + 15;
        EnemyDamage = EnemyLevel * 2 + 10;
    }

    // Update is called once per frame
    void Update()
    {
        // If Alive
        if (Alive == true)
        {
            if (beingHit == false)
            {
                if (Enemy.position.x < target.position.x)
                {
                    //face right
                    transform.localScale = new Vector3(5, 5, 1);
                }
                else if (Enemy.position.x > target.position.x)
                {
                    //face left
                    transform.localScale = new Vector3(-5, 5, 1);
                }
            }
        }


    }

    private void FixedUpdate()
    {
        if (Alive == true)
        {



            









            if (beingHit == false)
            {



                if (Vector2.Distance(transform.position, target.position) > stoppingDistance) // If Skeleton's distance to target is more than stopping distance, Move and play anim
                {

                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Move
                    anim.Play("Run"); // Play animation

                }


                if (Vector2.Distance(transform.position, target.position) <= stoppingDistance) // if equals to or less than stopping distance
                {

                    anim.Play("Attack1"); // Play animation

                }
            }


            if (EnemyHealth <= 0) { Alive = false; }

        }
        if (Alive == false)
        {
            anim.Play("Death");

            deathEvent();

        }
    }

    public void deathEvent() // New way of destroying after an animation, This is way better. Using animation events.
    {
        CharacterManager.Experience = CharacterManager.Experience + 2 + EnemyLevel / 2;

        Destroy(gameObject, 0.6f);
    }

    public void attackEvent()
    {
        int EnemyDamage = 2 * EnemyLevel + 5;
        CharacterManager.Health = CharacterManager.Health - EnemyDamage;
    }

    private void ProjectileHit()
    {
        Debug.Log("Hit Enemy with Projectile");
        beingHit = true;
        anim.Play("Take Hit");
        EnemyHealth = EnemyHealth - CharacterManager.KiDamage;
    }

    private void MeleeHit()
    {
        Debug.Log("Hit Enemy with Melee");
        beingHit = true;
        anim.Play("Take Hit");
        EnemyHealth = EnemyHealth - CharacterManager.Damage;
    }

    private void HitEvent()
    {
        beingHit = false;
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == "Projectile")
        {

            ProjectileHit();
            Invoke("HitEvent", 0.4f);


        }

        if (collider.transform.tag == "Melee")
        {

            MeleeHit();
            Invoke("HitEvent", 0.4f);


        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            //beingHit = false;
        }
    }
}
