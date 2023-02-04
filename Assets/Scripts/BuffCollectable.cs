using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCollectable : MonoBehaviour, ICollectable
{
    public BuffTypes buffType;


    public void Collect(PlayerController player)
    {
        Destroy(gameObject);
    }
}
