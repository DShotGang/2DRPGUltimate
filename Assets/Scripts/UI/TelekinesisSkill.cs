using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelekinesisSkill : MonoBehaviour
{
    public Button Button;
    public GameObject Object;
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
            Destroy(Object);
        }
        else if(CharacterManager.TP <= 0)
        {
            txt.text = "Not enough TP for Telekinesis! Cost 1 you have " + CharacterManager.TP;
            Invoke("TextReset", 2f);
        }
        else if(CharacterManager.SkillTeleU == true)
        {
            txt.text = "Already unlocked!";
        }
    }

    void TextReset()
    {
        txt.text = "";
    }

}
