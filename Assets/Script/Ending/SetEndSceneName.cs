using UnityEngine;

public class SetEndSceneName : MonoBehaviour
{
    private string sceneName;

    private void Start()
    {
        sceneName = JudgeEnding.Instance.GetEndSceneName();
        StoryManager sm = GetComponent<StoryManager>();
        sm.SetEndSceneName(sceneName);
    }
}
