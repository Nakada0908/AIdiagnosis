using UnityEngine;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        InputManager.Instance.SwitchMode(InputMode.Novel);

        MySceneManager.Instance.InitializeGame("Title");
        Destroy(gameObject);
    }
}
