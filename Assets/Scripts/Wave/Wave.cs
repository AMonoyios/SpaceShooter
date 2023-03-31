using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    [Min(0.0f)]
    public float spawnTimer;
}
