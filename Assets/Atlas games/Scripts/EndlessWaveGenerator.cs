using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessWaveGenerator : LevelEnemyManager, IListener
{
    [HideInInspector]public EnemyWave wave;
    [HideInInspector] public List<EnemySpawn> _enemySpawn;
    public GameObject[] enemies;
    public float increaseDifficultyRate = 0.4f;
    private float _currentDifficultyRate = 0f;
    [HideInInspector]public float[] _enemyCounts;
    public float InitialWaitAmount = 3;
    [HideInInspector] public int _waveCount;
 
    int totalEnemy, currentSpawn;

    void Start()
    {
        _enemyCounts = new float[enemies.Length];
        _enemyCounts[0] = 2;
        _enemyCounts[1] = 1;
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
            newSpawn.wait = 1 / (_currentDifficultyRate + increaseDifficultyRate);
            newSpawn.customHealth =
                Convert.ToInt32(Mathf.Round(enemies[i].GetComponent<SmartEnemyGrounded>().health *
                                            _currentDifficultyRate));
            newSpawn.customSpeed =
                Convert.ToInt32(Mathf.Round(enemies[i].GetComponent<SmartEnemyGrounded>().walkSpeed *
                                            _currentDifficultyRate));


         
             var rangeAttack = enemies[i].GetComponent<EnemyRangeAttack>();
             if (rangeAttack)
             {
                 newSpawn.customAttackDmg += _currentDifficultyRate * rangeAttack.damage;
               
             }
             var meleeAttack = enemies[i].GetComponent<EnemyMeleeAttack>();
             if (meleeAttack)
             {
                 newSpawn.customAttackDmg += _currentDifficultyRate *  meleeAttack.dealDamage;
             }
             var throwAttack = enemies[i].GetComponent<EnemyThrowAttack>();
             if (throwAttack)
             {
                 newSpawn.customAttackDmg += _currentDifficultyRate *    throwAttack.damage;
             }

            


            _enemySpawn.Add(newSpawn);
        }

        // set wave data ready for usage in LevelEnemyManager.cs for creation
        _waveCount++;
        _currentDifficultyRate = _waveCount * increaseDifficultyRate;
        wave.wait = InitialWaitAmount;
        wave.enemySpawns = _enemySpawn.ToArray();
    }

    public new void IPlay()
    {
        StartCoroutine(SpawnEnemyCo());
        //throw new System.NotImplementedException();
    }

    IEnumerator SpawnEnemyCo()
    {
        GenerateWave();
        EnemyWaves = new EnemyWave[1];
        EnemyWaves[0] = wave;


        for (int i = 0; i < EnemyWaves.Length; i++)
        {
            yield return new WaitForSeconds(EnemyWaves[i].wait);
            for (int j = 0; j < EnemyWaves[i].enemySpawns.Length; j++)
            {
                var enemySpawn = EnemyWaves[i].enemySpawns[j];
                yield return new WaitForSeconds(enemySpawn.wait);
                for (int k = 0; k < enemySpawn.numberEnemy; k++)
                {
                    GameObject _temp = Instantiate(enemySpawn.enemy,
                        (Vector2)spawnPositions[Random.Range(0, spawnPositions.Length)].position,
                        Quaternion.identity) as GameObject;
                    var isEnemy = (Enemy)_temp.GetComponent(typeof(Enemy));
                    if (isEnemy != null)
                    {
                        isEnemy.disableFX = FX_Smoke;
                        if (enemySpawn.customHealth > 0)
                            isEnemy.health = enemySpawn.customHealth;
                        if (enemySpawn.customSpeed > 0)
                            isEnemy.walkSpeed = enemySpawn.customSpeed;
                        if (enemySpawn.customAttackDmg > 0)
                        {
                            var rangeAttack = _temp.GetComponent<EnemyRangeAttack>();
                            if (rangeAttack)
                            {
                                rangeAttack.damage = enemySpawn.customAttackDmg;
                            }

                            var meleeAttack = _temp.GetComponent<EnemyMeleeAttack>();
                            if (meleeAttack)
                                meleeAttack.dealDamage = enemySpawn.customAttackDmg;
                            var throwAttack = _temp.GetComponent<EnemyThrowAttack>();
                            if (throwAttack)
                            {
                                throwAttack.damage = enemySpawn.customAttackDmg;
                            }
                        }

                        var rangeAttack1 = _temp.GetComponent<EnemyRangeAttack>();
                        if (rangeAttack1)
                            rangeAttack1.bullet = bullet;
                        var meleeAttack1 = _temp.GetComponent<EnemyMeleeAttack>();
                        var throwAttack1 = _temp.GetComponent<EnemyThrowAttack>();
                        if (throwAttack1)
                        {
                            throwAttack1.FX_Blow = FX_Blow;
                            throwAttack1.FX_Smoke = FX_Smoke;
                        }

                        if (enemySpawn.boosType != EnemySpawn.isBoss.NONE)
                        {
                            bossManeger.enemy = _temp.GetComponent<Enemy>();
                            if (enemySpawn.BossScale > 1)
                                bossManeger.enemy.gameObject.transform.localScale =
                                    new Vector2(enemySpawn.BossScale, enemySpawn.BossScale);
                            bossManeger.bossType = enemySpawn.boosType;
                            bossManeger.enemy.gameObject.GetComponent<GiveExpWhenDie>().expMin =
                                enemySpawn.BossMinExp;
                            bossManeger.enemy.gameObject.GetComponent<GiveExpWhenDie>().expMax =
                                enemySpawn.BossMaxExp;
                            bossManeger.gameObject.SetActive(true);
                            bossManeger.enemy.is_boss = true;
                            AudioClip bossMusic = bossManeger.enemy.BossMusic != null
                                ? bossManeger.enemy.BossMusic
                                : SoundManager.Instance.BossMusicClip;
                            SoundManager.PlayMusic(bossMusic, 0.5f);
                        }
                    }

                    _temp.SetActive(false);
                    _temp.transform.parent = transform;

                    yield return new WaitForSeconds(0.1f);
                    _temp.SetActive(true);
                    //_temp.transform.localPosition = Vector2.zero;
                    listEnemySpawned.Add(_temp);

                    currentSpawn++;
                    MenuManager.Instance.UpdateEnemyWavePercent(currentSpawn, totalEnemy);

                    yield return new WaitForSeconds(enemySpawn.rate);
                }
            }

            while (IsEnemyAlive())
            {
                yield return new WaitForSeconds(0.1f);
            }

           
                foreach(GameObject enemy in listEnemySpawned)
                {
                    Destroy(enemy);
                }
                listEnemySpawned.Clear();
                GenerateWave();
                EnemyWaves = new EnemyWave[1];
                EnemyWaves[0] = wave;
                StartCoroutine(SpawnEnemyCo());
          
        }
    }

    bool IsEnemyAlive()
    {
        for (int i = 0; i < listEnemySpawned.Count; i++)
        {
            if (listEnemySpawned[i].gameObject != null && listEnemySpawned[i].activeInHierarchy)
                return true;
        }

        return false;
    }
}