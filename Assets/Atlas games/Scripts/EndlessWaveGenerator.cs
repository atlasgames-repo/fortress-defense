using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EndlessWaveGenerator : LevelEnemyManager, IListener
{
    [Serializable]
    public class EndlessEnemyWave
    {
        public GameObject enemyObject;
        public int initialCount;
    }

    [SerializeField] private EndlessEnemyWave[] enemiesList;


    [HideInInspector] public EnemyWave wave;
    [HideInInspector] public List<EnemySpawn> enemySpawn;
    private GameObject[] _enemies;
    public float increaseEnemySpeedDifficultyRate = 0.05f;
    public float increaseEnemyAttackDifficultyRate = 0.1f;
    public float increaseEnemyHealthDifficultyRate = 0.1f;
    public float increaseEnemyAmountDifficultyRate = 0.4f;
    public float increaseEnemyWaitDifficultyRate = 0.2f;
    private float[] _enemyCounts;
    public float initialWaitAmount = 3;
    private bool _nightMode = false;
    private float _nightModeXpMultiplier = 1f;
    [HideInInspector] public int waveCount;

    int totalEnemy, currentSpawn;

    void Start()
    {
        if (GameLevelSetup.Instance)
        {
            if (GameLevelSetup.Instance.type() == LevelWave.LevelType.Endless)
            {
                GetComponent<LevelEnemyManager>().enabled = false;
            }

            _nightMode = GameLevelSetup.Instance.NightMode();
            enemiesList = GameLevelSetup.Instance.EndlessInitialWave();
            increaseEnemySpeedDifficultyRate = GameLevelSetup.Instance.IncreaseEnemySpeedDifficultyRate();
            increaseEnemyAttackDifficultyRate = GameLevelSetup.Instance.IncreaseEnemyAttackDifficultyRate();
            increaseEnemyHealthDifficultyRate = GameLevelSetup.Instance.IncreaseEnemyHealthDifficultyRate();
            increaseEnemyAmountDifficultyRate = GameLevelSetup.Instance.IncreaseEnemyAmountDifficultyRate();
            increaseEnemyWaitDifficultyRate = GameLevelSetup.Instance.IncreaseEnemyWaitDifficultyRate();
            initialWaitAmount = GameLevelSetup.Instance.InitialWaitAmount();
        }

        _enemies = new GameObject[enemiesList.Length];
        _enemyCounts = new float[enemiesList.Length];
        for (int a = 0; a < enemiesList.Length; a++)
        {
            _enemyCounts[a] = enemiesList[a].initialCount;
            _enemies[a] = enemiesList[a].enemyObject;
        }
    }


    // generate a new wave harder than last
    private void GenerateWave()
    {
        enemySpawn.Clear();

        // check if ran out of new enemies 
        int enemiesInUse;
        if ((waveCount * increaseEnemyAmountDifficultyRate) + 1 < _enemies.Length)
        {
            enemiesInUse = Convert.ToInt32(Mathf.Round((waveCount * increaseEnemyAmountDifficultyRate) + 1));
        }
        else
        {
            enemiesInUse = _enemies.Length;
        }

        // add new enemies to wave spwan 
        // add to the health, speed, and attack damage of enemies 

        for (int i = 0; i < enemiesInUse; i++)
        {
            EnemySpawn newSpawn = new EnemySpawn();
            newSpawn.enemy = _enemies[i];
            _enemyCounts[i] += increaseEnemyAmountDifficultyRate;
            newSpawn.numberEnemy = Convert.ToInt32(Mathf.Round(_enemyCounts[i]));
            newSpawn.wait = 1 / ((waveCount + 1) * increaseEnemyWaitDifficultyRate);
            newSpawn.customHealth =
                Convert.ToInt32(Mathf.Round(_enemies[i].GetComponent<SmartEnemyGrounded>().health *
                                            waveCount * increaseEnemyHealthDifficultyRate));
            newSpawn.customSpeed =
                Convert.ToInt32(Mathf.Round(_enemies[i].GetComponent<SmartEnemyGrounded>().walkSpeed *
                                            waveCount * increaseEnemySpeedDifficultyRate));


            var rangeAttack = _enemies[i].GetComponent<EnemyRangeAttack>();
            if (rangeAttack)
            {
                newSpawn.customAttackDmg += (waveCount * increaseEnemyAttackDifficultyRate + 1) * rangeAttack.damage;
            }

            var meleeAttack = _enemies[i].GetComponent<EnemyMeleeAttack>();
            if (meleeAttack)
            {
                newSpawn.customAttackDmg +=
                    (waveCount * increaseEnemyAttackDifficultyRate + 1) * meleeAttack.dealDamage;
            }

            var throwAttack = _enemies[i].GetComponent<EnemyThrowAttack>();
            if (throwAttack)
            {
                newSpawn.customAttackDmg += (waveCount * increaseEnemyAttackDifficultyRate + 1) * throwAttack.damage;
            }


            enemySpawn.Add(newSpawn);
        }

        // set wave data ready for usage in LevelEnemyManager.cs for creation
        waveCount++;
        wave.wait = initialWaitAmount;
        wave.enemySpawns = enemySpawn.ToArray();
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

        foreach (GameObject enemy in listEnemySpawned)
        {
            Destroy(enemy);
        }

        listEnemySpawned.Clear();

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
                    if (_nightMode)
                    {
                        _temp.GetComponent<SmartEnemyGrounded>().health = Mathf.RoundToInt(
                            _temp.GetComponent<SmartEnemyGrounded>().health *
                            GameLevelSetup.Instance.NightModeXpMultiplier());
                        _temp.GetComponent<GiveExpWhenDie>().expMax = Mathf.RoundToInt(
                            _temp.GetComponent<GiveExpWhenDie>().expMax *
                            GameLevelSetup.Instance.NightModeXpMultiplier());
                        _temp.GetComponent<GiveExpWhenDie>().expMin = Mathf.RoundToInt(
                            _temp.GetComponent<GiveExpWhenDie>().expMin *
                            GameLevelSetup.Instance.NightModeXpMultiplier());
                    }

                    currentSpawn++;
                    MenuManager.Instance.UpdateEnemyWavePercent(currentSpawn, totalEnemy);

                    yield return new WaitForSeconds(enemySpawn.rate);
                }
            }

            while (IsEnemyAlive())
            {
                yield return new WaitForSeconds(0.1f);
            }

            EnemyWaves = new EnemyWave[0];
        }
    }

    void Update()
    {
        if (EnemyWaves.Length == 0)
        {
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
