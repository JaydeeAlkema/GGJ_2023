using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffsIconScriptableObject", menuName = "ScriptableObjects/New Buffs Icons SciptableObject")]
public class BuffIconsScriptableObject : ScriptableObject
{
    public Sprite speedSprite;
    public Sprite damageSprite;
    public Sprite knockbackSprite;
}
