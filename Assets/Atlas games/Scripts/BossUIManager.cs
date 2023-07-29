using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public EnemySpawn.isBoss bossType = EnemySpawn.isBoss.NONE;
    public Image EnemyHealthBar;
    public Enemy enemy;
    public GameObject miniboss, boss;

    private bool isShaking = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        miniboss.SetActive(false);
        boss.SetActive(false);
        switch (bossType)
        {
            case EnemySpawn.isBoss.MINIBOSS:
                miniboss.SetActive(true);
                ScreenShake.instance.StartShake(0.08f, 0.08f);
                break;
            case EnemySpawn.isBoss.BOSS:
                boss.SetActive(true);
                break;
            default:
                break;
        }

        StartCoroutine(ShakeScreenRepeatedly());
    }

    IEnumerator ShakeScreenRepeatedly() {

            if (!isShaking)
            {
                ScreenShake.instance.StartShake(0.1f, 0.1f);
                isShaking = true;
            }

            yield return new WaitForSeconds(0.5f);

            isShaking = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHealthBar.fillAmount = (float)enemy.currentHealth / enemy.health;
        if(enemy.health <= 0) {
            ScreenShake.instance.StartShake(0, 0);
        }
    }
}
