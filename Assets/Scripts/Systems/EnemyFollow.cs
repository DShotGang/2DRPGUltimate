using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    public bool Alive = true; // Starts alive and creates Alive bool variable
    public int EnemyHealth = 25;
    public int damage = 2;

    public float speed;
    public float stoppingDistance;

    private Transform target;
    private Animator anim;
    private GameObject skele;




    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
        Alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If Alive


    }

    private void FixedUpdate()
    {
        if (Alive == true)
        {


            if (Vector2.Distance(transform.position, target.position) > stoppingDistance) // If Skeleton's distance to target is more than stopping distance, Move and play anim
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Move
                anim.Play("SkeletonWalk"); // Play animation
            }
            if (Vector2.Distance(transform.position, target.position) <= stoppingDistance) // if equals to or less than stopping distance
            {
                anim.Play("SkeletonAttackA1"); // Play animation
            }



            if (EnemyHealth <= 0) { Alive = false; }

        }
        if (Alive == false)
        {
            anim.Play("SkeletonDead");

            deathEvent();

        }
    }

    public void deathEvent() // New way of destroying after an animation, This is way better. Using animation events.
    {
        CharacterManager.Experience = CharacterManager.Experience + 2;

        Destroy(gameObject, 0.6f);
    }

    public void attackEvent()
    {
        int EnemyDamage = 5;
        CharacterManager.Health = CharacterManager.Health - EnemyDamage;
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        if (collider.transform.tag == "Projectile")
        {

            Debug.Log("Hit Enemy");
            EnemyHealth = EnemyHealth - CharacterManager.Damage;
        }
    }
}
