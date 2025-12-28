using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSlotManager : MonoBehaviour
{
    private int[] _chosenMagics;
    public ShopItemData data;
    public AffectZoneButton[] slots;
    [HideInInspector]public bool abilityUsed;

    void Start()
    {
        _chosenMagics = new int[slots.Length];
        string[] chosenMagicsDecode = GlobalValue.inventoryMagic.Split(',');
        for (int i = 0; i < chosenMagicsDecode.Length; i++)
        {
            _chosenMagics[i] = int.Parse(chosenMagicsDecode[i]);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            var Data = GetMagicData(_chosenMagics[i]);
            slots[i].image.sprite = Data.buttonImage;
            switch (Data.itemName)
            {
                case "Lightning Strick":
                    slots[i].affectType = AffectZoneType.Lighting;
                    break;
                case "Fire Strike":
                    slots[i].affectType = AffectZoneType.Fire;
                    break;
                case "Frozen":
                    slots[i].affectType = AffectZoneType.Frozen;
                    break;
                case "Dark":
                    slots[i].affectType = AffectZoneType.Dark;
                    break;
                case "Gale":
                    slots[i].affectType = AffectZoneType.Aero;
                    break;
                case "Poison":
                    slots[i].affectType = AffectZoneType.Poison;
                    break;
                case "Ice Spike":
                    slots[i].affectType = AffectZoneType.Frozen;
                    break;
                case "Cure":
                    slots[i].affectType = AffectZoneType.Cure;
                    break;
                case "Magnet Pull":
                    slots[i].affectType = AffectZoneType.Magnet;
                    break;
                case "LightningAll":
                    slots[i].affectType = AffectZoneType.LightningAll;
                    break;
                case "DefenseWall":
                    slots[i].affectType = AffectZoneType.DefenseWall;
                    break;
            }
        }
    }

    public void OnFirstMagicUse()
    {
        if (!abilityUsed)
        {
            abilityUsed = true;
            foreach (AffectZoneButton slot in slots)
            {
                slot.UpdateXPText();
            }
        }
    }

    ShopItemData.ShopItem GetMagicData(int id)
    {
        ShopItemData.ShopItem chosenData = new ShopItemData.ShopItem();
        for (int i = 0; i < data.ShopData.Length; i++)
        {
            if (data.ShopData[i].id == id)
            {
                chosenData = data.ShopData[i];
            }
        }

        return chosenData;
    }
}
