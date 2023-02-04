using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ScriptableInt", menuName = "ScriptableObjects/New Scriptable Int")]
public class ScriptableInt : ScriptableObject
{
    public int startValue;
    public int currentValue;
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
