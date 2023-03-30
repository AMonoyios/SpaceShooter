using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public virtual void HidePanel()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

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

    public void ToggleSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);

        DataManager.Instance.playerData.isAudioOn = !DataManager.Instance.playerData.isAudioOn;
        DataManager.Instance.SaveData();

        UpdateSFXIcon();
    }
    public void UpdateSFXIcon()
    {
        sfxImg.sprite = DataManager.Instance.playerData.isAudioOn ? sfxOnImg : sfxOffImg;
    }
}
