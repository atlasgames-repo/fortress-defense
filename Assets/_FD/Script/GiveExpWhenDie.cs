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
    public string[] affectedItems;
    private bool _isEndless = false;
    private ShopItemData.ShopItem[] _data;
    void Start()
    {
        ShopItemData.ShopItem[] _data = new ShopItemData.ShopItem[0];
        TimeChecker time_checker = FindFirstObjectByType<TimeChecker>();
        if (time_checker) _data = time_checker.data.ShopData;
        int initialExpMin = expMin;
        int initialExpMax = expMax;
        if (GameLevelSetup.self && GameLevelSetup.self.NightMode())
        {
            if (useCustomNightMultiplierOnly)
                {
                    expMax = Mathf.RoundToInt(initialExpMax * customNightMultiplier);
                    expMin = Mathf.RoundToInt(initialExpMin * customNightMultiplier);
                }
                else
                {
                    expMax =  Mathf.RoundToInt(GameLevelSetup.self.NightModeXpMultiplier() * initialExpMax);
                    expMin = Mathf.RoundToInt(GameLevelSetup.self.NightModeXpMultiplier() * initialExpMin);
                }
        }
        else
        {
            bool breakLoop = false;
            for (int i = 0; i < _data.Length; i++)
            {
                for (int j = 0; j < affectedItems.Length; j++)
                {
                    if (_data[i].itemName == affectedItems[j])
                    {
                        breakLoop = true;
                        expMax = Mathf.RoundToInt(initialExpMax * 2);
                        expMin = Mathf.RoundToInt(initialExpMin * 2);
                        if (breakLoop)
                        {
                            break;
                        }
                    }
                }

                if (breakLoop)
                {
                    break;
                }
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

