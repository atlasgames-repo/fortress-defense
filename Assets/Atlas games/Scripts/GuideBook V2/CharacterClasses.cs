using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class SPEED
{
    public static readonly string SLOW = "Slow";
    public static readonly string NORMAL = "Normal";
    public static readonly string FAST = "Fast";
    public static string get(SPEED_ENUM value)
    {
        switch (value)
        {
            case SPEED_ENUM.SLOW:
                return SLOW;
            case SPEED_ENUM.NORMAL:
                return NORMAL;
            case SPEED_ENUM.FAST:
                return FAST;
            default:
                return SLOW;
        }
    }
}
public enum SPEED_ENUM
{
    SLOW,
    NORMAL,
    FAST,
}
[Serializable]
public class EFFECTS
{
    public static readonly string NONE = "None";
    public static readonly string LIGHTNING = "Lightning";
    public static readonly string FROZED = "Frozed";
    public static readonly string POISON = "Poison";
    public static readonly string MAGNET = "Magnet";
    public static readonly string CURE = "Cure";
    public static readonly string FIRE = "Fire";
    public static readonly string DARK = "Dark";
    public static readonly string AERO = "Aero";
    public static readonly string LIGHTNING_ALL = "LightningAll";
    public static readonly string ARMAGDON = "Armagdon";
    public static readonly string DEFENSE_WALL = "DefenseWall";
    public static string get(EFFECTS_ENUM value)
    {
        switch (value)
        {
            case EFFECTS_ENUM.NONE:
                return NONE;
            case EFFECTS_ENUM.LIGHTNING:
                return LIGHTNING;
            case EFFECTS_ENUM.FROZED:
                return FROZED;
            case EFFECTS_ENUM.POISON:
                return POISON;
            case EFFECTS_ENUM.MAGNET:
                return MAGNET;
            case EFFECTS_ENUM.CURE:
                return CURE;
            case EFFECTS_ENUM.DARK:
                return DARK;
            case EFFECTS_ENUM.AERO:
                return AERO;
            case EFFECTS_ENUM.LIGHTNING_ALL:
                return LIGHTNING_ALL;
            case EFFECTS_ENUM.ARMAGDON:
                return ARMAGDON;
            case EFFECTS_ENUM.DEFENSE_WALL:
                return DEFENSE_WALL;
            default:
                return NONE;
        }
    }
}
public enum EFFECTS_ENUM
{
    NONE,
    LIGHTNING,
    FROZED,
    POISON,
    MAGNET,
    CURE,
    FIRE,
    DARK,
    AERO,
    LIGHTNING_ALL,
    ARMAGDON,
    DEFENSE_WALL,
}
[Serializable]
public class POWER
{
    public static readonly string WEAK = "Weak";
    public static readonly string STRONG = "Strong";
    public static readonly string VERY_STRONG = "Very strong";
    public static string get(POWER_ENUM value)
    {
        switch (value)
        {
            case POWER_ENUM.WEAK:
                return WEAK;
            case POWER_ENUM.STRONG:
                return STRONG;
            case POWER_ENUM.VERY_STRONG:
                return VERY_STRONG;
            default:
                return WEAK;
        }
    }
}
public enum POWER_ENUM
{
    WEAK,
    STRONG,
    VERY_STRONG,
}
[Serializable]
public class CHARACTER_TYPE
{
    public static readonly string FRIEND = "Friend";
    public static readonly string ENEMY = "Enemy";
    public static string get(CHARACTER_TYPE_ENUM value)
    {
        switch (value)
        {
            case CHARACTER_TYPE_ENUM.FRIEND:
                return FRIEND;
            case CHARACTER_TYPE_ENUM.ENEMY:
                return ENEMY;
            default:
                return FRIEND;
        }
    }
}
[Serializable]
public enum CHARACTER_TYPE_ENUM
{
    ENEMY,
    FRIEND
}
[CreateAssetMenu(fileName = "CharacterClasses", menuName = "Scriptable Objects/CharacterClasses")]
public class CharacterClasses : ScriptableObject
{

    [Serializable]
    public class Info
    {
        public string Name;
        public int index;
        public SPEED_ENUM Speed;
        public int Damage;
        public POWER_ENUM Power;
        public CHARACTER_TYPE_ENUM Type;
        public EFFECTS_ENUM Weakness, Strength;
        public Sprite EnemyProfile;
        public int levelUnlocked;
    }

    public List<Info> List;
}
