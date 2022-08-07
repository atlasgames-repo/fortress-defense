using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCallMinion : MonoBehaviour {
	public GameObject minion;
	public LayerMask layerAsGround;
	public float delayCallMin = 3;
	public float delayCallMax = 6;

	public float distanceMin = 1;
	public float distanceMax = 3;

	public int numberMinionMax = 3;
	//int currentSpawn = 0;

	float lastCallTime = 0;

	bool isSpawning = false;
	float delaySpawn;

    List<GameObject> listEnemy;
	// Use this for initialization
	void Start () {
		delaySpawn = Random.Range (delayCallMin, delayCallMax);
        listEnemy = new List<GameObject>();

    }

    public int numberEnemyLive()
    {
        int live = 0;
        if (listEnemy.Count > 0)
        {
            foreach (var obj in listEnemy)
            {
                if (obj.activeInHierarchy)
                    live++;
            }
        }

        return live;
    }

	public bool CanCallMinion(){
		if (isSpawning || numberEnemyLive() >= numberMinionMax)
			return false;

		if (Time.time >= lastCallTime + delaySpawn)
			return true;
		else
			return false;
	}

	public void CallMinion(bool isFacingRight){
		isSpawning = true;
		Vector2	randomSpawnPoint = new Vector2 (Random.Range (transform.position.x + distanceMin * (isFacingRight ? 1 : -1), transform.position.x + distanceMax * (isFacingRight ? 1 : -1)), transform.position.y + 1);
		RaycastHit2D hit = Physics2D.Raycast (randomSpawnPoint, Vector2.down, 10, layerAsGround);
		if (hit) {
            listEnemy.Add(Instantiate(minion, hit.point + Vector2.up * 0.1f, Quaternion.identity));
            //currentSpawn++;
            
            isSpawning = false;
			lastCallTime = Time.time;
			delaySpawn = Random.Range (delayCallMin, delayCallMax);
		} else
			Invoke ("CallMinion", 0.1f);		//wait and check can spawn again
	}
}
