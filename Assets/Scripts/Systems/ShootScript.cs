﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject crosshairs;
    public GameObject player;
    public GameObject Shoothole;
    public GameObject projectilePrefab1;
    public GameObject projectilePrefab2;
    public GameObject projectilePrefab3;// Work in Progress
    public GameObject projectilePrefab4;
    public GameObject Explosion;
    public Animator anim;
    public static int projectilePrefabchoice; // Work in Progress


    public float projectileSpeed = 70.0f;
    public float projectileSpeedMelee = 70.0f;
    public bool facingRight;

    public int projectilesize = 1;
    //public int projectilesize2;




    public bool SkillTeleU = true; // "Tele" Being short for telekinesis
    public bool SkillTeleExplodeU = true;
    public bool SkillTeleDupeU = true;
    public bool SkillFireballU = true;
    public bool SkillKameU = true;
    public bool SkillFlyU = true;
    public bool SkillTeleportU = true;
    public bool SkillDoubleJumpU = true;









    public float maxAngle;

    public float sensitivityY;
    //public Transform camera;

    private Collider col;

    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        Cursor.visible = true;
        //projectilePrefabchoice = ;


        col = GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        int projectilePrefabchoice = PlayerMovement.projectilechoice;

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        //crosshairs.transform.position = new Vector2(target.x, target.y);

        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);




        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerMovement.projectilechoice == 1)
            {

                

                if (CharacterManager.Stamina >= 5)//Melee
                {
                    anim.Play("Melee1");
                    Invoke("PlayerAttack", 0.25f);

                }

            }

            if (PlayerMovement.projectilechoice >= 2)
            {

                if (SkillFireballU == true)
                {
                    if (CharacterManager.Ki >= 5)//Projectile 2 ( Fireball once unlocked )
                    {
                        CharacterManager.Ki = CharacterManager.Ki - 5;
                        float distance = difference.magnitude;
                        Vector2 direction = difference / distance;
                        direction.Normalize();
                        fireProjectile(direction, rotationZ);
                        //Debug.Log("Pew"); //work in progress2

                    }
                }
            }

            if (PlayerMovement.projectilechoice == 3)
            {

                if (CharacterManager.Ki >= 5)//Fire Gun bullet Projectile
                {
                    CharacterManager.Ki = CharacterManager.Ki - 5;
                    float distance = difference.magnitude;
                    Vector2 direction = difference / distance;
                    direction.Normalize();
                    fireProjectile(direction, rotationZ);
                    //Debug.Log("Pew"); //work in progress2

                }
            }
        }


    }





    private void FixedUpdate()
    {



        if (PlayerMovement.projectilechoice == 1)
        {
            projectileSpeedMelee = 367;
            //projectilesize2 = 1;
        }


        projectileSpeed = CharacterManager.Wisdom / 5 + 2;

        projectilesize = CharacterManager.Wisdom / 200 + projectilesize;


        SkillTeleU = (CharacterManager.SkillTeleU); // "Tele" Being short for telekinesis
        SkillTeleExplodeU = (CharacterManager.SkillTeleExplodeU);
        SkillTeleDupeU = (CharacterManager.SkillTeleDupeU);
        SkillFireballU = (CharacterManager.SkillFireballU);
        SkillKameU = (CharacterManager.SkillKameU);
        SkillFlyU = (CharacterManager.SkillFlyU);
        SkillTeleportU = (CharacterManager.SkillTeleportU);
        SkillDoubleJumpU = (CharacterManager.SkillDoubleJumpU);


    }
    //GameObject b;
    void fireProjectile(Vector2 direction, float RotationZ)
    {


        if (PlayerMovement.projectilechoice == 1)
        {   // if press one choose. This is the last thing i worked on
            GameObject a = Instantiate(projectilePrefab1) as GameObject;
            a.transform.position = player.transform.position;
            a.transform.localScale = new Vector2(projectilesize, projectilesize);
            a.transform.rotation = Quaternion.Euler(0.0f, 0.0f, RotationZ);
            a.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            Debug.Log("Projectile choice = 1");
        }

        if (PlayerMovement.projectilechoice == 2)
        {   // if press two choose. This is the last thing i worked on
            GameObject b = Instantiate(projectilePrefab2) as GameObject;
            b.transform.position = Shoothole.transform.position;
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, RotationZ);
            b.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            Debug.Log("Projectile choice = 2");
        }

        if (PlayerMovement.projectilechoice == 3)
        {   // if press 3 choose. This is the last thing i worked on
            GameObject b = Instantiate(projectilePrefab3) as GameObject;
            b.transform.position = Shoothole.transform.position;
            b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, RotationZ);
            b.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            Debug.Log("Projectile choice = 3");
        }

        if (PlayerMovement.projectilechoice == 4)
        {   // if press one choose. This is the last thing i worked on
            GameObject a = Instantiate(projectilePrefab4) as GameObject;
            a.transform.position = player.transform.position;
            a.transform.localScale = new Vector2(projectilesize, projectilesize);
            a.transform.rotation = Quaternion.Euler(0.0f, 0.0f, RotationZ);
            a.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            Debug.Log("Projectile choice = 4");
        }

    }



    private void PlayerAttack()
    {

        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        //crosshairs.transform.position = new Vector2(target.x, target.y);

        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        CharacterManager.Stamina = CharacterManager.Stamina - 7;
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        direction.Normalize();
        fireProjectile(direction, rotationZ);
        //Debug.Log("Pew"); //work in progress2
        anim.Play("Idle");
    }

    private void Flip(float horizontal)
    {

        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;



            Debug.Log("Flipped");

        }

    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log("YAY");
    }
}