using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;

    private string currentLoadScene = "";
    private bool isLoading = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void InitializeGame(string firstSceneName)
    {
        if (isLoading)
        {
            return;
        }

        StartCoroutine(InitializeRoutine(firstSceneName));
    }

    public void ChangeScene(string sceneName)
    {
        if (isLoading)
        {
            return;
        }

        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator InitializeRoutine(string firstSceneName)
    {
        isLoading = true;

        yield return SceneManager.LoadSceneAsync(firstSceneName, LoadSceneMode.Additive);
        currentLoadScene = firstSceneName;

        isLoading = false;
    }

    private IEnumerator TransitionRoutine(string nextSceneName)
    {
        isLoading = true;

        yield return SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        yield return SceneManager.UnloadSceneAsync(currentLoadScene);
        currentLoadScene = nextSceneName;

        isLoading = false;
    }
}