using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class UILevelsPanel : UIBasePanel
{
    [Header("Buttons")]
    [SerializeField]
    private Button backBtn;

    [Header("Levels")]
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private GameObject levelButtonPrefab;
    [SerializeField]
    private LevelsScriptableObject levels;

    private void Start()
    {
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Debug.Log("Deleting levels child");
            Destroy(contentTransform.GetChild(i));
        }

        for (int levelIndex = 0; levelIndex < levels.levels.Count; levelIndex++)
        {
            LevelIndex level = Instantiate(levelButtonPrefab, contentTransform).GetComponent<LevelIndex>();
            level.Init(levelIndex);
        }
    }

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
