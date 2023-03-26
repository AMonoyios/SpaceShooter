using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class WaveManager : MonoBehaviour
{
    private enum WaveStatus
    {
        Idle,
        Preparing,
        Active
    }
    private WaveStatus waveStatus = WaveStatus.Idle;

    [SerializeField]
    private int currentWaveActive = -1;

    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private Button spawnButton;

    private void Start()
    {
        spawnButton.onClick.AddListener(StartWave);
    }

    private void StartWave()
    {
        currentWaveActive = 0;
        waveStatus = WaveStatus.Preparing;

        spawnButton.interactable = false;
        spawnButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (waveStatus)
        {
            case WaveStatus.Idle:
            {
                break;
            }
            case WaveStatus.Preparing:
            {
                if (waves[currentWaveActive].spawnTimer >= 0.0f)
                {
                    waves[currentWaveActive].spawnTimer -= Time.deltaTime;
                    if (waves[currentWaveActive].spawnTimer < 0.0f)
                    {
                        Debug.Log($"Spawning wave {currentWaveActive}");
                        waves[currentWaveActive].SpawnEnemies();

                        waveStatus = WaveStatus.Active;
                    }
                }

                break;
            }
            case WaveStatus.Active:
            {
                if (waves[currentWaveActive].IsWaveCompleted)
                {
                    if (waves.Length > currentWaveActive + 1)
                    {
                        Debug.Log($"Entering wave {currentWaveActive}...");
                        currentWaveActive++;

                        waveStatus = WaveStatus.Preparing;
                    }
                    else
                    {
                        Debug.Log("All waves completed!");

                        waveStatus = WaveStatus.Idle;
                    }
                }

                break;
            }
        }
    }
}
