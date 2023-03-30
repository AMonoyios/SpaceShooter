using UnityEngine;

public class DataManager : MonoPersistentSingleton<DataManager>
{
    public DataScriptableObject playerData;
    public int currentLevel;

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

        playerData.scrap = PlayerPrefs.GetInt("scrap", playerData.scrap);
        playerData.health = PlayerPrefs.GetInt("health", playerData.health);
        playerData.speed = PlayerPrefs.GetFloat("speed", playerData.speed);
        playerData.damage = PlayerPrefs.GetInt("damage", playerData.damage);
        playerData.reloadTime = PlayerPrefs.GetFloat("reloadTime", playerData.reloadTime);

        currentLevel = PlayerPrefs.GetInt("currentLevel", currentLevel);
        playerData.completedLevels = PlayerPrefs.GetInt("completedLevels", playerData.completedLevels);

        int isAudioOn_INTVALUE = PlayerPrefs.GetInt("isAudioOn", playerData.isAudioOn ? 1 : 0);
        playerData.isAudioOn = isAudioOn_INTVALUE == 1;
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("scrap", 0);
        PlayerPrefs.SetInt("health", 5);
        PlayerPrefs.SetFloat("speed", 150);
        PlayerPrefs.SetInt("damage", 1);
        PlayerPrefs.SetFloat("reloadTime", 1.3f);

        PlayerPrefs.SetInt("currentLevel", 0);
        PlayerPrefs.SetInt("completedLevels", -1);

        PlayerPrefs.SetInt("isAudioOn", 1);

        PlayerPrefs.Save();

        LoadData();
    }
}
