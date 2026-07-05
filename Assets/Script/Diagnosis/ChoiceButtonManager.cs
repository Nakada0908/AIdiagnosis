using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonManager : MonoBehaviour
{
    public static ChoiceButtonManager Instance;

    public Button[] buttons;

    private Diagnosis diagnosisElement;

    private float timer = 0f;

    public bool finishChoice { get; private set; }

    public End end1 { get; private set; }
    public End end2 { get; private set; }
    private bool isEnd1Choice = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!finishChoice)
        {
            timer += Time.deltaTime;
        }
    }

    public void ONChoiceButton(Diagnosis currentData)
    {
        finishChoice = false;
        diagnosisElement = currentData;
        timer = 0f;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = diagnosisElement.question1;
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = diagnosisElement.question2;
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = diagnosisElement.question3;
    }

    public void OFFChoiceButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            buttons[i].gameObject.SetActive(false);
        }
    }


    #region É{É^ÉìÇ…ÇÊÇÈèàóù
    public void OnChoiceButtonNum(int choiceNumber)
    {
        diagnosisElement.choiceNum = choiceNumber;
        diagnosisElement.answer = "";
        diagnosisElement.answerTime = timer;

        ChoiceWritingOutput.Instance.SaveChoiceToText(diagnosisElement);
        finishChoice = true;
    }

    public void End1_Happy()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.Happy;
        finishChoice = true;
    }
    public void End1_Bad()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.Bad;
        finishChoice = true;
    }
    public void End1_MerryBad()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.MerryBad;
        finishChoice = true;
    }
    
    public void End2_douzyou()
    {
        if(isEnd1Choice == false) { return; }
        end2 = End.douzyou;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
    }
    public void End2_nodouzyou()
    {
        if (isEnd1Choice == false) { return; }
        end2 = End.nodouzyou;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
    }
    public void End2_hannhann()
    {
        if (isEnd1Choice == false) { return; }
        end2 = End.hannhann;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
    }
    #endregion
}