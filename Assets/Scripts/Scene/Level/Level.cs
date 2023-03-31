using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///     Class that controls the current level
/// </summary>
[RequireComponent(typeof(Button))]
public sealed class Level : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private int levelIndex;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(int index)
    {
        levelIndex = index;
        levelText.text = levelIndex.ToString();

        button.interactable = levelIndex <= DataManager.Instance.playerData.completedLevels + 1;

        button.onClick.AddListener(() => Load(levelIndex));
    }

    private void Load(int levelIndex)
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        DataManager.Instance.currentLevel = levelIndex;
        DataManager.Instance.SaveData();

        SceneManager.Instance.LoadScene("Metagame");
    }
}
