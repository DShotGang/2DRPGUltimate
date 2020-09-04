using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float timeToChange = 0.1f;
    private float timeSinceChange = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceChange += Time.deltaTime;
        
        //Make sure the sprite renderer component is there
        if(spriteRenderer != null && timeSinceChange >= timeToChange)
        {
            Color newColor = new Color(
                    Random.value,
                    Random.value,
                    Random.value
                );

            spriteRenderer.color = newColor;
            timeSinceChange = 0f;
        }
    }
}
