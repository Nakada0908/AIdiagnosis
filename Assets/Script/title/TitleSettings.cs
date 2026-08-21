using UnityEngine;
using UnityEngine.UI;

public class TitleSettings : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject douisyo;

    void Start()
    {
        startButton.interactable = false;
        douisyo.SetActive(true);

        if (bgm != null)
        {
            SoundManager.Instance.PlayBGM(bgm);
        }

        ChoiceButtonManager.Instance.OFFDataCopyButton();
        DiagnosisSave.Instance.ClearData();
    }

    public void DouiButtonClick()
    {
        startButton.interactable = true;
        douisyo.SetActive(false);
    }
}
