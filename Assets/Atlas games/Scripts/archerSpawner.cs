using System;
using UnityEngine;
using UnityEngine.UI;

public class archerSpawner : MonoBehaviour
{
    public Transform[] archersTrfm;
    public GameObject[] archerObj;
    //public ShopItemData itemsData;
    [HideInInspector]public int[] chosenArcher;
    

    public void Start()
    {
        chosenArcher = new int[5];

        string[] chosenArcherDecode = GlobalValue.inventoryArchers.Split(',');
        chosenArcher[0] = int.Parse(chosenArcherDecode[0]);
        chosenArcher[1] = int.Parse(chosenArcherDecode[1]);
        chosenArcher[2] = int.Parse(chosenArcherDecode[2]);
        chosenArcher[3] = int.Parse(chosenArcherDecode[3]);
        chosenArcher[4] = int.Parse(chosenArcherDecode[4]);

        foreach(var location in archersTrfm)
        {

        }

        for(int i = 0 ; i < chosenArcher.Length ; i++)
            {
                Instantiate(archerObj[chosenArcher[i] - 50], archersTrfm[i]);
                Debug.Log(chosenArcher[i] - 50);
            }
    }

    /*public void archerData()
    {
        switch(Inventory.archerID1)
        {
            case 50:
                Instantiate(archerObj[0], archersTrfm);
                Debug.Log("1");
                break;
            case 51:
                Instantiate(archerObj[1], archersTrfm);
                break;
            case 52:
                Instantiate(archerObj[2], archersTrfm);
                break;
            case 53:
                Instantiate(archerObj[3], archersTrfm);
                break;
            case 54:
                Instantiate(archerObj[4], archersTrfm);
                break;
        }
    }
    void archerDataa()
    {
        switch(Inventory.archerID2)
        {
            case 50:
                Instantiate(archerObj[0], archersTrfm);
                break;
            case 51:
                Instantiate(archerObj[1], archersTrfm);
                break;
            case 52:
                Instantiate(archerObj[2], archersTrfm);
                break;
            case 53:
                Instantiate(archerObj[3], archersTrfm);
                break;
            case 54:
                Instantiate(archerObj[4], archersTrfm);
                break;
        }
    }
    void archerDataaa()
    {
        switch(Inventory.archerID3)
        {
            case 50:
                Instantiate(archerObj[0], archersTrfm);
                break;
            case 51:
                Instantiate(archerObj[1], archersTrfm);
                break;
            case 52:
                Instantiate(archerObj[2], archersTrfm);
                break;
            case 53:
                Instantiate(archerObj[3], archersTrfm);
                break;
            case 54:
                Instantiate(archerObj[4], archersTrfm);
                break;
        }
    }
    void archerDataaaa()
    {
        switch(Inventory.archerID4)
        {
            case 50:
                Instantiate(archerObj[0], archersTrfm);
                break;
            case 51:
                Instantiate(archerObj[1], archersTrfm);
                break;
            case 52:
                Instantiate(archerObj[2], archersTrfm);
                break;
            case 53:
                Instantiate(archerObj[3], archersTrfm);
                break;
            case 54:
                Instantiate(archerObj[4], archersTrfm);
                break;
        }
    }
    void archerDataaaaa()
    {
        switch(Inventory.archerID5)
        {
            case 50:
                Instantiate(archerObj[0], archersTrfm);
                break;
            case 51:
                Instantiate(archerObj[1], archersTrfm);
                break;
            case 52:
                Instantiate(archerObj[2], archersTrfm);
                break;
            case 53:
                Instantiate(archerObj[3], archersTrfm);
                break;
            case 54:
                Instantiate(archerObj[4], archersTrfm);
                break;
        }
    }*/
}
