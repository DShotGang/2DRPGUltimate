using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireballSkill : MonoBehaviour
{

    public Button Button;
    public Text txt;
    public GameObject Object;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = Button.GetComponent<Button>();
        //Button btn2 = SkillTelekinesis.GetComponent<Button>();
        //Button btn3 = next.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CharacterManager.SkillFireballU == true) { Destroy(Object); }
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");

        if (CharacterManager.TP >= 1)
        {
            CharacterManager.TP = CharacterManager.TP - 1;
            CharacterManager.SkillFireballU = true;
            txt.text = "You Learned Fireball!";
            Invoke("TextReset", 2f);
            Destroy(Object);
        }
        else
        {
            txt.text = "Not enough TP! Cost 1 you have " + CharacterManager.TP;
            Invoke("TextReset", 2f);
        }
    }
    void TextReset()
    {
        txt.text = "";
    }
}
