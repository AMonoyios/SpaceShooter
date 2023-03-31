using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/// <summary>
///     Enum state that handles the wave state
/// </summary>
public enum GameState
{
    Idle,
    Playing
}

/// <summary>
///     Manager that handles all logic for waves and levels
/// </summary>
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
        // Loading player data upon loading the wave manager
        DataManager.Instance.LoadData();

        currentWaveIndex = 0;

        // Starting initial wave
        StartCoroutine
        (
            CountdownTimer(levelsData.levels[DataManager.Instance.currentLevel].waves[currentWaveIndex].spawnTimer,
            () => SpawnWave(DataManager.Instance.currentLevel, currentWaveIndex))
        );
    }

    /// <summary>
    ///     Function that spawns all enemies of a specific level/wave
    /// </summary>
    /// <param name="levelIndex">The desired level of the wave</param>
    /// <param name="waveIndex">The desired wave index</param>
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

    /// <summary>
    ///     Despawns specific enemy
    /// </summary>
    /// <param name="enemy">Enemy type reference</param>
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

    /// <summary>
    ///     Function the will check if current wave is completed and then proceed to next wave if exist
    /// </summary>
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

                if (DataManager.Instance.currentLevel >= DataManager.Instance.playerData.completedLevels)
                {
                    DataManager.Instance.playerData.completedLevels = DataManager.Instance.currentLevel;
                }
                StartCoroutine(GoToMenu());
            }

            DataManager.Instance.SaveData();
        }
    }

    /// <summary>
    ///     Returns true if current wave active is completed
    /// </summary>
    private bool IsWaveCompleted
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

    /// <summary>
    ///     Countdown coroutine
    /// </summary>
    /// <param name="duration">The desired time that you want to count down to</param>
    /// <param name="onComplete">An action that you want to perform after countdown is completed</param>
    /// <returns></returns>
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

    /// <summary>
    ///     Coroutine that loads the Main Menu scene
    /// </summary>
    public IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(exitLevelTimer);

        SceneManager.Instance.LoadScene("Menu");
    }
}
