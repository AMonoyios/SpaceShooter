using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Scriptable object that holds all metagame leve data
/// </summary>
[CreateAssetMenu(menuName = "Assets/Levels", fileName = "Levels")]
public sealed class LevelsScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public List<Wave> waves = new List<Wave>();
    }

    public List<Level> levels = new List<Level>();
}
