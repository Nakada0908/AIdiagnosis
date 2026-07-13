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
        switch (cs)
        {
            case CurrentState.Story:
                if (finishText && input.NovelControls.NextText.WasPressedThisFrame())
                {
                    finishText = false;
                    NextText();
                }
                break;
            case CurrentState.Choice:
                if (ChoiceButtonManager.Instance.finishChoice)
                {
                    ChoiceButtonManager.Instance.OFFChoiceButton();
                    NextText();
                }
                break;
            case CurrentState.Writing:
                if (WritingManager.Instance.finishWriting)
                {
                    NextText();
                }
                break;
            case CurrentState.End:

                break;
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

        //引数にはstoryElementとコールバック関数を渡す
        TextWindowManager.Instance.ShowText(storyElement, OnStoryComplete);
    }

    private void OnStoryComplete()
    {
        var storyElement = storyData[dataIndex].storys[storyIndex];

        //データのStoryTypeを見て、次に何をするか（State）を決定し、1回だけUIを表示する
        switch (storyElement.storyType)
        {
            case StoryType.Story:
                cs = CurrentState.Story;
                //だだのテキスト進行の場合はクリック待機状態にする
                finishText = true;
                break;
            case StoryType.Choice:
                cs = CurrentState.Choice;
                //インライン化されたデータを直接渡す
                ChoiceButtonManager.Instance.ONChoiceButton(storyElement.diagnosis);
                break;
            case StoryType.Writing:
                cs = CurrentState.Writing;
                //インライン化されたデータを直接渡す
                WritingManager.Instance.ONInputField(storyElement.diagnosis);
                break;
            case StoryType.JudgeEnding:
                cs = CurrentState.End;

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