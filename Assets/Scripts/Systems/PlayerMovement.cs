using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D player;
    public float runSpeed = 2f;
    float timer;
    float timeout = 0.3f;

    public static Vector2 gravity;
    public Camera Camera;
    public Text PopUptxt;

    public float jumpForce = 400f;
    bool jump = false;
    bool fly = false;
    public bool Grounded = true;
    public float gravityScale;
    static public int projectilechoice = 1;

    enum GravityDirection { Down, Left, Up, Right };
    GravityDirection m_GravityDirection;





















    public bool lookRight = true;

    public float moveInput;

    private Animator anim;
    //Initialization
    void Start()
    {
        m_GravityDirection = GravityDirection.Down; // direct gravity be goin
        player = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();


    }


    //Called once a frame
    void Update()
    {






        if (Input.GetButton("Horizontal"))
        {
            if (Grounded == true)
            {

                anim.Play("Run");

            }
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            anim.Play("Idle");
        }


        if (Input.GetButton("1")) //Melee
        {
            projectilechoice = 1;
            PopUptxt.text = "Melee attack Selected";
            Invoke("TextReset", 2f);
        }
        if (CharacterManager.SkillFireballU == true)
        {
            if (Input.GetButton("2")) //Fireball
            {
                projectilechoice = 2;
                PopUptxt.text = "Fireball attack Selected";
                Invoke("TextReset", 2f);

            }
        }

        if (Input.GetButton("3"))//Gun
        {
            projectilechoice = 3;
            PopUptxt.text = "Gun Selected";
            Invoke("TextReset", 2f);

        }


        if (Grounded)
        {
            fly = false;
        }
        else
        {
            fly = true;
        }






        if (Input.GetButton("D"))
        {
            //Debug.Log("D Pressed");
            //player.velocity = new Vector2(player.velocity.x, player.velocity.y);
            //player.AddForce(new Vector2(runSpeed, 0));
            if (lookRight == false)
            {
                flip();
            }

            if (Grounded == true)
            {
                //anim.Play("Run");
            }

            if (fly == true)
                {
                    //anim.Play("FlyRight");
                }
        }

        if (Input.GetButton("A")) // When press A ( Move Right )
        {

            //player.velocity = new Vector2(player.velocity.x, player.velocity.y);
            //player.AddForce(new Vector2(-runSpeed, 0));






            if (fly == false) // If moving right and not flying
            {

                if (lookRight == true) // If looking Right flip cause your pressing A and going left most likely.
                {
                    flip(); //Flip();
                }
            }

            if (fly == true)
            {

            }
        }

        if (Input.GetButtonUp("w")) // Fly after jump?
        {
            //anim.Play("Idle");
        }


        if (Input.GetButtonDown("Space"))
            {
                timer = Time.time;
            if (Grounded)
            {

            }
        }
        else if (Input.GetButton("w"))
            {
                if (Time.time - timer > timeout)
                {
                    fly = true;
                    //anim.Play("FlyUp");
                    Debug.Log("time");
                }
            }
            else
            {
                timer = float.PositiveInfinity;
            }


        

    }

    
    private void FixedUpdate()
    {

        runSpeed = CharacterManager.Dexterity / 4 + CharacterManager.Strength / 4 + 5;




        if (Input.GetButton("Horizontal"))
        {

            //Use the two store floats to create a new Vector2 variable movement.
            //Vector2 movement = new Vector2(-1, 0);

            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            //player.AddForce(movement * runSpeed);

            moveInput = Input.GetAxis("Horizontal");
            player.velocity = new Vector2(moveInput * runSpeed, player.velocity.y);

        }


        if (Input.GetButton("Space"))
        {
            if (Grounded)
            {
                player.velocity = new Vector2(player.velocity.x, 0);
                player.AddForce(new Vector2(0, jumpForce));
                anim.Play("Jump");

            }
        }


        if (Input.GetButton("S"))
        {

                player.velocity = new Vector2(player.velocity.x, 0);
                player.AddForce(new Vector2(0, -400));

        }




        //Move our Character
        //controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;

        if (timer >= timeout)
        {
            //timer = 0;
        }

        if (Input.GetButton("L"))
        {
            //Store the current horizontal input in the float moveHorizontal.
            //float moveHorizontal = Input.GetAxis("Horizontal");

            //Store the current vertical input in the float moveVertical.
            //float moveVertical = Input.GetAxis("Vertical");

            //Use the two store floats to create a new Vector2 variable movement.
            //Vector2 movement = new Vector2(moveHorizontal, 0);

            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            //player.AddForce(movement * runSpeed);


        }

    }





    void TextReset()
    {
        PopUptxt.text = "";
    }





    private void OnCollisionStay2D(Collision2D collider)
    {

        if (collider.transform.tag == "Floor" || collider.transform.tag == "Item")
        {
            CheckIfGrounded();
        }
        

        //if (other.transform.tag == "Floor")
        //{
        //Grounded = true;
        //}
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        Grounded = false;



        //if (other.transform.tag == "Floor")
        //{
            //grounded = false;
        //}
    }

    public void flip()
    {
        lookRight = !lookRight;
        Vector3 oposcale = transform.localScale;
        oposcale.x *= -1;
        transform.localScale = oposcale;
    }

    public void CheckIfGrounded()
    {
        RaycastHit2D[] hits;

        //We raycast down 1 pixel from this position to check for a collider
        Vector2 positionToCheck = transform.position;
        hits = Physics2D.RaycastAll(positionToCheck, new Vector2(0, -1), 0.01f);

        //if a collider was hit, we are grounded
        if (hits.Length > 0)
        {
            Grounded = true;
        }
    }


}

