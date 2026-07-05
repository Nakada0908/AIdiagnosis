using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    Novel,
    UI
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputSystem_Actions input { get; private set; }
    private InputActionMap nowinput;

    //シーン読み込み前に自動で1回だけ実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        GameObject managerObj = new GameObject("InputManager");
        managerObj.AddComponent<InputManager>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        SwitchMode(InputMode.Novel);
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void OnDestroy()
    {
        input.Dispose();
    }

    public void SwitchMode(InputMode mode)
    {
        switch (mode)
        {
            case InputMode.Novel:
                SwitchActionMap(input.NovelControls);
                break;
            case InputMode.UI:
                SwitchActionMap(input.UI);
                break;
        }
    }

    public void SwitchActionMap(InputActionMap newMap)
    {
        if (nowinput == newMap){ return; }

        if (nowinput != null)
        {
            nowinput.Disable();
        }

        nowinput = newMap;
        nowinput.Enable();
    }
}