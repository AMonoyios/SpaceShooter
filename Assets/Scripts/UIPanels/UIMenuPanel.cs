using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIMenuPanel : UIBasePanel
{
    [Header("Menu Panel Properties")]
    [SerializeField]
    private Button levelsBtn;
    [SerializeField]
    private Button upgradesBtn;
    [SerializeField]
    private Button quitBtn;

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
