using UnityEngine;

enum CurrentState
{
    Story,
    Choice,
    Writing,
    End
}

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryData[] storyData;
    private int dataIndex = 0;
    private int storyIndex = 0;
    private bool finishText = false;

    private InputSystem_Actions input;

    private CurrentState cs;

    private void Start()
    {
        input = InputManager.Instance.input;

        if (storyData[dataIndex].bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(storyData[dataIndex].bgmClip);
        }

        PlayCurrentText();
    }

    private void Update()
    {
        if(input.NovelControls.NextText.WasPressedThisFrame())
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
        if (storyIndex < storyData[dataIndex].storys.Count)
        {
            PlayCurrentText();
        }
        else if(dataIndex < storyData.Length - 1)
        {
            ChangeStoryElent();
        }
        else
        {
            //EndBefore,UISelect1
            //ここ何かしらのif文で制御しないと事故りそう
            MySceneManager.Instance.ChangeScene("EndBefore");
        }
    }

    private void PlayCurrentText()
    {
        cs = CurrentState.Story;
        var storyElement = storyData[dataIndex].storys[storyIndex];

        if(storyElement.voiceClip != null)
        {
            SoundManager.Instance.PlayVoice(storyElement.voiceClip);
        }
        if(storyElement.seClip != null)
        {
            SoundManager.Instance.PlaySE(storyElement.seClip);
        }

        TextWindowManager.Instance.ShowText(storyElement, OnStoryComplete);
    }

    private void OnStoryComplete()
    {
        var storyElement = storyData[dataIndex].storys[storyIndex];

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
                cs = CurrentState.End;
                ChoiceButtonManager.Instance.ONEndingButton(storyElement.diagnosis, OnComplete);
                break;
            case StoryType.None:
                break;
        }
    }

    private void OnComplete()
    {
        NextText();
    }

    private void ChangeStoryElent()
    {
        storyIndex = 0;
        dataIndex++;

        if (storyData[dataIndex].bgmClip != null)
        {
            SoundManager.Instance.PlayBGM(storyData[dataIndex].bgmClip);
        }

        PlayCurrentText();
    }
}