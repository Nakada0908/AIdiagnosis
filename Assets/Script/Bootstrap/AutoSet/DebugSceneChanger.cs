using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugSceneChanger : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        //Bootstrapシーンが読み込まれていなければ加算ロードする
        const string BOOTSTRAP_SCENE = "Bootstrap";
        bool isBootstrapLoaded = false;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == BOOTSTRAP_SCENE)
            {
                isBootstrapLoaded = true;
                break;
            }
        }

        if (!isBootstrapLoaded)
        {
            SceneManager.LoadScene(BOOTSTRAP_SCENE, LoadSceneMode.Additive);
        }

        //キー入力を常時監視するデバッグ用オブジェクトを動的に生成する
        GameObject debugObj = new GameObject("DebugSceneBootstrapper");
        debugObj.AddComponent<DebugSceneChanger>();
        DontDestroyOnLoad(debugObj);
    }

    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("Title");
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("StartNovel");
        }
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("Novel");
        }
        if (Keyboard.current.f4Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("UISelect_1");
        }
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("UISelect_2");
        }
        if (Keyboard.current.f12Key.wasPressedThisFrame)
        {
            MySceneManager.Instance.ChangeScene("EndBefore");
        }
    }
}