using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelekinesisSkill : MonoBehaviour
{
    public Button Button;
    public Text txt; 
    // Start is called before the first frame update
    void Start()
    {
        Button btn = Button.GetComponent<Button>();
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
            CharacterManager.SkillTeleU = true;
            txt.text = "You learned Telekinesis!";
            Invoke("TextReset", 2.0f);
        }
        else
        {
            txt.text = "Not enough TP for Telekinesis! Cost 1 you have " + CharacterManager.TP;
            Invoke("TextReset", 2f);
        }
    }

    void TextReset()
    {
        txt.text = "";
    }

}
