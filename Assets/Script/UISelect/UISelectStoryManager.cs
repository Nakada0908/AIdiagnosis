using UnityEngine;

public class UISelectStoryManager : MonoBehaviour
{
    public static UISelectStoryManager Instance;

    [SerializeField] private StoryData[] storyData;
    private int dataIndex = 0;
    private int storyIndex = 0;
    private bool finishText = false;

    private InputSystem_Actions input;

    private CurrentState cs;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        input = InputManager.Instance.input;
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
                if (ChoiceButtonManager.Instance.finishChoice)
                {
                    ChoiceButtonManager.Instance.OFFEndingButton();
                    NextText();
                }
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
        else if (dataIndex < storyData.Length - 1)
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
        var storyElement = storyData[dataIndex].storys[storyIndex];

        switch (storyElement.storyType)
        {
            case StoryType.Story:
                cs = CurrentState.Story;
                finishText = true;
                break;
            case StoryType.Choice:
                cs = CurrentState.Choice;
                ChoiceButtonManager.Instance.ONChoiceButton(storyElement.diagnosis);
                break;
            case StoryType.Writing:
                cs = CurrentState.Writing;
                //ストーリーテキストを質問としてセットしておく
                storyElement.diagnosis.question1 = storyElement.storyText;
                WritingManager.Instance.ONInputField(storyElement.diagnosis);
                break;
            case StoryType.JudgeEnding:
                cs = CurrentState.End;
                ChoiceButtonManager.Instance.ONEndingButton(storyElement.diagnosis);
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
