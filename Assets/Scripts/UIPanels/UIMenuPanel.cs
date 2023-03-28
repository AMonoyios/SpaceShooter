using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIMenuPanel : UIBasePanel
{
    [Header("Buttons")]
    [SerializeField]
    private Button levelsBtn;
    [SerializeField]
    private Button upgradesBtn;
    [SerializeField]
    private Button quitBtn;

    private void Awake()
    {
        levelsBtn.onClick.AddListener(ShowLevelsPanel);
        upgradesBtn.onClick.AddListener(ShowUpgradesPanel);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public override void HidePanel()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    public override void ShowPanel()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(true);
    }

    private void ShowLevelsPanel()
    {
        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.levelsPanel.ShowPanel();
    }

    private void ShowUpgradesPanel()
    {
        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.upgradesPanel.ShowPanel();
    }

    private void QuitGame()
    {
        DataManager.Instance.SaveData();

        Debug.LogWarning("Closing game");
        Application.Quit();
    }
}
