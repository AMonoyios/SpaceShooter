using UnityEngine;
using TMPro;

/// <summary>
///     Game panel
/// </summary>
public sealed class UIGamePanel : UIBasePanel
{
    [Header("Game Panel Properties")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TextMeshProUGUI playerHealthText;

    private void Start()
    {
        EventsManager.Instance.OnUpdateMetagameHUD += UpdatePlayerHealthText;
    }

    /// <summary>
    ///     Method that updates player health. This is called via events upon any player health update
    /// </summary>
    private void UpdatePlayerHealthText()
    {
        playerHealthText.text = "Health: " + player.Health.ToString();
    }
}
