using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiscButton : MonoBehaviour
{
    private Button Button;
    // Start is called before the first frame update
    void Start()
    {
        Button Button = GetComponent<Button>();
        Button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("demoscene123");
    }
}
