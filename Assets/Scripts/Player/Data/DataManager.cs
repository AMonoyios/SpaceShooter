using UnityEngine;

/// <summary>
///     Manager that handles all data of player
/// </summary>
public class DataManager : MonoPersistentSingleton<DataManager>
{
    public DataScriptableObject playerData;
    public int currentLevel = 0;

    public void SaveData()
    {
        Debug.Log("Saving data...");

        PlayerPrefs.SetInt("scrap", playerData.scrap);
        PlayerPrefs.SetInt("health", playerData.health);
        PlayerPrefs.SetFloat("speed", playerData.speed);
        PlayerPrefs.SetInt("damage", playerData.damage);
        PlayerPrefs.SetFloat("reloadTime", playerData.reloadTime);

        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("completedLevels", playerData.completedLevels);

        int isAudioOn_INTVALUE = playerData.isAudioOn ? 1 : 0;
        PlayerPrefs.SetInt("isAudioOn", isAudioOn_INTVALUE);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Debug.Log("Loading data...");

        playerData.scrap = PlayerPrefs.GetInt("scrap", 0);
        playerData.health = PlayerPrefs.GetInt("health", 5);
        playerData.speed = PlayerPrefs.GetFloat("speed", 175);
        playerData.damage = PlayerPrefs.GetInt("damage", 1);
        playerData.reloadTime = PlayerPrefs.GetFloat("reloadTime", 1.3f);

        currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
        playerData.completedLevels = PlayerPrefs.GetInt("completedLevels", -1);

        int isAudioOn_INTVALUE = PlayerPrefs.GetInt("isAudioOn", 1);
        playerData.isAudioOn = isAudioOn_INTVALUE == 1;
    }

    /// <summary>
    ///     Resets all data of player. This is not being used in game, testing purposed only
    /// </summary>
    public void ResetData()
    {
        PlayerPrefs.SetInt("scrap", 0);
        PlayerPrefs.SetInt("health", 5);
        PlayerPrefs.SetFloat("speed", 175);
        PlayerPrefs.SetInt("damage", 1);
        PlayerPrefs.SetFloat("reloadTime", 1.3f);

        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.SetInt("completedLevels", -1);

        PlayerPrefs.SetInt("isAudioOn", 1);

        PlayerPrefs.Save();

        LoadData();
    }
}
