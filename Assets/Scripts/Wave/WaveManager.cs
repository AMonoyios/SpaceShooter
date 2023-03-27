using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class WaveManager : MonoSingleton<WaveManager>
{
    private int currentWaveIndex = -1;
    private const float exitLevelTimer = 3.0f;

    [SerializeField]
    private TextMeshProUGUI guiText;

    [SerializeField]
    private Wave[] waves;

    private void Awake()
    {
        currentWaveIndex = 0;

        StartCoroutine(CountdownTimer(3.0f, () => SpawnWave(currentWaveIndex)));
    }

    public void SpawnWave(int waveIndex)
    {
        waves[waveIndex].SpawnEnemies();
    }

    public void TryProceedToNextWave()
    {
        Debug.Log("Try Proceed to next");
        if (waves[currentWaveIndex].IsWaveCompleted)
        {
            ProceedToNextWave();
        }
    }

    public void ProceedToNextWave()
    {
        Debug.Log("Proceed to next");
        if (waves.Length > currentWaveIndex + 1)
        {
            currentWaveIndex++;
            Debug.Log($"Entering wave {currentWaveIndex}...");

            StartCoroutine(CountdownTimer(waves[currentWaveIndex].spawnTimer, () => SpawnWave(currentWaveIndex)));
        }
        else
        {
            Debug.Log("Level cleared!");

            CompleteWave(currentWaveIndex, "Level cleared!");

            StartCoroutine(GoToMenu());
        }
    }

    public void CompleteWave(int waveIndex, string waveCompletedText = "Wave Completed!")
    {
        waves[waveIndex].DespawnEnemies(true);

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
