using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class SceneManager : MonoSingleton<SceneManager>
{
    [SerializeField]
    private Slider loadingBar;

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
