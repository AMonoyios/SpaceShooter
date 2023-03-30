using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum GameState
{
    Idle,
    Playing
}

public sealed class WaveManager : MonoSingleton<WaveManager>
{
    public GameState gameState = GameState.Idle;

    [Space(10.0f)]
    public PlayerController player;
    [SerializeField]
    private TextMeshProUGUI guiText;

    [SerializeField]
    private LevelsScriptableObject levelsData;
    private int currentWaveIndex = 0;
    private const float exitLevelTimer = 3.0f;

    private readonly List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        DataManager.Instance.LoadData();

        currentWaveIndex = 0;

        StartCoroutine
        (
            CountdownTimer(levelsData.levels[DataManager.Instance.currentLevel].waves[currentWaveIndex].spawnTimer,
            () => SpawnWave(DataManager.Instance.currentLevel, currentWaveIndex))
        );
    }

    private void SpawnWave(int levelIndex, int waveIndex)
    {
        for (int enemyIndex = 0; enemyIndex < levelsData.levels[levelIndex].waves[waveIndex].enemies.Length; enemyIndex++)
        {
            GameObject enemyGO = Instantiate(levelsData.levels[levelIndex].waves[waveIndex].enemies[enemyIndex]);
            enemies.Add(enemyGO.GetComponent<Enemy>());
        }

        gameState = GameState.Playing;

        HideUIText();
    }

    public void DespawnEnemy(Enemy enemy)
    {
        if (enemies.Remove(enemy))
        {
            Debug.Log($"Removed {enemy.name} from enemies");

            TryProceedToNextWave();

            Destroy(enemy.gameObject);
        }
        else
        {
            Debug.LogError($"{enemy.name} does not exist in enemies");
        }
    }

    private void TryProceedToNextWave()
    {
        Debug.Log("Try Proceed to next wave...");

        if (IsWaveCompleted)
        {
            Debug.Log("Proceed to next wave");

            ShowUIText("Level cleared!");

            if (levelsData.levels[DataManager.Instance.currentLevel].waves.Count > currentWaveIndex + 1)
            {
                currentWaveIndex++;
                Debug.Log($"Entering wave {currentWaveIndex}...");

                StartCoroutine
                (
                    CountdownTimer
                    (
                        levelsData.levels[DataManager.Instance.currentLevel].waves[currentWaveIndex].spawnTimer,
                        () => SpawnWave(DataManager.Instance.currentLevel, currentWaveIndex)
                    )
                );
            }
            else
            {
                Debug.Log("Level cleared!");

                ShowUIText("Cleared level!");
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Completed);
                DataManager.Instance.playerData.completedLevels = DataManager.Instance.currentLevel;
                StartCoroutine(GoToMenu());
            }

            DataManager.Instance.SaveData();
        }
    }

    // public void DespawnEnemy(Enemy enemy)
    // {
    //     if(enemies.Remove(enemy))
    //     {
    //         Debug.Log($"Removed {enemy.name} from enemies");
    //     }

    //     Destroy(enemy.gameObject);
    // }

    // public void DespawnWave(int levelIndex, int waveIndex)
    // {
    //     Debug.Log($"Despawning wave => Level: {levelIndex}, Wave: {waveIndex}");
    //     foreach (Enemy enemy in enemies)
    //     {
    //         DespawnEnemy(enemy);
    //     }
    //     enemies.Clear();
    //     Debug.Log($"Enemies count: {enemies.Count}");
    // }

    // public void CompleteWave(string waveCompletedText = "Wave Completed!")
    // {
    //     guiText.gameObject.SetActive(true);
    //     guiText.text = waveCompletedText;
    // }

    // public void TryProceedToNextWave()
    // {
    //     Debug.Log("Try Proceed to next wave");

    //     if (IsWaveCompleted)
    //     {
    //         Debug.Log("Proceed to next");
    //         DespawnWave(DataManager.Instance.currentLevel, currentWaveIndex);
    //         CompleteWave("Level cleared!");

    //         if (levelsData.levels[DataManager.Instance.currentLevel].waves.Count > currentWaveIndex + 1)
    //         {
    //             currentWaveIndex++;
    //             Debug.Log($"Entering wave {currentWaveIndex}...");

    //             StartCoroutine
    //             (
    //                 CountdownTimer
    //                 (
    //                     levelsData.levels[DataManager.Instance.currentLevel].waves[currentWaveIndex].spawnTimer,
    //                     () => SpawnWave(DataManager.Instance.currentLevel, currentWaveIndex)
    //                 )
    //             );
    //         }
    //         else
    //         {
    //             Debug.Log("Level cleared!");

    //             StartCoroutine(GoToMenu());
    //         }

    //         DataManager.Instance.SaveData();
    //     }
    // }

    public bool IsWaveCompleted
    {
        get
        {
            bool isThereAtLeastOneEnemyAlive = enemies.Any((enemy) => enemy.IsAlive);

            if (isThereAtLeastOneEnemyAlive)
            {
                Debug.Log("There is still one enemy alive.");
                gameState = GameState.Playing;
                return false;
            }

            gameState = GameState.Idle;
            return true;
        }
    }

    public void ShowUIText(string text)
    {
        guiText.gameObject.SetActive(true);
        guiText.text = text;
    }
    public void HideUIText()
    {
        guiText.text = "";
        guiText.gameObject.SetActive(false);
    }

    private IEnumerator CountdownTimer(float duration, System.Action onComplete = null)
    {
        while (duration > 0.0f)
        {
            ShowUIText(Mathf.FloorToInt(duration).ToString());
            yield return new WaitForSeconds(1.0f);
            duration--;
        }

        HideUIText();
        onComplete?.Invoke();
    }

    public IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(exitLevelTimer);

        SceneManager.Instance.LoadScene("Menu");
    }
}
