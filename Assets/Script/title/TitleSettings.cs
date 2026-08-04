using UnityEngine;
using UnityEngine.UI;

public class TitleSettings : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    void Start()
    {
        if(bgm != null)
        {
            SoundManager.Instance.PlayBGM(bgm);
        }

        ChoiceButtonManager.Instance.OFFDataCopyButton();
        DiagnosisSave.Instance.ClearData();
    }
}
