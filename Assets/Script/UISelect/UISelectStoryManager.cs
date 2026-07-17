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

    private int selectCnt = 0;
    private int selectMaxCnt;

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

        selectMaxCnt = storyData.Length;

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
        if (selectCnt >= selectMaxCnt)
        {
            MySceneManager.Instance.ChangeScene("UISelect2");
            return;
        }

        ++storyIndex;
        if (storyIndex < storyData[selectDataIndex].storys.Count)
        {
            PlayCurrentText();
        }
        else
        {
            TextWindowManager.Instance.HideText();
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
                ChoiceButtonManager.Instance.ONChoiceButton(storyElement.diagnosis, NextText);
                break;
            case StoryType.Writing:
                cs = CurrentState.Writing;
                //ストーリーテキストを質問としてセットしておく
                storyElement.diagnosis.question1 = storyElement.storyText;
                WritingManager.Instance.ONInputField(storyElement.diagnosis, NextText);
                break;
            case StoryType.JudgeEnding:
                break;
            case StoryType.None:
                break;
        }
    }

    public void ChangeSelectStoryElent(int index)
    {
        storyIndex = 0;
        selectDataIndex = index;
        selectCnt++;

        if (storyData[selectDataIndex].bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(storyData[selectDataIndex].bgmClip);
        }

        PlayCurrentText();
    }
}