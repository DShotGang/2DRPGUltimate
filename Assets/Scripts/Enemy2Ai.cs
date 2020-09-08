using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Ai : MonoBehaviour
{
    public bool Alive = true; // Starts alive and creates Alive bool variable

    public int EnemyLevel = 1;
    public int EnemyHealth = 50;
    public int damage = 10;

    public float speed = 3;
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


        EnemyLevel = CharacterManager.Level / 4 + 1;
        EnemyHealth = EnemyLevel * 20 + 50;

    }

    // Update is called once per frame
    void Update()
    {
        // If Alive
        if(Alive == true)
        {
            if (Enemy.position.x < target.position.x)
            {
                //face right
                transform.localScale = new Vector3(6, 6, 6);
            }
            else if (Enemy.position.x > target.position.x)
            {
                //face left
                transform.localScale = new Vector3(-6, 6, 6);
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
        CharacterManager.Experience = CharacterManager.Experience + 4 * EnemyLevel;

        Destroy(gameObject, 0.6f);
    }

    public void attackEvent()
    {
        int EnemyDamage = 5;
        CharacterManager.Health = CharacterManager.Health - EnemyDamage;
    }

    private void Hit()
    {
        Debug.Log("Hit Enemy");
        beingHit = true;
        anim.Play("Take Hit");
        EnemyHealth = EnemyHealth - CharacterManager.KiDamage;
    }
        
    private void HitEvent()
    {
        beingHit = false;
    }


    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == "Projectile")
        {

            Hit();
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
