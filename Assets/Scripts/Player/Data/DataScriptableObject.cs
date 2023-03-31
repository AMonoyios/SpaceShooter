using UnityEngine;

/// <summary>
///     Scriptable object that holds all data for player. This can also be used for enemy but enemy script will need refactoring
/// </summary>
[CreateAssetMenu(menuName = "Assets/Stats", fileName = "Stats")]
public class DataScriptableObject : ScriptableObject
{
    [Min(0)]
    public int scrap = 0;

    [Space(10.0f)]

    [Min(1)]
    public int health = 5;
    [Min(50.0f)]
    public float speed = 100.0f;
    [Min(1)]
    public int damage = 1;
    [Min(0.5f)]
    public float reloadTime = 1.5f;

    [Space(10.0f)]

    [Min(0)]
    public int completedLevels = -1;

    [Space(10.0f)]
    public bool isAudioOn = true;
}
