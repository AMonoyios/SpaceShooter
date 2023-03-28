using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    public DataScriptableObject playerData;

    public void SaveData()
    {
        Debug.Log("Saving data...");

        PlayerPrefs.SetInt("scrap", playerData.scrap);
        PlayerPrefs.SetInt("health", playerData.health);
        PlayerPrefs.SetFloat("speed", playerData.speed);
        PlayerPrefs.SetInt("damage", playerData.damage);
        PlayerPrefs.SetFloat("reloadTime", playerData.reloadTime);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Debug.Log("Loading data...");

        playerData.scrap = PlayerPrefs.HasKey("scrap") ? PlayerPrefs.GetInt("scrap") : 0;
        playerData.health = PlayerPrefs.HasKey("health") ? PlayerPrefs.GetInt("health") : 5;
        playerData.speed = PlayerPrefs.HasKey("speed") ? PlayerPrefs.GetFloat("speed") : 100.0f;
        playerData.damage = PlayerPrefs.HasKey("damage") ? PlayerPrefs.GetInt("damage") : 1;
        playerData.reloadTime = PlayerPrefs.HasKey("reloadTime") ? PlayerPrefs.GetFloat("reloadTime") : 1.5f;
    }
}
