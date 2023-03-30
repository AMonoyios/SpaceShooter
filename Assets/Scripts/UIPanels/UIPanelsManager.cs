using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIPanelsManager : MonoSingleton<UIPanelsManager>
{
    [Header("Panels")]
    public UIMenuPanel menuPanel;
    public UILevelsPanel levelsPanel;
    public UIUpgradesPanel upgradesPanel;

    private void Awake()
    {
        DataManager.Instance.LoadData();
    }

    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.LogWarning("Reset player prefs");
            DataManager.Instance.ResetData();
        }
    }
    #endif

    public void HideAllPanels()
    {
        menuPanel.HidePanel();
        levelsPanel.HidePanel();
        upgradesPanel.HidePanel();
    }
}
