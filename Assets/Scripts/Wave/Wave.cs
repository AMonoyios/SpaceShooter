using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public sealed class Wave
{
    [SerializeField]
    private GameObject[] enemies;
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
        Debug.Log($"Current enemies list count: {enemiesList.Count}");
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
            bool isThereAnyoneAlive = false;
            foreach (Enemy enemy in enemiesList)
            {
                Debug.Log($"{enemy.name} health: {enemy.Health}");
                if (enemy.IsAlive)
                {
                    Debug.Log(enemy.name);
                    isThereAnyoneAlive = true;
                }
            }

            return isThereAnyoneAlive;
        }
    }
}
