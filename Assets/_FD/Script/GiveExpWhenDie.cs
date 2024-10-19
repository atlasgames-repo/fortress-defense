using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GiveExpWhenDie : MonoBehaviour
{
    public int expMin = 5;
    public int expMax = 7;
    [Space(3)] [Header("NightMode")] public int customNightMultiplier = 2;
    public bool useCustomNightMultiplierOnly = false;

    private bool _isEndless = false;

    void Start()
    {
        int initialExpMin = expMin;
        int initialExpMax = expMax;
        if (GameLevelSetup.Instance && GameLevelSetup.Instance.NightMode())
        {
            if (useCustomNightMultiplierOnly)
                {
                    expMax = Mathf.RoundToInt(initialExpMax * customNightMultiplier);
                    expMin = Mathf.RoundToInt(initialExpMin * customNightMultiplier);
                }
                else
                {
                    expMax =  Mathf.RoundToInt(GameLevelSetup.Instance.NightModeXpMultiplier() * initialExpMax);
                    expMin = Mathf.RoundToInt(GameLevelSetup.Instance.NightModeXpMultiplier() * initialExpMin);
                }
        }
    }
    public void GiveExp()
    {
        int random = Random.Range(expMin, expMax); 
        //SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
        //User.Coin = random;
        GameManager.Instance.UpdateXp(random,transform);
        FloatingTextManager.Instance.ShowText((int)random + "", Vector2.up * 1, Color.yellow, transform.position);
    }
}

