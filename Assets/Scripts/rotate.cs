using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour
{
    public float ySpeed = 250.0f;

    public float yMinLimit = -80f;
    public float yMaxLimit = 80f;

    private float x = 0.0f;
    private float y = 0.0f;
    private float z = 0.0f;

    void Start()
    {
        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;
        z = transform.eulerAngles.z;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            x += ySpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            x -= ySpeed * Time.deltaTime;
        }

        x = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion newRot = Quaternion.Euler(x, y, z);

        transform.rotation = newRot;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}