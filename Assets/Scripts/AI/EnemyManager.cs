using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region SingletonLogic
    public static EnemyManager instance { get; private set; } = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    #endregion
    private static List<EnemyBase> enemies = new List<EnemyBase>();
    private static Dictionary<int, List<EnemySpawner>> enemySpawners = new Dictionary<int, List<EnemySpawner>>();
    public PlayerController playerRef;

    public void RegisterSpawner(int key_, EnemySpawner es_)
    {
        //Automating the creation of enemyspawner lists
        if (enemySpawners.ContainsKey(key_))
        {
            enemySpawners[key_].Add(es_);
        }
        else
        {
            List<EnemySpawner> les = new List<EnemySpawner>();
            les.Add(es_);
            enemySpawners.Add(key_, les);
        }

        BeginEncounter(0);
    }

    //Asks spawners to spawn enemies
    public void BeginEncounter(int encounterIndex_)
    {
        if (enemySpawners.ContainsKey(encounterIndex_))
        {
            foreach (EnemySpawner es in enemySpawners[encounterIndex_])
            {
                es.SpawnEnemy();
            }
        }
    }

    #region enemiesListManagement
    //Adds an enemy to the list and returns their list index
    public int AddEnemyToList(EnemyBase e_)
    {
        enemies.Add(e_);
        return enemies.Count - 1;

    }

    //Called by enemies upon dying
    public void RemoveEnemy(int index_)
    {
        if (index_ >= 0 && index_ < enemies.Count)
        {
            enemies.RemoveAt(index_);
            if(enemies.Count == 0)
            {
                //TODO
                //Call Sean's stuff here
            }
        }
    }
    #endregion
}
