using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] EnemyObjs;
    public Transform[] EnemySpawnPoints;
    public float curSpawnDelay = 0f;
    public float maxSpawnDelay = 1f;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            EnemySpawn();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0f;
        }
    }

    void EnemySpawn()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 5);
        Instantiate(EnemyObjs[ranEnemy], EnemySpawnPoints[ranPoint].position, EnemySpawnPoints[ranPoint].rotation);

    }

}
