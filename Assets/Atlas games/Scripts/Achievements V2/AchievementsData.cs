using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement Data", menuName = "Scriptable/AchievementData")]
public class AchievementsData : ScriptableObject
{
    public AchievementModel[] models;
}
