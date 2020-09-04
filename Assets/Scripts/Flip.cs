using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{

    
    public bool lookRight = true;
    public Rigidbody2D player;
    public int movespeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void FixedUpdate()
    {





        if (Input.GetButtonDown("a")) // if horizontal button is pressed to the positive value in input control in unity
        {                
            transform.position = Vector2.MoveTowards(transform.position, player.position, movespeed * Time.deltaTime);
            flip(); //Flip();
        }

        if (Input.GetButtonDown("d")) // if horizontal button is pressed to the positive value in input control in unity
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, movespeed * Time.deltaTime);
            flip(); //Flip();
        }
    }

    void flip()
    {
        lookRight = !lookRight;
        Vector3 oposcale = transform.localScale;
        oposcale.x *= -1;
        transform.localScale = oposcale;
    }

}
