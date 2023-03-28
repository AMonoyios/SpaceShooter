using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Levels", fileName = "Levels")]
public class LevelsScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class Level
    {
        public List<Wave> waves = new List<Wave>();
    }

    public List<Level> levels = new List<Level>();
}
