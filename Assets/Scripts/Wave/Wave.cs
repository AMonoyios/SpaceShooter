using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class Wave
{
    [SerializeField]
    private Enemy[] enemies;
    [Min(0.0f)]
    public float spawnTimer;

    public void SpawnEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Spawn();
        }
    }

    public void DespawnEnemies(bool ignoreNextWave = false)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.Despawn(ignoreNextWave);
        }
    }

    public bool IsWaveCompleted
    {
        get
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsAlive)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
