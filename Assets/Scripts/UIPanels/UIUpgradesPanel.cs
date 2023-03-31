using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///     Upgrades panel
/// </summary>
public sealed class UIUpgradesPanel : UIBasePanel
{
    [Header("Upgrades Panel Properties")]
    [SerializeField]
    private TextMeshProUGUI currentSpeedTxt;
    [SerializeField]
    private TextMeshProUGUI currentReloadTxt;
    [SerializeField]
    private TextMeshProUGUI currentHealthTxt;
    [SerializeField]
    private TextMeshProUGUI currentScrapTxt;

    [Header("Upgrade Buttons")]
    [SerializeField]
    private Button upgradeSpeedBtn;
    [SerializeField]
    private Button upgradeRelodeBtn;
    [SerializeField]
    private Button upgradeHealthBtn;

    [Header("Upgrade Cost Texts")]
    [SerializeField]
    private TextMeshProUGUI upgradeSpeedTxt;
    [SerializeField]
    private TextMeshProUGUI upgradeReloadTxt;
    [SerializeField]
    private TextMeshProUGUI upgradeHealthTxt;

    [Header("Upgrades")]
    [SerializeField, Min(1)]
    private int upgradeCost = 10;
    [SerializeField]
    private float speedUpgradeValue = 10.0f;
    [SerializeField]
    private float reloadTimeUpgradeValue = 0.1f;
    [SerializeField]
    private int healthUpgradeValue = 1;

    [SerializeField]
    private Button backBtn;

    private void Start()
    {
        EventsManager.Instance.OnUpdateUpgradesHUD += UpdatePlayerStats;
    }

    public override void Awake()
    {
        base.Awake();

        backBtn.onClick.AddListener(ShowMenuPanel);

        upgradeSpeedBtn.onClick.AddListener(UpgradeSpeed);
        upgradeRelodeBtn.onClick.AddListener(UpgradeReload);
        upgradeHealthBtn.onClick.AddListener(UpgradeHealth);

        upgradeSpeedTxt.text = $"+{speedUpgradeValue} Speed for {upgradeCost} scrap";
        upgradeReloadTxt.text = $"-{reloadTimeUpgradeValue} Reload time for {upgradeCost} scrap";
        upgradeHealthTxt.text = $"+{healthUpgradeValue} Health for {upgradeCost} scrap";

        UpdatePlayerStats();
    }

    private void ShowMenuPanel()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        UIPanelsManager.Instance.HideAllPanels();
        UIPanelsManager.Instance.menuPanel.ShowPanel();
    }

    /// <summary>
    ///     Function that will upgrade speed and then save
    /// </summary>
    private void UpgradeSpeed()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.speed += speedUpgradeValue;

            DataManager.Instance.SaveData();
            EventsManager.Instance.UpdateUpgradesHUD();
        }
    }

    /// <summary>
    ///     Function that will upgrade reload times and then save
    /// </summary>
    private void UpgradeReload()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.reloadTime -= reloadTimeUpgradeValue;

            DataManager.Instance.SaveData();
            EventsManager.Instance.UpdateUpgradesHUD();
        }
    }

    /// <summary>
    ///     Function that will upgrade health and then save
    /// </summary>
    private void UpgradeHealth()
    {
        if (DataManager.Instance.playerData.scrap >= upgradeCost)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Upgrade);

            DataManager.Instance.playerData.scrap -= upgradeCost;
            DataManager.Instance.playerData.health += healthUpgradeValue;

            DataManager.Instance.SaveData();
            EventsManager.Instance.UpdateUpgradesHUD();
        }
    }

    /// <summary>
    ///     Function that updates the stats of the player, this will be called from the event manager every time a stat is upgraded
    /// </summary>
    private void UpdatePlayerStats()
    {
        currentSpeedTxt.text = "Speed " + DataManager.Instance.playerData.speed.ToString();
        currentReloadTxt.text = "Reload " + DataManager.Instance.playerData.reloadTime.ToString();
        currentHealthTxt.text = "Health " + DataManager.Instance.playerData.health.ToString();
        currentScrapTxt.text = "Scrap " + DataManager.Instance.playerData.scrap.ToString();
    }
}
