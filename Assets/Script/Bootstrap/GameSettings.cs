using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //スマホで解像度を固定すると、その絵が実機の画面いっぱいに引き伸ばされて歪むのでPCだけにする
        //……特殊なモニターへの対策もしないと
#if !UNITY_ANDROID && !UNITY_IOS
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
#endif

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
