using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : MonoBehaviour
{

    public Text txt;

    int kichargevalue = 1;
    int kipassivechargevalue = 1;
    bool idleaura = false;
    bool passivechargingKi = false;
    bool chargingKi = false;



    string CharacterName = "AISkeleton";
    static public int Level = 1;
    static public int BattlePower = 100;
    static public float Form = 1f;
    static public int Health = 100;
    public int maxHealth = 100;
    static public int Ki = 100;
    public int maxKi = 100;
    public static int Stamina = 100;
    public int maxStamina = 100;
    public int StaminaChargeRate = 1;
    static public int xp = 0;
    static public float maxForm = 1f;


    byte rgbr = 255;
    byte rgbg = 255;
    byte rgbb = 255;
    byte rgba = 255;


    public static bool Base = true;
    static bool SS1 = false;
  
    bool SS1Unlocked = false;
    bool SS1AUnlocked = false;

    private Transform AIstartingPosition;
    private Transform AITarget;





    public int damage = 2;

    public float speed = 4;
    public float stoppingDistance = 4;

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



    private void FixedUpdate()
    {
        Stamina = Stamina + StaminaChargeRate; // Charge Stamina constant
        if (Alive == true)
        {


            if (Vector2.Distance(transform.position, target.position) > stoppingDistance) // If Skeleton's distance to target is more than stopping distance, Move and play anim
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); // Move

            }

            if (Vector2.Distance(transform.position, target.position) <= stoppingDistance) // if equals to or less than stopping distance
            {
                anim.Play("SkeletonAttackA1"); // Play animation
            }

            //Stamina Cap
            if (Stamina >= maxStamina) { Stamina = maxStamina; }
            Stamina = Stamina + StaminaChargeRate;

            if (Health <= 0)
            {
                Alive = false;
            }

        }
        else if (Alive == false)
        {
            anim.Play("SkeletonDead");
            deathEvent();
            // Destroy(skele, 3.4f); // Lame way of destroying after an animation, the right way is to use animation triggers i think, do it later.
        }

    }



    // Update is called once per frame
    void Update()
    {


        
    

       
    }


    // FUNCTIONS

    public void deathEvent() // New way of destroying after an animation, This is way better. Using animation events.
    {
        Destroy(skele);
    }
}
