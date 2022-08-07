using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemHelper : MonoBehaviour {
	public bool spawnWhenHit = false;
	public bool spawnWhenDie = true;
	[Range(0,1)]
	public float chanceSpawn=0.5f;
	public GameObject[] Items;
	public Transform spawnPoint;

	void Start(){
		if (spawnPoint == null)
			spawnPoint = transform;
	}

	public void Spawn(){
		if (Items.Length > 0 && Random.Range (0f, chanceSpawn) < chanceSpawn) {
            Instantiate(Items[Random.Range(0, Items.Length)], spawnPoint.position, Quaternion.identity);
		}
	}
}
