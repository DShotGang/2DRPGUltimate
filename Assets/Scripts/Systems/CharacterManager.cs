using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Databox;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    private float timer = 0f;
    private float timer1 = 2f;
    private float timer2 = 0f;
    private float holdDur = 3f;

    public Text txt;
    public Text poptxt;

    public int kichargevalue = 1;
    public int kipassivechargevalue = 1;

    public bool idleaura = false;
    public bool passivechargingKi = false;
    public bool chargingKi = false;

    static public bool Spam = false;

    public string CharacterName = "Default";

    static public int Level = 1;
    static public float Form = 1f;

    static public int Experience = 0;
    static public int LevelExperienceNeeded =3;

    static public int Health = 100;
    static public int maxHealth = 100;

    static public int Ki = 100;
    static public int maxKi = 100;

    public static int Stamina = 100;
    static public int maxStamina = 100;
    public int StaminaChargeRate = 1;

    static public int TP = 0; // For training point, every stat, skill or ability, cost one training point. and other requirments


    static public float maxForm = 1f;

    static public int Strength = 1;
    static public int Wisdom = 1;
    static public int Dexterity = 1;
    static public int Luck = 1;

    static public int Defense = 0;
    static public int Damage = 5;
    static public int KiDamage = 1;



    public byte rgbr = 255;
    public byte rgbg = 255;
    public byte rgbb = 255;
    public byte rgba = 255;


    public static bool Base = true;
    public static bool SS1 = false;


    public static bool TransformationSS1Unlocked = false;
    public static bool TransformationSS1FullPowerUnlocked = false;
    public static bool TransformationSS2Unlocked = false;
    public static bool TransformationSS3Unlocked = false;




    public static bool SkillTeleU = false; // "Tele" Being short for telekinesis
    public static bool SkillTeleExplodeU = true;
    public static bool SkillTeleDupeU = true;
    public static bool SkillFireballU = false;
    public static bool SkillKameU = true;
    public static bool SkillFlyU = true;
    public static bool SkillTeleportU = true;
    public static bool SkillDoubleJumpU = true;
    public static bool ExtraSenses = false;


    public Animator CharacterAnimator;
    public Animator auraAnimator;
    public GameObject Aura;
    public SpriteRenderer AuraColor;

    public DataboxObject PlayerSave;
    public Transform Player;

    bool unlocked3d;




    // Start is called before the first frame update
    void Start()
    {
        Aura.transform.gameObject.SetActive(true);
        //bool idleaura = false;
        AuraBaseColor();
        Color32 newColor = new Color32(rgbr, rgbg, rgbb, 0);
        AuraColor.color = newColor;

        int Armour = 0;
        int Defense = Strength / 20 + Armour;

        GetData();

    }

    // Update is called once per frame
    void Update()
    {


        if (Health <= 0)
        {
            Application.Quit();
        }
        AuraScale();


        //Health Cap
        if (Health >= maxHealth) { Health = maxHealth; }
        maxHealth = Strength * 15 + 100;

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
        


        timer2 += Time.deltaTime;
        if (timer2 >= 2) // Do every second
        {
            Health = Health + Strength / 4;
            StaminaChargeRate = Strength * Dexterity / 4;
            Stamina = Stamina + StaminaChargeRate;
            timer2 = 0;
        }




        if (Base == true)
        {
            //auraAnimator.Play("AuraAnimBase");
            AuraBaseColor();
        }

        if (Input.GetButtonDown("T")) // Revert to base form
        {
            Form = 0;
            Base = true;
        }

        if (Input.GetButton("I")) // Inventory probably
        {
            Debug.Log("Fuckin button pressed : I");
        }

        if (Input.GetButtonDown("Cancel"))
        {
            AddData();
            PlayerSave.SaveDatabase();
            if (unlocked3d == true) { SceneManager.LoadScene("3d"); }
            else if (unlocked3d == false) { Application.Quit(); }
            
            
        }

        if (Input.GetButtonDown("H"))
        {
            timer = Time.time;
            

            if (Input.GetButton("H")) // Transform/ next form
            {
                if (Time.time - timer > holdDur)
                {
                    //by making it positive inf, we won't subsequently run this code by accident,
                    //since X - +inf = -inf, which is always less than holdDur
                    CharacterAnimator.Play("TransformSS1");
                    if (Level >= 5 && Level <= 9)
                    {
                        Form = Form + 0.5f;
                        Ki = Ki - 150;
                        if (Form >= maxForm) { Form = maxForm; }
                        Base = false;

                    }

                    if (Form == 0f)
                    {
                        auraAnimator.Play("AuraAnimBase");
                        AuraBaseColor();
                    }

                    if (Form == 0.5f)
                    {
                        auraAnimator.Play("AuraAnimBase");
                        rgbr = 255;
                        rgbg = 251;
                        rgbb = 0;
                    }

                }
            }
            else
            {
                timer = float.PositiveInfinity;
            }
        }




        if (Input.GetButtonUp("P"))
        {
            //LevelUp();
            
        }

        if (Input.GetButtonDown("C"))
        {
            Color32 newColor = new Color32(rgbr, rgbg, rgbb, 255);
            AuraColor.color = newColor;
            timer1 = Time.time;
        }


        if (Input.GetButtonUp("C"))
        {
            Color32 newColor = new Color32(rgbr, rgbg, rgbb, 0);
            AuraColor.color = newColor;
        }


        if (Input.GetButton("C"))
        {


            timer1 += Time.deltaTime;
            if (timer1 >= 0.5)
            {
                Ki = Ki + kichargevalue + CharacterManager.Wisdom;
                timer1 = 0;
            }


            
        }
        if (passivechargingKi == true)
        {
            Ki = Ki + kipassivechargevalue;
        }
}


   private void FixedUpdate()
    {
        

        if(Experience >= LevelExperienceNeeded)
        {
            LevelUp();
            Experience = 0;
        }


        Damage = Strength * 5;
        KiDamage = Wisdom * 5;
        maxKi = Wisdom * 5 + 10;
        

    }


    private void EndofIdle()
    {
        Health = Health + Strength / 2;
        StaminaChargeRate = 1 + Strength * Dexterity + 2;
        Stamina = Stamina + StaminaChargeRate;
    }

    void TextReset()
    {
        poptxt.text = "";
    }

    private void LevelUp()
    {
        Level = Level + 1;
        LevelExperienceNeeded = LevelExperienceNeeded + 20 * Level;
        TP = TP + 1;
        Health = maxHealth;

        

        if (Level == 10)
        {
            Debug.Log("You unlocked Super Saiyan 1");
            maxForm = 1f;
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

    public void GetData()
    {
        PlayerSave.LoadDatabase();
        IntType level = PlayerSave.GetData<IntType>("Player", "Stats", "Level");
        IntType strength = PlayerSave.GetData<IntType>("Player", "Stats", "Strength");
        IntType wisdom = PlayerSave.GetData<IntType>("Player", "Stats", "Wisdom");
        IntType dexterity = PlayerSave.GetData<IntType>("Player", "Stats", "Dexterity");
        IntType luck = PlayerSave.GetData<IntType>("Player", "Stats", "Luck");
        IntType health = PlayerSave.GetData<IntType>("Player", "Stats", "Health");
        IntType stamina = PlayerSave.GetData<IntType>("Player", "Stats", "Stamina");
        IntType ki = PlayerSave.GetData<IntType>("Player", "Stats", "Ki");

        BoolType fireballunlocked = PlayerSave.GetData<BoolType>("Player", "Unlocked", "Fireball");
        BoolType telekinesisunlocked = PlayerSave.GetData<BoolType>("Player", "Unlocked", "Telekinesis");


        Level = level.Value;
        Strength = strength.Value;
        Wisdom = wisdom.Value;
        Dexterity = dexterity.Value;
        Luck = luck.Value;
        Health = health.Value;
        Stamina = stamina.Value;
        Ki = ki.Value;
        SkillFireballU = fireballunlocked.Value;
        SkillTeleU = telekinesisunlocked.Value;



        Debug.Log(level.Value.ToString());
        Debug.Log("LOADED\nLOADED/nLOADED");
        poptxt.text = "Loaded!";
        Invoke("TextReset", 1.3f);
    }

    public void AddData()
    {
        // create a new float value

        IntType level = new IntType();
        IntType strength = new IntType();
        IntType wisdom = new IntType();
        IntType dexterity = new IntType();
        IntType luck = new IntType();
        IntType health = new IntType();
        IntType stamina = new IntType();
        IntType ki = new IntType();

        BoolType fireballunlocked = new BoolType();
        BoolType telekinesisunlocked = new BoolType();

        level.Value = Level; // Set the value that will be saved from Variable.
        strength.Value = Strength;
        wisdom.Value = Wisdom;
        dexterity.Value = Dexterity;
        luck.Value = Luck;
        health.Value = Health;
        stamina.Value = Stamina;
        ki.Value = Ki;

        fireballunlocked.Value = SkillFireballU;
        telekinesisunlocked.Value = SkillTeleU;

        PlayerSave.AddData("Player", "Stats", "Strength", strength);
        PlayerSave.AddData("Player", "Stats", "Level", level); // Save the variable and value

        PlayerSave.AddData("Player", "Stats", "Strength", strength);
        PlayerSave.AddData("Player", "Stats", "Wisdom", wisdom);
        PlayerSave.AddData("Player", "Stats", "Dexterity", dexterity);
        PlayerSave.AddData("Player", "Stats", "Luck", luck);
        PlayerSave.AddData("Player", "Stats", "Health", health);
        PlayerSave.AddData("Player", "Stats", "Stamina", stamina);
        PlayerSave.AddData("Player", "Stats", "Ki", ki);
        PlayerSave.AddData("Player", "Unlocked", "Fireball", fireballunlocked);
        PlayerSave.AddData("Player", "Stats", "Telekinesis", telekinesisunlocked);
        PlayerSave.SaveDatabase();
        Debug.Log("Saved"); // Send log that it saved
        poptxt.text = "Saved!!";
        Invoke("TextReset", 1.3f);
    }


}
