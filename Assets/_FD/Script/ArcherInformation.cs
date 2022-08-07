using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherInformation : MonoBehaviour
{
    public UpgradedCharacterParameter upgradedCharacterID;
    public Text priceTxt, damageTxt;
    public Image doubleIco, iceIco, poisonIco;

    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = "EXP = " + upgradedCharacterID.price;
        damageTxt.text = "Damage = " + (upgradedCharacterID.weaponEffect.normalDamageMin + upgradedCharacterID.UpgradeRangeDamage) + "~" + (upgradedCharacterID.weaponEffect.normalDamageMax + upgradedCharacterID.UpgradeRangeDamage);
        doubleIco.color = upgradedCharacterID.numberOfArrow == NumberArrow.Double ? Color.white : new Color(0,0,0,0.25f);
        iceIco.color = upgradedCharacterID.weaponEffect.effectType == WEAPON_EFFECT.FREEZE ? Color.white : new Color(0, 0, 0, 0.25f);
        poisonIco.color = upgradedCharacterID.weaponEffect.effectType == WEAPON_EFFECT.POISON ? Color.white : new Color(0, 0, 0, 0.25f);
    }
}
