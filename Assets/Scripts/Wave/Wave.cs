using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField]
    private GameObject[] enemies;
    public GameObject[] Enemies => enemies;
    private readonly List<Enemy> enemiesList = new List<Enemy>();
    [Min(0.0f)]
    public float spawnTimer;

    public void Init()
    {
        Debug.Log($"Initializing wave... current enemies alive: {enemiesList.FindAll((enemy) => enemy.IsAlive).Count}");
        enemiesList.Clear();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesList.Add(enemies[i].GetComponent<Enemy>());
        }
    }

    public void SpawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Object.Instantiate(enemy, Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 180.0f));
        }
    }

    public void DespawnEnemies(bool ignoreNextWave = false)
    {
        foreach (Enemy enemy in enemiesList)
        {
            enemy.Despawn(ignoreNextWave);
        }
    }

    public bool IsWaveCompleted
    {
        get
        {
            foreach (Enemy enemy in enemiesList)
            {
                if (enemy.IsAlive)
                {
                    Debug.Log($"{enemy.name} is still alive.");
                    return false;
                }
            }

            return true;
        }
    }
}
