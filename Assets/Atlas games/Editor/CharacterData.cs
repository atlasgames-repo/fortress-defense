using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public string enemyName = "Character";

    public SmartEnemyGroundedProp smartEnemyGrounded;
    public ATTACKTYPE attakType = ATTACKTYPE.MELEE;
    public EnemyMeleeAttackProp enemyMeleeAttack;
    public EnemyRangeAttackProp enemyRangeAttack;
    public EnemyThrowAttackProp enemyThrowAttack;
    public int expMin, expMax;
}
[System.Serializable]
public class SmartEnemyGroundedProp
{
    [Header("Setup")]
    public bool useGravity = false;
    public float gravity = 35f;
    public float walkSpeed = 3;
    [Header("Behavier")]
    public ATTACKTYPE attackType;
    public STARTBEHAVIOR startBehavior = STARTBEHAVIOR.WALK_LEFT;
    public float spawnDelay = 1;
    [Header("HEALTH")]
    [Range(0, 5000)]
    public int health = 100;
}
[System.Serializable]
public class EnemyMeleeAttackProp
{
    public LayerMask targetLayer;
    public float radiusCheck = 1;
    public float dealDamage = 20;
    [Range(1, 100)]
    public int criticalPercent = 10;
    public float meleeRate = 1;
}
[System.Serializable]
public class EnemyRangeAttackProp
{
    [Header("AIM TARGET")]

    public bool aimTarget = false;
    public Vector2 aimTargetOffset = new Vector2(0, 0.5f);
    public float damage = 30;
    public float detectDistance = 5;

}
[System.Serializable]
public class EnemyThrowAttackProp
{
    [Header("Grenade")]
    public LayerMask targetPlayer;
    public float angleThrow = 60;       //the angle to throw the bomb
    public float throwForceMin = 290;		//how strong?
    public float throwForceMax = 320;
    public float addTorque = 100;
    public float throwRate = 0.5f;
    public float damage = 30;
    public float radiusDetectPlayer = 5;
    public bool onlyAttackTheFortrest = true;
}