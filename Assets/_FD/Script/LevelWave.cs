﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWave : MonoBehaviour
{
    public enum LevelType
    {
        Endless,
        Normal
    }

    [Space(3)] 
    public EndlessWaveGenerator.EndlessEnemyWave[] enemiesList;
    public LevelType type;
    public int defaultExp = 200;
    public EnemyWave[] Waves;
    public float increaseEnemySpeedDifficultyRate = 0.05f;
    public float increaseEnemyAttackDifficultyRate = 0.1f;
    public float increaseEnemyHealthDifficultyRate = 0.1f;
    public float increaseEnemyAmountDifficultyRate = 0.4f;
    public float increaseEnemyWaitDifficultyRate = 0.2f;
    public float initialWaitAmount = 3;
    
    [Space(2)]
    [Header("Other settings")]
    public int level = 1;
    public Sprite backgroundSprite;

}