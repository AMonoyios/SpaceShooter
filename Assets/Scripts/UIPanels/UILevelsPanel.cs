using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Levels panel
/// </summary>
public sealed class UILevelsPanel : UIBasePanel
{
    [Header("Levels Panel Properties")]
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private LevelsScriptableObject levels;

    [SerializeField]
    private Button backBtn;

    /// <summary>
    ///     Resets the panels visible.
    /// </summary>
    private void Start()
    {
        // FIXME: find a better way to handle duplicate level buttons
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Debug.Log("Deleting levels child");
            Destroy(contentTransform.GetChild(i));
        }

        for (int levelIndex = 0; levelIndex < levels.levels.Count; levelIndex++)
        {
            Level level = Instantiate(levelButtonPrefab, contentTransform).GetComponent<Level>();
            level.Init(levelIndex);
        }
    }

    public override void Awake()
    {
        base.Awake();

        backBtn.onClick.AddListener(ShowMenuPanel);
    }

    private void ShowMenuPanel()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.menuPanel.ShowPanel();
    }
}
