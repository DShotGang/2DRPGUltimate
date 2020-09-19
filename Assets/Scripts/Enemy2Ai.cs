﻿using System.Collections;
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

    public GameObject EnemyGuy;
    private Transform Enemy;
    private Transform target;
    private Animator anim;
    private Transform RespawnTarget;

    public AudioSource[] audio_source;
    public AudioClip[] audio_clip;
    // Enemy Audio Source array
    // 0, 0-2 - Attacking / Moving
    // 1, 3-5 - Getting Attacked / Death
    // 2, 6-8 - Extra

    bool beingHit;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RespawnTarget = GameObject.Find("EnemyRespawn").GetComponent<Transform>();
        anim = gameObject.GetComponent<Animator>();
        Enemy = GetComponent<Transform>();
        Alive = true;
        beingHit = false;


        EnemyLevel = CharacterManager.Level / 4 + 1;
        EnemyHealth = EnemyLevel * 20 + 50;

        EnemyGuy.transform.position = RespawnTarget.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        // If Alive
        if(Alive == true)
        {

        }


    }

    private void FixedUpdate()
    {
        if (Alive == true)
        {

            if (beingHit == false)
            {


                //if (Vector2.Distance(transform.position, RespawnTarget.position) <= 10) {}
                

                    if (Vector2.Distance(transform.position, target.position) <= 9)
                    {


                        if (Vector2.Distance(transform.position, target.position) > stoppingDistance) // If Skeleton's distance to target is more than stopping distance, Move and play anim
                        {

                            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Move
                            anim.Play("Run"); // Play animation
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
            else
             { 
                
                if (Vector2.Distance(transform.position, RespawnTarget.position) >= stoppingDistance)
                 {
                        transform.position = Vector2.MoveTowards(transform.position, RespawnTarget.position, speed * Time.deltaTime);
                        anim.Play("Run");
                        if (transform.position.x < RespawnTarget.position.x)
                        {
                            //face right
                            transform.localScale = new Vector3(6, 6, 1);
                        }
                        else if (Enemy.position.x > RespawnTarget.position.x)
                        {
                            //face left
                            transform.localScale = new Vector3(-6, 6, 1);
                        }
                 }
                else
                    {
                        anim.Play("Idle");
                    }
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
        PlaySelectSound(0, 5);
        CharacterManager.Experience = CharacterManager.Experience + 4 * EnemyLevel;
        Invoke("Respawn", 0.5f);
        Destroy(gameObject, 0.52f);
    }

    public void attackEvent()
    {
        int EnemyDamage = 5;
        CharacterManager.Health = CharacterManager.Health - EnemyDamage;
        PlayRandomSound(0, 1, 2);
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

    private void Respawn()
    {
        Instantiate(EnemyGuy);
        RespawnTarget = GameObject.Find("EnemyRespawn").GetComponent<Transform>();
        EnemyGuy.transform.position = RespawnTarget.transform.position;
    }



    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == "Projectile")
        {

            ProjectileHit();
            Invoke("HitEvent", 0.4f);
            PlayRandomSound(1, 3, 4);

        }

        if (collider.transform.tag == "Melee")
        {

            MeleeHit();
            Invoke("HitEvent", 0.4f);
            PlayRandomSound(1, 3, 4);

        }


    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            //beingHit = false;
        }
    }

    public void PlayRandomSound(int audioplayerselect2, int cliprange1, int cliprange2)
    {
        AudioSource chosenaudioplayer = audio_source[audioplayerselect2];
        if (chosenaudioplayer.isPlaying == false)
        {
            int selection = Random.Range(cliprange1, cliprange2);
            chosenaudioplayer.PlayOneShot(audio_clip[selection]);
        }
    }

    public void PlaySelectSound(int audioplayerselect, int selection)
    {

        AudioSource chosenaudioplayer2 = audio_source[audioplayerselect];
        if (chosenaudioplayer2.isPlaying == false)
        {
            chosenaudioplayer2.PlayOneShot(audio_clip[selection]);
        }
    }

}
