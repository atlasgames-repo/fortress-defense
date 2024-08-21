using System.Collections;
using System.Collections.Generic;
using DynamicScrollRect;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    public DynamicScrollRect.DynamicScrollRect dynamicScrollRect;
    public ScrollContent content;
    public Transform scrollContentParent;
    public void Init(ShopItemData data,int[] chosenItems, Shop.ItemTypes category)
    {
        ClearItems();
        List<ScrollItemData> contentDatas = new List<ScrollItemData>();
        for (int i = 0; i < data.ShopData.Length; i++)
        {
            if (data.ShopData[i].type == category && GlobalValue.GetChosenShopItem(data.ShopData[i].itemName)>0)
            {
               // for (int j = 0; j < data.ShopData.Length; j++)
               // {
               //     
               // }
                contentDatas.Add(new ScrollItemData(data.ShopData[i]));
                content.InitScrollContent(contentDatas);
            }
        }
    }

    public void ClearItems()
    {
        for (int i = 0; i < scrollContentParent.childCount; i++)
        {
            if (scrollContentParent.GetChild(i).name != "RefItem")
            {
                Destroy(scrollContentParent.GetChild(i).gameObject);
            }            
        }
    }
}
