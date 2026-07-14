using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;

    private string currentLoadScene = "";
    private bool isLoading = false;

    private List<string> baseScenes = new List<string>
    {
        "Bootstrap",
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene crrnetEditorScene = SceneManager.GetSceneAt(i);
            //常駐シーンリストに含まれていないシーンを現在のメインシーンとして認識する
            if (!baseScenes.Contains(crrnetEditorScene.name))
            {
                currentLoadScene = crrnetEditorScene.name;
                break;
            }
        }
    }

    public void InitializeGame(string firstSceneName)
    {
        if (isLoading)
        {
            return;
        }

        if(SceneManager.GetSceneByName(firstSceneName).isLoaded)
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

        if (SceneManager.GetSceneByName(sceneName).isLoaded)
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