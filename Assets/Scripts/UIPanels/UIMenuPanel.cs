using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Menu panel
/// </summary>
public sealed class UIMenuPanel : UIBasePanel
{
    [Header("Menu Panel Properties")]
    [SerializeField]
    private Button levelsBtn;
    [SerializeField]
    private Button upgradesBtn;
    [SerializeField]
    private Button quitBtn;

    [Header("Upgrades available")]
    [SerializeField]
    private GameObject upgradesAvailableMark;

    private void Start()
    {
        if (DataManager.Instance.playerData.isAudioOn)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Music, isLooping: true);
        }
    }

    private void OnEnable()
    {
        CheckForUpgradesNotification();
    }

    public override void Awake()
    {
        base.Awake();

        levelsBtn.onClick.AddListener(ShowLevelsPanel);
        upgradesBtn.onClick.AddListener(ShowUpgradesPanel);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void ShowLevelsPanel()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.levelsPanel.ShowPanel();
    }

    /// <summary>
    ///     Method that checks if the player has enough scrap to perform any upgrade
    /// </summary>
    private void CheckForUpgradesNotification()
    {
        upgradesAvailableMark.SetActive(DataManager.Instance.playerData.scrap >= 10);
    }

    private void ShowUpgradesPanel()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.upgradesPanel.ShowPanel();
    }

    private void QuitGame()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        DataManager.Instance.SaveData();

        Debug.LogWarning("Closing game");
        Application.Quit();
    }
}
