using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Aura : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float timeToChange = 0.1f;
    private float timeSinceChange = 0f;




    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceChange += Time.deltaTime;
        
            Color32 newColor = new Color32(100,100,100, 150);

            spriteRenderer.color = newColor;
            timeSinceChange = 0f;
        
    }
}
