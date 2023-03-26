using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UILevelsPanel : UIBasePanel
{
    [SerializeField]
    private UIPanelsManager uiPanelsManager;

    [Header("Buttons")]
    [SerializeField]
    private Button backBtn;

    private void Awake()
    {
        backBtn.onClick.AddListener(ShowMenuPanel);
    }

    public override void HidePanel()
    {
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
        ShowPanelBehaviour();

        gameObject.SetActive(true);
    }

    public override void ShowPanelBehaviour()
    {
        base.ShowPanelBehaviour();

        // This is where you can code custom show behaviour for specific panel
    }

    private void ShowMenuPanel()
    {
        uiPanelsManager.CloseAllPanels();
        uiPanelsManager.menuPanel.ShowPanel();
    }
}
