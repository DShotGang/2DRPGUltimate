using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthButton : MonoBehaviour
{

    public Text txt;
    public Button Button;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = Button.GetComponent<Button>();
        //Button btn2 = SkillTelekinesis.GetComponent<Button>();
        //Button btn3 = next.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");

        if (CharacterManager.TP >= 1)
        {
            CharacterManager.TP = CharacterManager.TP - 1;
            CharacterManager.Strength = CharacterManager.Strength + 1;
            txt.text = "Trained 1 Strength!";
            Invoke("TextReset", 2f);
        }
        else
        {
            txt.text = "Not enough TP for +1 Strength! Cost 1 you have " + CharacterManager.TP;
            Invoke("TextReset", 2f);
        }
    }

    void TextReset()
    {
        txt.text = "";
    }
}
