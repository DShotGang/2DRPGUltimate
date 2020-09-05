using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
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
        txt.text = "Attributes\n" +
    "Strength:  " + CharacterManager.Strength + 
    "\nWisdom: " + CharacterManager.Wisdom +
    "\nDexterity: " + CharacterManager.Dexterity + 
    "\nLuck: " + CharacterManager.Luck;
    }
}
