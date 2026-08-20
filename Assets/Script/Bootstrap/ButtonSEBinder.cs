using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 読み込まれたシーンの Button を全部拾って、クリックSEを一括で仕込む。
/// Bootstrap の常駐オブジェクトに付けておけば、以降に読み込まれるシーンも自動で対象になる。
/// </summary>
public class ButtonSEBinder : MonoBehaviour
{
    [SerializeField] private AudioClip clickSE;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        //自分がいるシーン(Bootstrap)は sceneLoaded を購読する前に読み込み済みなので、ここで登録する
        BindScene(gameObject.scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindScene(scene);
    }

    private void BindScene(Scene scene)
    {
        if (clickSE == null || !scene.IsValid())
        {
            return;
        }

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            //切り替えで非表示にしているCanvas(UISelectなど)の中のボタンも対象にする
            foreach (Button button in root.GetComponentsInChildren<Button>(true))
            {
                button.onClick.AddListener(PlayClickSE);
            }
        }
    }

    private void PlayClickSE()
    {
        SoundManager.Instance.PlaySE(clickSE);
    }
}
