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

    public bool IsWaveCompleted
    {
        get
        {
            if (spawnTimer < 0)
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
            else
            {
                return false;
            }
        }
    }
}
