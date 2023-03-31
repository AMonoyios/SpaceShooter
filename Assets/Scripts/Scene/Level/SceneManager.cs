using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Manager for all logic about scene changing
/// </summary>
public sealed class SceneManager : MonoSingleton<SceneManager>
{
    [SerializeField]
    private Slider loadingBar;

    /// <summary>
    ///     Loads a scene if exists asynchronously if it exists and then unload the current scene in the background
    /// </summary>
    public async void LoadScene(string sceneName)
    {
        AsyncOperation scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingBar.gameObject.SetActive(true);

        do
        {
            await Task.Delay(100);
            loadingBar.value = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        loadingBar.gameObject.SetActive(false);
    }
}
