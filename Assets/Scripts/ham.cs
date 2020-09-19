using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ham : MonoBehaviour
{
    private Material mat;
    private Material matreplace;
    public Texture texture;
    GameObject contact;
    public Material mat1;
    // Start is called before the first frame update
    void Start()
    {
        Material mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collider)
    {


        if (collider.transform.tag == "Destroyable" || collider.transform.tag == "Floor" || collider.transform.tag == "Item" || collider.transform.tag == "Enemy" || collider.transform.tag == "Player")
        {

            

            GameObject contact = collider.gameObject;
            Material replace = contact.GetComponent<Renderer>().material;
            contact.GetComponent<Renderer>().material = mat;
            Debug.Log("enter");
        }

    }
    private void OnCollisionExit2D(Collision2D collider) { Debug.Log("exit"); }

}
