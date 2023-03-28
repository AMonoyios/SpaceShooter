using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIndex : MonoBehaviour
{
    [SerializeField]
    private int index;
    public int Index => index;

    [SerializeField]
    private TextMeshProUGUI levelText;

    public void Init(int index)
    {
        this.index = index;
        levelText.text = index.ToString();
    }
}
