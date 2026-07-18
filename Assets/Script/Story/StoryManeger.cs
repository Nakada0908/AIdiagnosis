using UnityEngine;
using UnityEngine.EventSystems;

enum CurrentState
{
    Story,
    Choice,
    Writing,
    WaitSelect,
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
        if (cs != CurrentState.Story)
        {
            return;
        }

        if (UIHoverBlocker.IsHovering)
        {
            return;
        }

        if (finishText && input.NovelControls.NextText.WasPressedThisFrame())
        {
            finishText = false;
            NextText();
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
            MySceneManager.Instance.ChangeScene("UISelect1");
        }
    }

    private void PlayCurrentText()
    {
        cs = CurrentState.Story;
        var storyElement = storyData[dataIndex].storys[storyIndex];

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
                ChoiceButtonManager.Instance.ONChoiceButton(storyElement.diagnosis, NextText);
                break;
            case StoryType.Writing:
                cs = CurrentState.Writing;
                //ストーリーテキストを質問としてセットしておく
                storyElement.diagnosis.question1 = storyElement.storyText;
                WritingManager.Instance.ONInputField(storyElement.diagnosis, NextText);
                break;
            case StoryType.JudgeEnding:
                cs = CurrentState.End;
                ChoiceButtonManager.Instance.ONEndingButton(storyElement.diagnosis, NextText);
                break;
            case StoryType.None:
                break;
        }
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