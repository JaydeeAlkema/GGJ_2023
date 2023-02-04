using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public int duration;
    public BuffTypes type;

    public Buff(BuffTypes type, int duration)
    {
        this.duration = duration;
        this.type = type;
    }
}
