using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Matches the encounter index")]
    [SerializeField]
    private int enemySpawnerKey = -1;
    [SerializeField]
    private EnemyBase enemyToSpawn = null;

    public LevelBase level; //The level in which the spawner resides

    void Start()
    {
        EnemyManager.instance.RegisterSpawner(enemySpawnerKey, this);
    }

    public void SpawnEnemy()
    {
        if (enemyToSpawn)
        {
            EnemyBase e = Instantiate(enemyToSpawn, transform.position, transform.rotation);
            if (e)
                e.UpdateLevelData(level); //Pass in the level data for the enemy to know the limits of the room
        }
    }
}
