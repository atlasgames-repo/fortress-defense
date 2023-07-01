using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessWaveGenerator : MonoBehaviour
{
    [HideInInspector]public EnemyWave wave;
    [HideInInspector] public List<EnemySpawn> _enemySpawn;
    public GameObject[] enemies;
    public float increaseDifficultyRate = 0.4f;
    private float _currentDifficultyRate = 0f;
    float[] _enemyCounts;
    public float InitialWaitAmount = 3;
    [HideInInspector] public int _waveCount;

    void Start()
    {
        _enemyCounts = new float[enemies.Length];
    }
    // generate a new wave harder than last
    public void GenerateWave()
    {
        _enemySpawn.Clear();
        
        // check if ran out of new enemies 
        int enemiesInUse;
        if (_currentDifficultyRate + 1 < enemies.Length)
        {
            enemiesInUse = Convert.ToInt32(Mathf.Round(_currentDifficultyRate + 1));
        }
        else
        {
            enemiesInUse = enemies.Length;
        }

        // add new enemies to wave spwan 
        // add to the health, speed, and attack damage of enemies 
        
        for (int i = 0; i < enemiesInUse; i++)
        {

            EnemySpawn newSpawn = new EnemySpawn();
            newSpawn.enemy = enemies[i];
            _enemyCounts[i] += increaseDifficultyRate;
            newSpawn.numberEnemy = Convert.ToInt32(Mathf.Round(_enemyCounts[i]));
            newSpawn.wait = 1 / _currentDifficultyRate;
            newSpawn.customHealth = Convert.ToInt32(Mathf.Round(enemies[i].GetComponent<SmartEnemyGrounded>().health * _currentDifficultyRate));
            newSpawn.customSpeed = Convert.ToInt32(Mathf.Round(enemies[i].GetComponent<SmartEnemyGrounded>().walkSpeed * _currentDifficultyRate));

            #region SettingAttackDamage
            
            //this code currently doesn't work 
            // var rangeAttack = enemies[i].GetComponent<EnemyRangeAttack>();
            // if (rangeAttack)
            // {
            //     newSpawn.customAttackDmg =  rangeAttack.damage;
            //   
            // }
            // var meleeAttack = enemies[i].GetComponent<EnemyMeleeAttack>();
            // if (meleeAttack)
            // {
            //     newSpawn.customAttackDmg =   meleeAttack.dealDamage;
            // }
            // var throwAttack = enemies[i].GetComponent<EnemyThrowAttack>();
            // if (throwAttack)
            // {
            //     newSpawn.customAttackDmg =     throwAttack.damage;
            // }
            //            newSpawn.customAttackDmg = Convert.ToInt32(Mathf.Round(newSpawn.customAttackDmg * _currentDifficultyRate));
            #endregion


            _enemySpawn.Add(newSpawn);
        }

        // set wave data ready for usage in LevelEnemyManager.cs for creation
        _waveCount++;
        _currentDifficultyRate = _waveCount * increaseDifficultyRate;
        wave.wait = InitialWaitAmount;
        wave.enemySpawns = _enemySpawn.ToArray();
    }
    
}