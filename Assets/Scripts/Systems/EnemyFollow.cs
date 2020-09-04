using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    public float speed;
    public float stoppingDistance;

    private Transform target;
    private Animator anim;
    private GameObject skele;
    public bool Alive = true; // Starts alive and creates Alive bool variable
    


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





        }
        else if (Alive == false)
        {
            anim.Play("SkeletonDead");
            Destroy(skele, 3.4f); // Lame way of destroying after an animation, the right way is to use animation triggers i think, do it later.
        }

    }


    private bool lookRight = true;
    private void flip()
    {
        lookRight = !lookRight;
        Vector3 oposcale = transform.localScale;
        oposcale.x *= -1;
        transform.localScale = oposcale;
    }
}
