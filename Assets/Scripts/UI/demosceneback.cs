using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class demosceneback : MonoBehaviour
{
    private Button Button;

    void Start()
    {
        Button Button = GetComponent<Button>();
        Button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Basic");
    }


}
