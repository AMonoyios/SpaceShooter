using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIPanelsManager : MonoBehaviour
{
    [Header("Panels")]
    public UIMenuPanel menuPanel;
    public UILevelsPanel levelsPanel;

    public void CloseAllPanels()
    {
        menuPanel.HidePanel();
        levelsPanel.HidePanel();
    }
}
