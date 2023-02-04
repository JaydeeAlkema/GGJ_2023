using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableFloat", menuName = "ScriptableObjects/New Scriptable Float")]
public class ScriptableFloat : ScriptableObject
{
    public float startValue;
    public float currentValue;
    public bool reset;

    public void OnEnable()
    {
        currentValue = startValue;
    }

    public void OnDisable()
    {
        currentValue = startValue;
    }

}
