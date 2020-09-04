using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    public int width;
    public int height;
    public int distance;
    public int space;

    public GameObject Grass;
    public GameObject Dirt;
    public GameObject Stone;

    public float heightpoint;
    public float heightpoint2;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetButtonDown("L"))
        {
            Generation();
        }
    }


    void Generation()
    {
        distance = height;
        for (int w= 0; w <width; w++)
        {
            int lowernum = distance - 1;
            int highernum = distance + 2;
            distance = Random.Range(lowernum, highernum);
            space = Random.Range(12, 20);
            int stonespace = distance - space;

            for (int j = 0; j < stonespace; j++)
            {
                Instantiate(Stone, new Vector3(w, j), Quaternion.identity);
            }
            for (int j = stonespace; j < distance; j++)
            {
                Instantiate(Dirt, new Vector3(w, j), Quaternion.identity);
            }
            Instantiate(Grass, new Vector3(w, distance), Quaternion.identity);
           

        }
    }


}
