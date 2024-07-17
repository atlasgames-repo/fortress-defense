using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsage : MonoBehaviour
{
    public enum Items
    {
        Log
    }

    public GameObject logPrefab;
    public Transform itemSpawnPos;
    
    public void SpawnItem(string itemName)
    {
        if (itemName == "Log")
        {
            Instantiate(logPrefab,itemSpawnPos.position, Quaternion.identity);
        }
    }
}
