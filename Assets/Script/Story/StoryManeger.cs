using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StoryManeger : MonoBehaviour
{
    [SerializeField] private StoryData[] storyDatas;
    [SerializeField] private Diagnosis[] diagnosisDatas;

    [SerializeField] private Image background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    private int storyIndex;
    private int textIndex;

    private bool finishText = false;
    private bool isWaitChoice = false;
    private bool isWaitInput = false;

    private InputSystem_Actions input;

    private void Start()
    {
        input = InputManager.Instance.input;

        storyText.text = "";
        characterName.text = "";
        SetStoryElement(storyIndex, textIndex);
    }

    private void Update()
    {
        if (isWaitChoice && ChoiceButtonManager.Instance.finishChoice)
        {
            isWaitChoice = false;
            ChoiceButtonManager.Instance.OFFChoiceButton();

            NextText();
        }

        if (isWaitInput && WritingManeger.Instance.finishWriting)
        {
            isWaitInput = false;
            NextText();
        }

        if (input.NovelControls.NextText.WasPressedThisFrame() && finishText && !isWaitChoice)
        {
            NextText();
        }
    }

    private void NextText()
    {
        finishText = false;
        ++textIndex;
        storyText.text = "";
        ProgressionStory(storyIndex);
    }

    private void ProgressionStory(int _storyIndex)
    {
        if (textIndex < storyDatas[_storyIndex].storys.Count)
        {
            SetStoryElement(storyIndex, textIndex);
        }
        else
        {
            ChangeStoryElent();
        }
    }

    private void SetStoryElement(int _storyIndex, int _textIndex)
    {
        var storyElement = storyDatas[_storyIndex].storys[_textIndex];

        background.sprite = storyElement.BackGround;
        characterImage.sprite = storyElement.CharacterImage;
        characterName.text = storyElement.CharacterName;
        //ストーリーのテキストを1文字ずつ表示する
        StartCoroutine(TypeSentence(_storyIndex, _textIndex));
    }

    private IEnumerator TypeSentence(int _storyIndex, int _textIndex)
    {
        //1文字ずつ表示する
        foreach (char letter in storyDatas[_storyIndex].storys[_textIndex].StoryText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }

        finishText = true;

        //if (storyDatas[_storyIndex].storys[_textIndex].isChoice)
        //{
        //    isWaitChoice = true;
        //    Diagnosis currentDiagnosis = diagnosisDatas[diagnosesIndex].diagnoses[questionIndex];
        //    ChoiceButtonManager.Instance.ONChoiceButton(currentDiagnosis);
        //    diagnosesIndex++;
        //}

        //if (storyDatas[_storyIndex].storys[_textIndex].isWriting)
        //{
        //    isWaitInput = true;
        //    Diagnosis currentDiagnosis = diagnosisDatas[diagnosesIndex].diagnoses[questionIndex];
        //    WritingManeger.Instance.ONInputField(currentDiagnosis);
        //    diagnosesIndex++;
        //}
    }

    private void ChangeStoryElent()
    {
        textIndex = 0;
        storyIndex++;
        SetStoryElement(storyIndex, textIndex);
    }
}
