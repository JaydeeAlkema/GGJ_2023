using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffsScriptableObject", menuName = "ScriptableObjects/New Buffs SciptableObject")]
public class BuffsScriptableObject : ScriptableObject
{
    public float boostedMovementSpeed;
    public int boostedDamage;
    public int boostedKnockback;

    public int moveSpeedBuffDuration;
    public int damageBuffDuration;
    public int boostedKnockbackDuration;
}
