using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
public class GameAnalytics : MonoBehaviour
{
    // Start is called before the first frame update
    public AddAndUpgradePlayer[] Players;
    public float DPS, EDPS, ETH, EXPGPS, DIFF, AZDPS;
    [ReadOnly]
    public float MAX_DIFF = 10_000;
    void Start()
    {
        //File.AppendAllLines()
        StartCoroutine(update());
    }
    void Update()
    {

    }
    // Update is called once per frame
    public string Getpath(int number)
    {
        string path = Application.dataPath.Replace("/Assets", "") + $"/Logs/Analytics/";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        return path + $"Level-{GlobalValue.levelPlaying}-Analytics-{number}.txt";
    }
    IEnumerator update()
    {
        int number = 1;
        string filename = Getpath(number);//$"d:\\Projects\\Hadi Asadollahi\\Hadi Asadollahi\\FORTRESS DEFENSE\\Logs\\Analytics{number}.txt";
        while (true)
        {
            bool is_path_exist = System.IO.File.Exists(filename);
            if (is_path_exist)
            {
                number++;
                filename = Getpath(number);//$"d:\\Projects\\Hadi Asadollahi\\Hadi Asadollahi\\FORTRESS DEFENSE\\Logs\\Analytics{number}.txt";
            }
            else
            {

                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        System.IO.StreamWriter file = new System.IO.StreamWriter(filename, true);
        file.Write($"DPS\tTD\tETH\tEDPS\tEXPGPS\tAZDPS\tDIFF\n");
        yield return new WaitForSeconds(3f);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            float _EDPS = 0, _ETH = 0, _DPS = 0, _EXPGPS = 0, _DIFF = 0, _AZDPS = 0;
            foreach (GameObject enemy in LevelEnemyManager.Instance.listEnemySpawned)
            {

                if (enemy.activeInHierarchy && enemy.GetComponent<Enemy>().enemyState == ENEMYSTATE.ATTACK)
                {
                    if (enemy.GetComponent<Enemy>().attackType == ATTACKTYPE.MELEE)
                    {
                        _EDPS += enemy.GetComponent<EnemyMeleeAttack>().dealDamage;
                    }
                    if (enemy.GetComponent<Enemy>().attackType == ATTACKTYPE.RANGE)
                    {
                        _EDPS += enemy.GetComponent<EnemyRangeAttack>().damage;
                    }
                    if (enemy.GetComponent<Enemy>().attackType == ATTACKTYPE.THROW)
                    {
                        _EDPS += enemy.GetComponent<EnemyThrowAttack>().damage;
                    }

                    //_EXPGPS += (enemy.GetComponent<GiveExpWhenDie>().expMin + enemy.GetComponent<GiveExpWhenDie>().expMax) / 2;
                }
                foreach (AddAndUpgradePlayer item in Players)
                {
                    Player_Archer player = item.GetcurrentPlayer;
                    if (player.gameObject.activeInHierarchy && player.is_attacking)
                    {
                        _ETH += enemy.GetComponent<Enemy>().currentHealth;
                        break;
                    }
                }

            }
            foreach (AddAndUpgradePlayer item in Players)
            {
                Player_Archer player = item.GetcurrentPlayer;
                if (player.gameObject.activeInHierarchy && player.is_attacking)
                {
                    _DPS += (player.damageMin + player.damageMax) / 2;
                }
            }
            foreach (AffectZone item in AffectZoneManager.Instance.affectZoneList)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    switch (item.getAffectZoneType)
                    {
                        case AffectZoneType.Lighting:
                            _AZDPS += item.lightingDamage;
                            break;
                        case AffectZoneType.Frozen:
                            _AZDPS += item.frozenDamage;
                            break;
                        case AffectZoneType.Poison:
                            _AZDPS += item.poisonDamage;
                            break;
                        default:
                            break;
                    }
                }
            }
            _EXPGPS = GameManager.Instance.currentExp;
            EDPS = _EDPS;
            ETH = _ETH;
            DPS = _DPS;
            AZDPS = _AZDPS;
            EXPGPS = _EXPGPS;
            _DIFF = ETH + EDPS - DPS - AZDPS - EXPGPS;
            if (_DIFF < 0)
            {
                _DIFF = 0;
            }
            DIFF = ((float)_DIFF / MAX_DIFF) * 100;
            DIFF = Mathf.Floor(DIFF);
            file.Write($"{DPS}\t{ETH}\t{EDPS}\t{EXPGPS}\t{AZDPS}\t{DIFF}\n");
            if (GameManager.Instance.State != GameManager.GameState.Playing && GameManager.Instance.State != GameManager.GameState.Pause)
            {
                file.Close();
                break;
            }

        }

    }





}
#endif