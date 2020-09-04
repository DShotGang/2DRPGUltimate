using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AICharacterManager : MonoBehaviour
{
    private float timer = 0f;
    private float timer1 = 0f;
    private float timer2 = 0f;
    private float holdDur = 3f;

    public Text txt;

    public int kichargevalue = 1;
    public int kipassivechargevalue = 1;
    public bool idleaura = false;
    public bool passivechargingKi = false;
    public bool chargingKi = false;



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


    public byte rgbr = 255;
    public byte rgbg = 255;
    public byte rgbb = 255;
    public byte rgba = 255;


    public static bool Base = true;
    public static bool SS1 = false;
  
    public bool SS1Unlocked = false;
    public bool SS1AUnlocked = false;



    public Animator CharacterAnimator;
    public Animator auraAnimator;
    public GameObject Aura;
    public SpriteRenderer AuraColor;

    private Transform AIstartingPosition;
    private Transform AITarget;


    private Rigidbody2D player;
    public float runSpeed = 2f;
    float timeout = 0.3f;

    public static Vector2 gravity;
    public Camera Camera;

    public float jumpForce = 400f;
    bool jump = false;
    bool fly = false;
    public bool Grounded = true;
    public float gravityScale;
    static public int projectilechoice = 1;

    enum GravityDirection { Down, Left, Up, Right };
    GravityDirection m_GravityDirection;

    Vector2 reachedPositionDistance;


    private void Awake()
    {
        
    }

    Vector2 AiTarget2; 

    // Start is called before the first frame update
    void Start()
    {
        Aura.transform.gameObject.SetActive(true);
        //bool idleaura = false;
        AuraBaseColor();
        Color32 newColor = new Color32(rgbr, rgbg, rgbb, 0);
        AuraColor.color = newColor;

        AITarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        AiTarget2 = new Vector2(AITarget.position.x, AITarget.position.y);

    }

    // Update is called once per frame
    void Update()
    {





        AuraScale();


        //Health Cap
        if (Health >= maxHealth) { Health = maxHealth; }

        //Ki Cap
        if (Ki >= maxKi) { Ki = maxKi; }

        if (idleaura == true)
        {
            Color32 newColor = new Color32(rgbr, rgbg, rgbb, 255);
            AuraColor.color = newColor;
            passivechargingKi = true;
        }

        if (Ki <= 0) { Ki = 0; }



        //Stamina Cap
        if (Stamina >= maxStamina) { Stamina = maxStamina; }
        Stamina = Stamina + StaminaChargeRate;


        if (Input.GetButtonDown("T"))
        {
            Form = 0;
            Base = true;
        }




        if (Input.GetButtonDown("H"))
        {
            timer = Time.time;
            timer1 = Time.time;
        }
        if (Input.GetButton("H"))
        {
            if (Time.time - timer > holdDur)
            {
                //by making it positive inf, we won't subsequently run this code by accident,
                //since X - +inf = -inf, which is always less than holdDur
                CharacterAnimator.Play("TransformSS1");
                if (Level >= 5 && Level <= 9)
                {
                    Form = Form + 1f;
                    Ki = Ki - 150;
                    if (Form >= maxForm) { Form = maxForm; }
                    Base = false;
                    timer1 = float.PositiveInfinity;

                }


                if (Form == 0f)
                {
                    auraAnimator.Play("AuraAnimBase");
                    AuraBaseColor();
                }


                if (Form == 1f)
                {

                    Color32 newColor = new Color32(rgbr, rgbg, rgbb, 255);
                }
            }
        }
        else
        {
            timer = float.PositiveInfinity;
        }


    }


    private void FixedUpdate()
    {
        Stamina = Stamina + StaminaChargeRate;

        if (Vector2.Distance(transform.position, AiTarget2) >= 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, AITarget.position, runSpeed * Time.deltaTime);
        }
    }




    private void AuraBaseColor()
    {
        rgbr = 255;
        rgbg = 255;
        rgbb = 255;
    }

    private void AuraScale()
    {

        float MaxAura = (Level / 40f + 1f);

        Aura.transform.localScale = new Vector3(MaxAura, MaxAura, 1);

    }


}
