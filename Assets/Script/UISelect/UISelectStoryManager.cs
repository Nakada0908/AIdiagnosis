using UnityEngine;

public class UISelectStoryManager : MonoBehaviour
{
    public static UISelectStoryManager Instance;

    [SerializeField] private StoryData[] storyData;
    private int selectDataIndex = 0;
    private int storyIndex = 0;
    private bool finishText = false;

    private InputSystem_Actions input;

    private CurrentState cs;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        input = InputManager.Instance.input;

        if (storyData[selectDataIndex].bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(storyData[selectDataIndex].bgmClip);
        }

        PlayCurrentText();
    }

    private void Update()
    {
        if (input.NovelControls.NextText.WasPressedThisFrame())
        {
            if (cs == CurrentState.Story && finishText)
            {
                finishText = false;
                NextText();
            }
        }
    }

    private void NextText()
    {
        ++storyIndex;
        if (storyIndex < storyData[selectDataIndex].storys.Count)
        {
            PlayCurrentText();
        }
        else if (selectDataIndex < storyData.Length - 1)
        {
            //ボタン選択待機のため待ち
        }
        else
        {
            //今回ここではシーン移動はするけど、UIを全部選択しきったのかの確認をする
            //もうちょい上の方で確認したほうがいいかな？

            //EndBefore,UISelect1
            //ここ何かしらのif文で制御しないと事故りそう
            MySceneManager.Instance.ChangeScene("EndBefore");
        }
    }

    private void PlayCurrentText()
    {
        cs = CurrentState.Story;
        var storyElement = storyData[selectDataIndex].storys[storyIndex];

        if (storyElement.voiceClip != null)
        {
            SoundManager.Instance.PlayVoice(storyElement.voiceClip);
        }
        if (storyElement.seClip != null)
        {
            SoundManager.Instance.PlaySE(storyElement.seClip);
        }

        TextWindowManager.Instance.ShowText(storyElement, OnStoryComplete);
    }

    private void OnStoryComplete()
    {
        var storyElement = storyData[selectDataIndex].storys[storyIndex];

        switch (storyElement.storyType)
        {
            case StoryType.Story:
                cs = CurrentState.Story;
                finishText = true;
                break;
            case StoryType.Choice:
                cs = CurrentState.Choice;
                ChoiceButtonManager.Instance.ONChoiceButton(storyElement.diagnosis, OnComplete);
                break;
            case StoryType.Writing:
                cs = CurrentState.Writing;
                //ストーリーテキストを質問としてセットしておく
                storyElement.diagnosis.question1 = storyElement.storyText;
                WritingManager.Instance.ONInputField(storyElement.diagnosis, OnComplete);
                break;
            case StoryType.JudgeEnding:
                break;
            case StoryType.None:
                break;
        }
    }

    private void OnComplete()
    {
        NextText();
    }

    public void ChangeSelectStoryElent(int index)
    {
        storyIndex = 0;
        selectDataIndex = index;

        if (storyData[selectDataIndex].bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(storyData[selectDataIndex].bgmClip);
        }

        PlayCurrentText();
    }
}