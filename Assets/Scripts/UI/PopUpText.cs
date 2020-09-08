using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{

    public string stringname;
    public Text PopUpTexttxt;
    // Start is called before the first frame update
    void Start()
    {
        stringname = "testttt";
    }

    // Update is called once per frame
    void Update()
    {
        PopUpTexttxt.text = stringname;
    }

    public void TextReset()
    {

        PopUpTexttxt.text = " ";
        Debug.Log("POPUPTEXT");
    }
}
