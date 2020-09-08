using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiButton : MonoBehaviour
{
    public Button AbilityFireball;
    public Text txt;

    void Start()
    {
        Button btn = AbilityFireball.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

    }

    private void Update()
    {
        
    }
    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }

}
