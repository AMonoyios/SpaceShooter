using UnityEngine;
using UnityEngine.UI;

public sealed class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private string levelSceneName;

    [SerializeField]
    private Button button;

    [SerializeField]
    private LevelIndex levelIndex;

    private void Awake()
    {
        button.onClick.AddListener(() => Load(levelIndex.Index));
    }

    private void Load(int levelIndex)
    {
        DataManager.Instance.currentLevel = levelIndex;
        DataManager.Instance.SaveData();

        SceneManager.Instance.LoadScene(levelSceneName);
    }
}
