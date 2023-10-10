using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public string[] enemyNames;
    public GameObject[] enemyTypes;

    public List<GameObject> enemies;
    public GameObject enemy;

    public string killCondition = "Two";


    private void Start()
    {
        //SpawnEnemies();

        SpawnAtRandom();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            SpawnAtRandom();

        if(Input.GetKeyDown(KeyCode.K))
            KillAllEnemies();

       // if (Input.GetKeyDown(KeyCode.L))
           // KillSpecificEnemy();
            
    }



    /// <summary>
    /// Spawns enemy at every spawn point
    /// </summary>
    void SpawnEnemies()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }
   
    /// <summary>
    /// Spawns random enemy at random spawn point.
    /// </summary>
    void SpawnAtRandom()
    {
        int rnd = Random.Range(0, enemyTypes.Length);
        Instantiate(enemyTypes[rnd], spawnPoints[rnd].position, spawnPoints[rnd].rotation);
        enemies.Add(enemy);
        ShowEnemyCount(); 
    }

    /// <summary>
    /// Shows amount of enemies in stage
    /// </summary>
    void ShowEnemyCount()
    {
        print(enemies.Count);
    }

    /// <summary>
    /// Kills enemies 
    /// </summary>
    /// <param name="_enemy"></param>

    void KillEnemy(GameObject _enemy)
    {
        if (enemies.Count == 0)
            return;

        Destroy(_enemy);
        enemies.Remove(_enemy);
    }

    /// <summary>
    /// Kills All enemies in our stage
    /// </summary>
    void KillAllEnemies()
    {
        if (enemies.Count == 0)
            return;

        for (int i = enemies.Count-1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
    }

    void KillSpecificEnemy(string _condition)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].name.Contains(_condition))
                KillEnemy(enemies[i]);
        }
    }
 
    /// <summary>
    /// All example code from class
    /// </summary>
    void Examples()
    {
        //LOOPS
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(enemyTypes[i], spawnPoints[i].position, spawnPoints[i].rotation);
        }

        print(enemyNames.Length);
        print(enemyNames[enemyNames.Length - 1]);

        //INSTANTIATING
        GameObject first = Instantiate(enemyTypes[0], spawnPoints[0].position, spawnPoints[0].rotation);
        first.name = enemyNames[0];

        int lastEnemy = enemyTypes.Length - 1;
        GameObject last = Instantiate(enemyTypes[lastEnemy], spawnPoints[lastEnemy].position, spawnPoints[lastEnemy].rotation);
        last.name = enemyNames[lastEnemy];



        //CREATE LOOP IN A LOOP
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);

        for (int i=0; i < 10; i++)
        {
            for (int j=0; j < 10; j++)
            {
                Instantiate(wall, new Vector3(i, j, 0), transform.rotation);
            }
        }
    }
}

