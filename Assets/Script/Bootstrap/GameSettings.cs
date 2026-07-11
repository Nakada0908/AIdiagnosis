using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        InputManager.Instance.SwitchMode(InputMode.Novel);

        //エディターでのテスト時はtitleを起動しないようにする
        string activeSceneName = SceneManager.GetActiveScene().name;

        if (activeSceneName == "Bootstrap")
        {
            MySceneManager.Instance.InitializeGame("Title");
        }

        Destroy(gameObject);
    }
}
