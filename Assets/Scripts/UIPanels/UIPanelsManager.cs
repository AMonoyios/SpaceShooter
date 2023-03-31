using UnityEngine;

/// <summary>
///     Manager that handles all logic for panels
/// </summary>
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

    // Resets the player prefs, this is only for testing purposes
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

    /// <summary>
    ///     Hides all panels. This is chained with desired panel's ShowPanel() method
    /// </summary>
    public void HideAllPanels()
    {
        menuPanel.HidePanel();
        levelsPanel.HidePanel();
        upgradesPanel.HidePanel();
    }
}
