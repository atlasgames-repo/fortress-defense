using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UI : MonoBehaviour
{
    public float lerpSpeed = 1;

    [Header("PLAYER HEALTHBAR")]
    public Slider healthSlider;
    public Text health;
    float healthValue;

    //[Header("ENEMY HEALTHBAR")]
    //public Slider enemyHealthSlider;
    //public Text enemyHealth;
    //float enemyHealthValue;

    [Header("ENEMY WAVE")]
    public Slider enemyWavePercentSlider;
    float enemyWaveValue;

    [Space]
    public Text coinTxt;
    public Text expTxt;
    public Text levelName;

    private void Start()
    {
        healthValue = 1;
        enemyWaveValue = 0;

        healthSlider.value = 1;
        enemyWavePercentSlider.value = 0;
        levelName.text = "Level " + GlobalValue.levelPlaying;
    }

    private void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, healthValue, lerpSpeed * Time.deltaTime);
        //enemyHealthSlider.value = Mathf.Lerp(enemyHealthSlider.value, enemyHealthValue, lerpSpeed * Time.deltaTime);
        
        enemyWavePercentSlider.value = Mathf.Lerp(enemyWavePercentSlider.value, enemyWaveValue, lerpSpeed * Time.deltaTime);
        coinTxt.text = GlobalValue.SavedCoins + "";
        expTxt.text = GameManager.Instance.currentExp + "";
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth/*, HEALTH_CHARACTER healthBarType*/)
    {
        //if (healthBarType == HEALTH_CHARACTER.PLAYER)
        //{
            healthValue = Mathf.Clamp01(currentHealth / maxHealth);
            health.text = (int)currentHealth + "/" + (int)maxHealth;
        //}
        //else if(healthBarType == HEALTH_CHARACTER.ENEMY)
        //{
        //    enemyHealthValue = Mathf.Clamp01(currentHealth / maxHealth);
        //    enemyHealth.text = (int) currentHealth + "/" + (int)maxHealth;
        //}
    }

    public void UpdateEnemyWavePercent(float currentSpawn, float maxValue)
    {
        enemyWaveValue = Mathf.Clamp01(currentSpawn / maxValue);
    }
    
   
}
