using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class WaveManager : MonoSingleton<WaveManager>
{
    // Remove wave from here and get from player prefs the current level and then load that wave
    private const float exitLevelTimer = 3.0f;

    [SerializeField]
    private TextMeshProUGUI guiText;

    [SerializeField]
    private LevelsScriptableObject levelsData;
    private int currentWaveIndex = 0;
    private Wave currentWave;

    private void Awake()
    {
        DataManager.Instance.LoadData();
        StartCoroutine(CountdownTimer(3.0f, () => SpawnWave(currentWaveIndex)));
    }

    public void SpawnWave(int waveIndex)
    {
        currentWave = levelsData.levels[DataManager.Instance.currentLevel].waves[waveIndex];

        currentWave.Init();
        currentWave.SpawnEnemies();
    }

    public void TryProceedToNextWave()
    {
        Debug.Log("Try Proceed to next wave");
        if (currentWave.IsWaveCompleted)
        {
            ProceedToNextWave();
        }
    }

    public void ProceedToNextWave()
    {
        Debug.Log("Proceed to next");
        if (levelsData.levels[DataManager.Instance.currentLevel].waves.Count > currentWaveIndex + 1)
        {
            currentWaveIndex++;
            Debug.Log($"Entering wave {currentWaveIndex}...");

            StartCoroutine(CountdownTimer(currentWave.spawnTimer, () => SpawnWave(currentWaveIndex)));
        }
        else
        {
            Debug.Log("Level cleared!");

            CompleteCurrentWave("Level cleared!");

            StartCoroutine(GoToMenu());
        }

        DataManager.Instance.SaveData();
    }

    public void CompleteCurrentWave(string waveCompletedText = "Wave Completed!")
    {
        currentWave.DespawnEnemies(true);

        guiText.gameObject.SetActive(true);
        guiText.text = waveCompletedText;
    }

    public IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(exitLevelTimer);

        SceneManager.Instance.LoadScene("Menu");
    }

    private IEnumerator CountdownTimer(float duration, System.Action onComplete = null)
    {
        guiText.gameObject.SetActive(true);

        while (duration > 0.0f)
        {
            guiText.text = Mathf.FloorToInt(duration).ToString();
            yield return new WaitForSeconds(1.0f);
            duration--;
        }

        guiText.gameObject.SetActive(false);
        onComplete?.Invoke();
    }
}
