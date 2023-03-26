using UnityEngine;
using UnityEngine.UI;

public sealed class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private string levelSceneName;

    [SerializeField]
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(Load);
    }

    private void Load()
    {
        SceneManager.Instance.LoadScene(levelSceneName);
    }
}
