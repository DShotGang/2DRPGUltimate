using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WisdomButton : MonoBehaviour
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
        txt.text = "Welcome!";
        Invoke("TextReset", 1.7f);
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
            CharacterManager.Wisdom = CharacterManager.Wisdom + 1;
            txt.text = "Trained 1 Wisdom!";
            Invoke("TextReset", 2f);
        }
        else
        {
            txt.text = "Not enough TP for +1 Wisdom! Cost 1 you have " + CharacterManager.TP;
            Invoke("TextReset", 2f);
        }
    }

    void TextReset()
    {
        txt.text = "";
    }
}
