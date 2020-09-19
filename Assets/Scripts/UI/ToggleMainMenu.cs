using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleMainMenu : MonoBehaviour
{
    Canvas myCanvas;
    private Button Button;
    // Start is called before the first frame update
    void Start()
    {
        Button Button = GetComponent<Button>();
        Button.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        myCanvas.enabled = !myCanvas.enabled;
    }
}