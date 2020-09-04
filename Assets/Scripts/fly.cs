using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour
{
    public float jumpForce = 150;
    public float fallForce = -150;
    public bool grounded;
    bool canDoubleJump;
    Rigidbody2D rb;


    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        anim = gameObject.GetComponent<Animator>();

        canDoubleJump = false;
    }


    private void Update()
    {
        if (Input.GetButton("w"))
        {
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce));
                anim.Play("Jump");

            }
        }
    }

    private void FixedUpdate()
    {

        if (Input.GetButton("S"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, fallForce));
        }


        if (Input.GetButton("w"))
        {
            if (grounded)
            {

            }

            else
            {
                if (canDoubleJump == true)
                {
                    canDoubleJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, jumpForce));
                    
                }
            }
        }
    }
    public GameObject other;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Floor")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Floor")
        {
            grounded = false;
        }
    }
}
