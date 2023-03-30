using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class UIUpgradesPanel : UIBasePanel
{
    [Header("Upgrades Panel Properties")]
    [SerializeField]
    private TextMeshProUGUI speedStatText;
    [SerializeField]
    private TextMeshProUGUI reloadeStatText;
    [SerializeField]
    private TextMeshProUGUI healthStatText;
    [SerializeField]
    private TextMeshProUGUI scrapStatText;

    [Header("Upgrade Buttons")]
    [SerializeField]
    private Button upgradeSpeedBtn;
    [SerializeField]
    private Button upgradeRelodeBtn;
    [SerializeField]
    private Button upgradeHealthBtn;

    private const int upgradeCost = 5;
    [Header("Upgrades")]
    [SerializeField]
    private float speedUpgradeValue = 10.0f;
    [SerializeField]
    private float reloadTimeUpgradeValue = 0.1f;
    [SerializeField]
    private int healthUpgradeValue = 1;

    [SerializeField]
    private Button backBtn;

    public override void Awake()
    {
        base.Awake();

        backBtn.onClick.AddListener(ShowMenuPanel);

        upgradeSpeedBtn.onClick.AddListener(UpgradeSpeed);
        upgradeRelodeBtn.onClick.AddListener(UpgradeReload);
        upgradeHealthBtn.onClick.AddListener(UpgradeHealth);
    }

    private void ShowMenuPanel()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.menuPanel.ShowPanel();
    }

    private void UpgradeSpeed()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.speed += speedUpgradeValue;
            UpdateStatsDisplay();
        }
    }

    private void UpgradeReload()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.reloadTime += reloadTimeUpgradeValue;
            UpdateStatsDisplay();
        }
    }

    private void UpgradeHealth()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.health += healthUpgradeValue;
            UpdateStatsDisplay();
        }
    }

    private void UpdateStatsDisplay()
    {
        DataManager.Instance.SaveData();

        speedStatText.text = "Speed " + DataManager.Instance.playerData.speed.ToString();
        reloadeStatText.text = "Reload " + DataManager.Instance.playerData.reloadTime.ToString();
        healthStatText.text = "Health " + DataManager.Instance.playerData.health.ToString();
        scrapStatText.text = "Scrap " + DataManager.Instance.playerData.scrap.ToString();
    }
}
