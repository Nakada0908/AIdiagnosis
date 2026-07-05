using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleSettings : MonoBehaviour
{
    private InputSystem_Actions input;

    [SerializeField] private AudioClip bgm;

    void Start()
    {
        input = InputManager.Instance.input;
        if(bgm != null)
        {
            SoundManager.Instance.PlayBGM(bgm);
        }
    }

    void Update()
    {
        if (input.NovelControls.NextText.WasPressedThisFrame())
        {
            MySceneManager.Instance.ChangeScene("Novel");
        }
    }
}
