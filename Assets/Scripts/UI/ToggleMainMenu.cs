using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToggleMainMenu : MonoBehaviour
{
    Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            myCanvas.enabled = !myCanvas.enabled;
        }
    }
}