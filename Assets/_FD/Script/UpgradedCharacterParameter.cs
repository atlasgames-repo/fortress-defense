using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class UpgradeStep
{
    public int price;
    public int rangeDamageStep, criticalStep;
}

public class UpgradedCharacterParameter : MonoBehaviour
{
    public string ID = "unique ID";
    public int price = 50;
    [Header("Default")]
    [Range(0, 100)]
    public float criticalDamagePercent = 10f;
    [Header("ABILITY")]
    public NumberArrow numberOfArrow = NumberArrow.Single;
    public WeaponEffect weaponEffect;
    [Space]
    public UpgradeStep[] UpgradeSteps;
    
    public int CurrentUpgrade
    {
        get
        {
            int current = PlayerPrefs.GetInt(ID + "upgradeHealth" + "Current", 0);
            if (current >= UpgradeSteps.Length)
                current = -1;   //-1 mean overload
            return current;
        }
        set
        {
            PlayerPrefs.SetInt(ID + "upgradeHealth" + "Current", value);
        }
    }

    public void UpgradeCharacter()
    {
        if (CurrentUpgrade == -1)
            return;

        UpgradeRangeDamage += UpgradeSteps[CurrentUpgrade].rangeDamageStep;

        UpgradeCriticalDamage += UpgradeSteps[CurrentUpgrade].criticalStep;

        CurrentUpgrade++;
    }
    
    public int UpgradeRangeDamage
    {
        get { return PlayerPrefs.GetInt(ID + "UpgradeRangeDamage", 0); }
        set { PlayerPrefs.SetInt(ID + "UpgradeRangeDamage", value); }
    }
   
    public float UpgradeCriticalDamage
    {
        get { return PlayerPrefs.GetFloat(ID + "UpgradeCriticalDamage", criticalDamagePercent); }
        set { PlayerPrefs.SetFloat(ID + "UpgradeCriticalDamage", value); }
    }
}
