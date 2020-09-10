using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraStats : MonoBehaviour
{


    public Text txt;
    int ki;
    int health;
    string stringname;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Expanded Stats\n\n" +
            "Experience needed to Level:  " + CharacterManager.LevelExperienceNeeded +
            "\nDamage: " + CharacterManager.Damage +
            "\n Projectile Damage: " + CharacterManager.KiDamage +
            "\nTraining Points  " + CharacterManager.TP;
    }
}
