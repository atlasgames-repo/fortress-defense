using System;
using System.Collections;
using System.Collections.Generic;
using DynamicScrollRect;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public DynamicScrollRect.DynamicScrollRect dynamicScrollRect;
    public ScrollContent content;
    public Transform scrollContentParent;
    public TextMeshProUGUI ShopTab;
    public enum ItemTypes
    {
        Pet,Item,Magic,Monster,Website,Towers,Archer
    }

    public enum ItemPurchaseType
    {
        Exp,Coin
    }

    public ShopItemData data;
    ItemTypes _chosenType;
    public void OpenMenu(string menuName)
    {
        //SoundManager.Instance.PauseMusic(true);
        switch (menuName)
        {
            case "pets":
                _chosenType = ItemTypes.Pet;
                ShopTab.SetText("Pets");
                break;
            case "items":
                _chosenType = ItemTypes.Item;
                ShopTab.SetText("Items");
                break;
            case "magics":
                _chosenType = ItemTypes.Magic;
                ShopTab.SetText("Magics");

                break;
            case "monsters":
                _chosenType = ItemTypes.Monster;
                ShopTab.SetText("Monsters");

                break;
            case "website":
                _chosenType = ItemTypes.Website;
                ShopTab.SetText("Website");

                break;
            case "tower":
                _chosenType = ItemTypes.Towers;
                ShopTab.SetText("Towers");

                break;

            case "archer":
                _chosenType = ItemTypes.Archer;
                ShopTab.SetText("Archers");

                break;
        }
        List<ScrollItemData> contentDatas = new List<ScrollItemData>();
        for (int i = 0; i < data.ShopData.Length; i++)
        {
            if (data.ShopData[i].type == _chosenType && !data.ShopData[i].isFree)
            {
                contentDatas.Add(new ScrollItemData(data.ShopData[i]));
            }
        }

        for (int i = 0; i < scrollContentParent.childCount; i++)
        {
            if (scrollContentParent.GetChild(i).name != "RefItem")
            {
                Destroy(scrollContentParent.GetChild(i).gameObject);
            }            
        }

        if (contentDatas.Count > 0)
        {
            dynamicScrollRect.vertical = true;
            content.InitScrollContent(contentDatas);
        }
        else
        {
            dynamicScrollRect.vertical = false;
        }
    }
    
    //delete
    void Start()
    {
        User.Coin = 1000;
    }
}

