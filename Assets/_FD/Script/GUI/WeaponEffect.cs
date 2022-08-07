/*
 * Use for weapon
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NumberArrow { Single, Double }
public enum WEAPON_EFFECT { NONE, POISON, FREEZE, LIGHTING }

[System.Serializable]
public class WeaponEffect
{
    public WEAPON_EFFECT effectType = WEAPON_EFFECT.NONE;
    [Header("NORMAL")]
    public int normalDamageMin = 30;
    public int normalDamageMax = 50;

    [Header("POISON")]
    [Range(0f, 1f)]
    public float poisonChance = 0.1f;
    public float poisonTime = 5;
    public float poisonDamagePerSec = 80;
    [Space]
    [Header("FREEZE")]
    [Range(0f, 1f)]
    public float freezeChance = 0.1f;
    public float freezeTime = 5;
}
