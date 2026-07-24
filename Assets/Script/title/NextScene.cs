using UnityEngine;

public class NextScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    public void GoNextScene()
    {
        MySceneManager.Instance.ChangeScene(nextSceneName);
    }
}
