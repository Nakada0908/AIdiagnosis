using UnityEngine;

public class NextScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    public void GoNextScene()
    {
        MySceneManager.Instance.ChangeScene(nextSceneName);
    }

    public void OnSelectEnding(string targetSceneName)
    {
        MySceneManager.Instance.ChangeScene(targetSceneName);
    }
}
