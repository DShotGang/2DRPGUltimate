using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{


    public Text txt;
    public Button Button;

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
        CharacterManager.Spam = true;
        txt.text = "Spam unlocked!";
        Invoke("TextReset", 2f);
    }

    void TextReset()
    {
        txt.text = "";
    }
}
