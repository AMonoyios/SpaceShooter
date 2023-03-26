using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIMenuPanel : UIBasePanel
{
    [SerializeField]
    private UIPanelsManager uiPanelsManager;

    [Header("Buttons")]
    [SerializeField]
    private Button levelsBtn;
    [SerializeField]
    private Button quitBtn;

    private void Awake()
    {
        levelsBtn.onClick.AddListener(ShowLevelsPanel);
        quitBtn.onClick.AddListener(QuitGame);
    }

    public override void HidePanel()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        HidePanelBehaviour();

        gameObject.SetActive(false);
    }

    public override void HidePanelBehaviour()
    {
        base.HidePanelBehaviour();

        // This is where you can code custom hide behaviour for specific panel
    }

    public override void ShowPanel()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        ShowPanelBehaviour();

        gameObject.SetActive(true);
    }

    public override void ShowPanelBehaviour()
    {
        base.ShowPanelBehaviour();

        // This is where you can code custom show behaviour for specific panel
    }

    private void ShowLevelsPanel()
    {
        uiPanelsManager.CloseAllPanels();
        uiPanelsManager.levelsPanel.ShowPanel();
    }

    private void QuitGame()
    {
        Debug.LogWarning("Closing game");
        Application.Quit();
    }
}
