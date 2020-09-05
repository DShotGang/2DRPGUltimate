using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextUpdate : MonoBehaviour
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
    void FixedUpdate()
    {

        int ki = CharacterManager.Ki;
        int health = CharacterManager.Health;

        txt.text = "Stats\n" +
            "Level " + CharacterManager.Level +
            "\nBP " + CharacterManager.BattlePower +
            "\nHealth " + CharacterManager.Health +
            "\nKi " + CharacterManager.Ki +
            "\nStamina " + CharacterManager.Stamina + 
            "\nForm " + CharacterManager.Form +
            "\nXP " + CharacterManager.Experience;
    }
}
