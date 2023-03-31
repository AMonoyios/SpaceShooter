using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     This is used as a base for all panels. It can also be used a standalone panel.
/// </summary>
public class UIBasePanel : MonoBehaviour
{
    [Header("Base Panel properties")]
    [SerializeField]
    private Button sfxBtn;
    [SerializeField]
    private Image sfxImg;
    [SerializeField]
    private Sprite sfxOnImg;
    [SerializeField]
    private Sprite sfxOffImg;

    /// <summary>
    ///     Closes the panel if not already closed
    /// </summary>
    public virtual void HidePanel()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    ///     Opens the panel if not already open
    /// </summary>
    public virtual void ShowPanel()
    {
        if (gameObject.activeSelf)
        {
            return;
        }

        UpdateSFXIcon();
        gameObject.SetActive(true);
    }

    public virtual void Awake()
    {
        sfxBtn.onClick.AddListener(ToggleSFX);
        UpdateSFXIcon();
    }

    /// <summary>
    ///     Logic that controls the toggle of music
    /// </summary>
    public void ToggleSFX()
    {
        DataManager.Instance.playerData.isAudioOn = !DataManager.Instance.playerData.isAudioOn;
        DataManager.Instance.SaveData();

        if (DataManager.Instance.playerData.isAudioOn)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Music, isLooping: true);
        }
        else
        {
            SoundManager.Instance.StopAll();
        }

        UpdateSFXIcon();
    }
    public void UpdateSFXIcon()
    {
        sfxImg.sprite = DataManager.Instance.playerData.isAudioOn ? sfxOnImg : sfxOffImg;
    }
}
