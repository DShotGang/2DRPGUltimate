﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{

    private Camera myCamera; //For storing the camera
    private RaycastHit2D theClickedObject; //For storing the the Clicked Object we are dragging
    public GameObject Player;
    public GameObject Crate;
    public GameObject Explosion1;
    public GameObject DupeEffect;
    private Transform target;
    private Transform theClickedObjectPos;
    bool TSkill;

    bool SkillSpawnCrateU = true;

    public bool SkillTeleU = true; // "Tele" Being short for telekinesis
    public bool SkillTeleExplodeU = true;
    public bool SkillTeleDupeU = true;
    public bool SkillFireballU = true;
    public bool SkillKameU = true;
    public bool SkillFlyU = true;
    public bool SkillTeleportU = true;
    public bool SkillDoubleJumpU = true;
    


    void Start()
    {
        myCamera = GetComponent<Camera>(); //For getting the camera
    }

    void Update()
    {

        //Store our mouse position at the beginning of the frame for use later
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        target = Player.GetComponent<Transform>();
        //Did we mouse click? "Fire1" is set to use Mouse0 in Edit > Project Settings > Input Manager
        if (Input.GetButtonDown("Fire1"))
        {

            //Shoot a ray at the exact position of our mouse, and store the returned result into theClickedObject
            theClickedObject = Physics2D.Raycast(mousePos, Vector2.zero);


        }

        if (SkillSpawnCrateU == true)
        {

            if (CharacterManager.Spam == true)
            {
                if (Input.GetButton("E")) // Spawn Crate
                {
                    GameObject newItem = Instantiate(Crate) as GameObject;
                    newItem.transform.position = new Vector2(mousePos.x, mousePos.y);
                    print("Crate Spawned!");
                }
            }
            else if (Input.GetButtonDown("E")) // Spawn Crate
            {
                GameObject newItem = Instantiate(Crate) as GameObject;
                newItem.transform.position = new Vector2(mousePos.x, mousePos.y);
                print("Crate Spawned!");
            }
        }



        //Are we holding the mouse button down?
        if (Input.GetButton("Fire1"))
        {

            //Is the collider of our theClickedObject RaycastHit2D variable NOT null?


            // IF OBJECT CLICKED ON HAS "ITEM" TAG
            if (theClickedObject.transform.tag == "Destroyable" || theClickedObject.transform.tag == "Floor" || theClickedObject.transform.tag == "Item" || theClickedObject.transform.tag == "Enemy")
            {


                if (CharacterManager.SkillTeleU == true) // If telekinesis skill unlocked
                {
                    theClickedObject.collider.transform.position = mousePos; // Hold Object to cursor
                }


                if (CharacterManager.SkillTeleExplodeU == true) // If Telekinesis Force Explode Skill is unlocked
                {
                    if (Input.GetButton("F")) // Force Explode held object
                    {
                        Destroy(theClickedObject.collider.gameObject);
                        GameObject boom = Instantiate(Explosion1) as GameObject;
                        boom.transform.position = theClickedObject.transform.position;


                        Debug.Log("You destroyed the " + theClickedObject);
                    }
                }
                if (CharacterManager.SkillTeleDupeU == true) // if skill duplicate is unlocked 
                {

                    if (CharacterManager.Spam == true)
                    {


                        if (Input.GetButton("Q")) // Duplicate Held Object
                        {

                            GameObject boom = Instantiate(DupeEffect) as GameObject;
                            boom.transform.position = theClickedObject.transform.position;
                            GameObject dupe = Instantiate(theClickedObject.collider.gameObject) as GameObject;
                            dupe.transform.position = new Vector2(theClickedObject.transform.position.x + 1, theClickedObject.transform.position.y + 1);


                            Debug.Log("You duped the " + theClickedObject);
                        }
                    }
                    else if (Input.GetButtonDown("Q")) // Duplicate Held Object
                    {

                        GameObject boom = Instantiate(DupeEffect) as GameObject;
                        boom.transform.position = theClickedObject.transform.position;
                        GameObject dupe = Instantiate(theClickedObject.collider.gameObject) as GameObject;
                        dupe.transform.position = new Vector2(theClickedObject.transform.position.x + 1, theClickedObject.transform.position.y + 1);


                        Debug.Log("You duped the " + theClickedObject);
                    }



                    

                }





                


                else
                {
                    Debug.Log("Log");
                }
            }


            //Did we let go of the mouse button?
            if (Input.GetButtonUp("Fire1"))
            {

                //Reset the puzzlePiece to null
                theClickedObject = new RaycastHit2D();
            }
        }
    }
    private void FixedUpdate()

    {

        SkillTeleU = (CharacterManager.SkillTeleU); // "Tele" Being short for telekinesis
        SkillTeleExplodeU = (CharacterManager.SkillTeleExplodeU);
        SkillTeleDupeU = (CharacterManager.SkillTeleDupeU);
        SkillFireballU = (CharacterManager.SkillFireballU);
        SkillKameU = (CharacterManager.SkillKameU);
        SkillFlyU = (CharacterManager.SkillFlyU);
        SkillTeleportU = (CharacterManager.SkillTeleportU);
        SkillDoubleJumpU = (CharacterManager.SkillDoubleJumpU);
    }
        


}

