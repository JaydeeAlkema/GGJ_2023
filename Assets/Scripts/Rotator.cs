using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rpm = 120f;

    void Update()
    {
        float degreesPerSecond = rpm * 360f / 60f;
        transform.Rotate(Vector3.forward, degreesPerSecond * Time.deltaTime);
    }
}
