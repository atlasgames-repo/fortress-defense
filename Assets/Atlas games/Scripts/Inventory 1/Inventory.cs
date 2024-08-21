using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ShopItemData data;
    public ItemPicker itemPicker;
    public InventorySlot[] magicSlotsUI;
    public InventorySlot[] itemSlotsUI;
    public InventorySlot[] petSlotsUI;
    public InventorySlot[] towerSlotsUI;
    [HideInInspector] public int[] chosenMagics;
    [HideInInspector] public int[] chosenItems;
    [HideInInspector] public int[] chosenPet;
    [HideInInspector] public int[] chosenTower;
    public int[] chosenInitialItemsID;
    public int[] chosenInitialMagicsID;
    public int[] chsenInitialPetsID;
    public int[] chosenInitialTowersID;
    private int _editingSlot;
    private Shop.ItemTypes _edittedType;
    void Start()
    {
        InitSlots();
    }

    public void InitSlots()
    {
        ShopItemData.ShopItem[] itemsData = data.ShopData;
        chosenMagics = new int[magicSlotsUI.Length];
        chosenItems = new int[itemSlotsUI.Length];
        chosenPet = new int[petSlotsUI.Length];
        chosenTower = new int[towerSlotsUI.Length];
        for (int i = 0; i < chosenMagics.Length; i++)
        {
            chosenMagics[i] = chosenInitialMagicsID[i];
            for (int j = 0; j < itemsData.Length; j++)
            {
                if (chosenMagics[i] == itemsData[j].id)
                {
                    towerSlotsUI[i].Init(itemsData[j].itemImage);
                }
            }
        }

        for (int i = 0; i < chosenItems.Length; i++)
        {
            chosenItems[i] = chosenInitialItemsID[i];
            for (int j = 0; j < itemsData.Length; j++)
            {
                if (chosenItems[i] == itemsData[j].id)
                {
                    towerSlotsUI[i].Init(itemsData[j].itemImage);
                }
            }
        }

        for (int i = 0; i < chosenPet.Length; i++)
        {
            chosenPet[i] = chsenInitialPetsID[i];
            for (int j = 0; j < itemsData.Length; j++)
            {
                if (chosenPet[i] == itemsData[j].id)
                {
                    towerSlotsUI[i].Init(itemsData[j].itemImage);
                }
            }
        }

        for (int i = 0; i < chosenTower.Length; i++)
        {
            chosenTower[i] = chosenInitialTowersID[i];
            for (int j = 0; j < itemsData.Length; j++)
            {
                if (chosenTower[i] == itemsData[j].id)
                {
                    towerSlotsUI[i].Init(itemsData[j].itemImage);
                }
            }
        }
    }
    public void OpenPets()
    {
        _editingSlot = 0;
        _edittedType = Shop.ItemTypes.Pet;
        itemPicker.gameObject.SetActive(true);
        itemPicker.Init(data,chosenPet,Shop.ItemTypes.Pet);
    }

    public void OpenItems(int slot)
    {
        _editingSlot = slot;
        _edittedType = Shop.ItemTypes.Item;
        itemPicker.gameObject.SetActive(true);
        itemPicker.Init(data, chosenItems,Shop.ItemTypes.Item);
    }

    public void OpenMagics(int slot)
    {
        _editingSlot = slot;
        _edittedType = Shop.ItemTypes.Magic;
        itemPicker.gameObject.SetActive(true);
        itemPicker.Init(data,chosenMagics,Shop.ItemTypes.Magic);
    }

    public void OpenTowers()
    {
        _editingSlot = 0;
        _edittedType = Shop.ItemTypes.Towers;
        itemPicker.gameObject.SetActive(true);
        itemPicker.Init(data,chosenTower,Shop.ItemTypes.Towers);
    }

    public void ChangeChosenItem(ShopItemData.ShopItem item)
    {
        switch (_edittedType)
        {
            case Shop.ItemTypes.Item:
                itemSlotsUI[_editingSlot].ChangeSlotSprite(item.itemImage);
                chosenItems[_editingSlot] = item.id;
                break;
            case Shop.ItemTypes.Magic:
                magicSlotsUI[_editingSlot].ChangeSlotSprite(item.itemImage);
                chosenMagics[_editingSlot] = item.id;
                break;
            case Shop.ItemTypes.Pet:
                petSlotsUI[_editingSlot].ChangeSlotSprite(item.itemImage);
                chosenPet[_editingSlot] = item.id;
                break;
            case Shop.ItemTypes.Towers:
                towerSlotsUI[_editingSlot].ChangeSlotSprite(item.itemImage);
                chosenTower[_editingSlot] = item.id;
                break;
        }
    }
    
    
}
