using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {

    public List<GameObject> enemyList;
    public GameObject enemyCatcher;

    public float halfHeight = 100;

    //Testing Variables
    [Header("Testing Variables")]
    public int difficulty = 0;
    public float minTime = 0.5f, maxTime = 4;

    private void Start()
    {
        StartCoroutine(ConstantRandomSpawner());
        StartCoroutine(WatchTimer());
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator WatchTimer()
    {
        while(difficulty < 10)
        {
            yield return new WaitForSeconds(120);
            difficulty++;
            maxTime -= 0.3f;
        }
    }

    IEnumerator ConstantRandomSpawner()
    {
        float randoTime = 0;
        while (this)
        {
            randoTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randoTime);
            GameObject enemy = SpawnRandomEnemy();
            enemy.GetComponent<ShipStats>().MaxHp += difficulty;
            enemy.transform.localPosition = RandomSpot();
        }
    }

    public Vector2 RandomSpot()
    {
        float y = Random.Range(-halfHeight, halfHeight);
        return new Vector2(0, y);
    }

    public GameObject SpawnRandomEnemy()
    {
        int rndNum = Random.Range(0, enemyList.Count);
        return Instantiate(enemyList[rndNum], transform);
    }
}
