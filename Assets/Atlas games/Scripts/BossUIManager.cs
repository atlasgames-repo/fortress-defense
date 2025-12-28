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
    public GameObject fortHealth;
    public GameObject[] UI_Elements;
    public Animator healthBarAnim;
    bool isBossDead = false;
    [ReadOnly] public float targetFill = 1;
    public float fill_speed = 1;
    // Start is called before the first frame update
    public void UpdateHealthBar(float to) {
        targetFill = to;
    }
    void Update() {
        EnemyHealthBar.fillAmount = Mathf.SmoothStep(EnemyHealthBar.fillAmount, targetFill, Time.deltaTime * fill_speed);
    }
    void OnEnable()
    {
        miniboss.SetActive(false);
        boss.SetActive(false);
        switch (bossType)
        {
            case EnemySpawn.isBoss.MINIBOSS:
                miniboss.SetActive(true);
                //fortHealth.SetActive(true);
                healthBarAnim.SetBool("isBossAttack", true);
                Time.timeScale = 1;
                SoundManager.PlayMusic(SoundManager.Instance.BossMusicClip);
                foreach (GameObject uiElement in UI_Elements)
                {
                    //uiElement.SetActive(false);
                }
                break;
            case EnemySpawn.isBoss.BOSS:
                boss.SetActive(true);
                //fortHealth.SetActive(true);
                healthBarAnim.SetBool("isBossAttack", true);
                Time.timeScale = 1;
                SoundManager.PlayMusic(SoundManager.Instance.BossMusicClip);
                foreach (GameObject uiElement in UI_Elements)
                {
                    //uiElement.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
}
