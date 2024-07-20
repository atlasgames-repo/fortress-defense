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
    public float slowedDownRate = 0.3f;
    public float slowDownTime = 3f;
    public void SpawnItem(string itemName)
    {
        if (itemName == "Log")
        {
            Instantiate(logPrefab,itemSpawnPos.position, Quaternion.identity);
        }else if (itemName == "SlowDown")
        {
            GlobalValue.SlowDownRate = slowedDownRate;
            StartCoroutine(DisableSlowDown());
        }
    }

    IEnumerator DisableSlowDown()
    {
        yield return new WaitForSeconds(slowDownTime);
        GlobalValue.SlowDownRate = 1f;
    }
}
