﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelSetup : MonoBehaviour
{
    public static GameLevelSetup Instance;
    public List<LevelWave> levelWaves = new List<LevelWave>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public LevelWave.LevelType type()
    {
        LevelWave.LevelType levelType = LevelWave.LevelType.Normal;
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
            {
                levelType=  obj.type;
            }
        }

        return levelType;
    }
    public EndlessWaveGenerator.EndlessEnemyWave[] EndlessInitialWave()
    {
        EndlessWaveGenerator.EndlessEnemyWave[] wave = new EndlessWaveGenerator.EndlessEnemyWave[0];     
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
            {
                wave =  obj.enemiesList;
            }
        }

        return wave;
    }


    public float IncreaseEnemySpeedDifficultyRate()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
            
            num = obj.increaseEnemySpeedDifficultyRate;
        }
        return num;
    }
    public float IncreaseEnemyAttackDifficultyRate()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
        num = obj.increaseEnemyAttackDifficultyRate;
        }
        return num;
    }
    public float IncreaseEnemyHealthDifficultyRate()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
        num = obj.increaseEnemyHealthDifficultyRate;
        }
        return num;
    }
    public float IncreaseEnemyAmountDifficultyRate()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
        num = obj.increaseEnemyAmountDifficultyRate;
        }
        return num;
    }
    public float IncreaseEnemyWaitDifficultyRate()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
        num = obj.increaseEnemyWaitDifficultyRate;
        }
        return num;
    }
    public float InitialWaitAmount()
    {
    float num= 0;
        foreach (var obj in levelWaves)
        {
        num = obj.initialWaitAmount;
        }
        return num;
    }
    public EnemyWave[] GetLevelWave()
    {
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.Waves;
        }

        return null;
    }

    public int GetCurrentLevelExp()
    {
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.defaultExp;
        }

        return 9999;
    }

    public Sprite GetBackground()
    {
        foreach (var obj in levelWaves)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj.backgroundSprite;
        }

        return null;
    }

    public int getTotalLevels()
    {
        return levelWaves.Count;
    }

    public bool isFinalLevel()
    {
        return GlobalValue.levelPlaying == levelWaves.Count;
    }

    private void OnDrawGizmos()
    {
        // if (levelWaves.Count != transform.childCount)
        // {
        //     var waves = transform.GetComponentsInChildren<LevelWave>();
        //     levelWaves = new List<LevelWave>(waves);

        //     for (int i = 0; i < levelWaves.Count; i++)
        //     {
        //         levelWaves[i].level = i + 1;
        //         levelWaves[i].gameObject.name = "Level " + levelWaves[i].level;
        //     }
        // }
    }
}
