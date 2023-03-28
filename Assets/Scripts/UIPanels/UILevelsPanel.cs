using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UILevelsPanel : UIBasePanel
{
    [Header("Buttons")]
    [SerializeField]
    private Button backBtn;

    private void Awake()
    {
        backBtn.onClick.AddListener(ShowMenuPanel);
    }

    public override void ShowPanel()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(true);
    }

    public override void HidePanel()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private void ShowMenuPanel()
    {
        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.menuPanel.ShowPanel();
    }
}
