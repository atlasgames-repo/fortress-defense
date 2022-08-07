using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyManager : MonoBehaviour, IListener
{
    public static LevelEnemyManager Instance;
    public Transform[] spawnPositions;
    public EnemyWave[] EnemyWaves;
    int currentWave = 0;

    List<GameObject> listEnemySpawned = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    int totalEnemy, currentSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (GameLevelSetup.Instance)
            EnemyWaves = GameLevelSetup.Instance.GetLevelWave();

        //calculate number of enemies
        totalEnemy = 0;
        for (int i = 0; i < EnemyWaves.Length; i++)
        {

            for (int j = 0; j < EnemyWaves[i].enemySpawns.Length; j++)
            {
                var enemySpawn = EnemyWaves[i].enemySpawns[j];
                for (int k = 0; k < enemySpawn.numberEnemy; k++)
                {
                    totalEnemy++;
                }
            }
        }

        currentSpawn = 0;
    }

    IEnumerator SpawnEnemyCo()
    {
        for (int i = 0; i < EnemyWaves.Length; i++)
        {
            yield return new WaitForSeconds(EnemyWaves[i].wait);

            for (int j = 0; j < EnemyWaves[i].enemySpawns.Length; j++)
            {
                var enemySpawn = EnemyWaves[i].enemySpawns[j];
                yield return new WaitForSeconds(enemySpawn.wait);
                for (int k = 0; k < enemySpawn.numberEnemy; k++)
                {
                    GameObject _temp = Instantiate(enemySpawn.enemy, (Vector2) spawnPositions[Random.Range(0,spawnPositions.Length)].position, Quaternion.identity) as GameObject;
                    var isEnemy = (Enemy) _temp.GetComponent(typeof(Enemy));
                    if (isEnemy != null)
                    {
                        if (enemySpawn.customHealth > 0)
                            isEnemy.health = enemySpawn.customHealth;
                        if (enemySpawn.customSpeed > 0)
                            isEnemy.walkSpeed = enemySpawn.customSpeed;
                        if (enemySpawn.customAttackDmg > 0)
                        {
                            var rangeAttack = _temp.GetComponent<EnemyRangeAttack>();
                            if (rangeAttack)
                                rangeAttack.damage = enemySpawn.customAttackDmg;
                            var meleeAttack = _temp.GetComponent<EnemyMeleeAttack>();
                            if (meleeAttack)
                                meleeAttack.dealDamage = enemySpawn.customAttackDmg;
                            var throwAttack = _temp.GetComponent<EnemyThrowAttack>();
                            if (throwAttack)
                                throwAttack.damage = enemySpawn.customAttackDmg;
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
        }

        //check all enemy killed
        while (isEnemyAlive()) { yield return new WaitForSeconds(0.1f); }

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.Victory();
    }


    bool isEnemyAlive()
    {
        for(int i = 0; i< listEnemySpawned.Count;i++)
        {
            if (listEnemySpawned[i].gameObject != null && listEnemySpawned[i].activeInHierarchy)
                return true;
        }

        return false;
    }

    public void IGameOver()
    {
        //throw new System.NotImplementedException();
    }

    public void IOnRespawn()
    {
        //throw new System.NotImplementedException();
    }

    public void IOnStopMovingOff()
    {
        //throw new System.NotImplementedException();
    }

    public void IOnStopMovingOn()
    {
        //throw new System.NotImplementedException();
    }

    public void IPause()
    {
        //throw new System.NotImplementedException();
    }

    public void IPlay()
    {
        StartCoroutine(SpawnEnemyCo());
        //throw new System.NotImplementedException();
    }

    public void ISuccess()
    {
        StopAllCoroutines();
        //throw new System.NotImplementedException();
    }

    public void IUnPause()
    {
        //throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class EnemyWave
{
    public float wait = 3;
    public EnemySpawn[] enemySpawns;
}


