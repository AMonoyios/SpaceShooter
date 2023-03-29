using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
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

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Debug.Log("Loading data...");

        playerData.scrap = PlayerPrefs.GetInt("scrap");
        playerData.health = PlayerPrefs.GetInt("health");
        playerData.speed = PlayerPrefs.GetFloat("speed");
        playerData.damage = PlayerPrefs.GetInt("damage");
        playerData.reloadTime = PlayerPrefs.GetFloat("reloadTime");

        currentLevel = PlayerPrefs.GetInt("currentLevel");
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("scrap", 0);
        PlayerPrefs.SetInt("health", 5);
        PlayerPrefs.SetFloat("speed", 150);
        PlayerPrefs.SetInt("damage", 1);
        PlayerPrefs.SetFloat("reloadTime", 1.3f);

        PlayerPrefs.SetInt("currentLevel", currentLevel);

        PlayerPrefs.Save();
    }
}
