using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonManager : MonoBehaviour
{
    public static ChoiceButtonManager Instance;

    [SerializeField] private Button[] buttons;
    [SerializeField] private Button[] endingButtons;
    [SerializeField] private Button dataCopyButton;

    private Diagnosis diagnosisElement;

    private float timer = 0f;

    public bool finishChoice { get; private set; }

    public End end1 { get; private set; }
    public End end2 { get; private set; }
    private bool isEnd1Choice = false;

    private Action compCollback;

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
        dataCopyButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!finishChoice)
        {
            timer += Time.deltaTime;
        }
    }

    public void ONChoiceButton(Diagnosis diagnosis, Action onComplete)
    {
        finishChoice = false;
        diagnosisElement = diagnosis;
        timer = 0f;

        compCollback = onComplete;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
        }

        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question1;
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question2;
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question3;
    }

    public void OFFChoiceButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void ONEndingButton(Diagnosis diagnosis, Action onComplete)
    {
        finishChoice = false;

        compCollback = onComplete;

        for (int i = 0; i < endingButtons.Length; i++)
        {
            endingButtons[i].gameObject.SetActive(true);
        }

        endingButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question1;
        endingButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question2;
        endingButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = diagnosis.question3;

    }

    public void OFFEndingButton()
    {
        for (int i = 0; i < endingButtons.Length; i++)
        {
            endingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            endingButtons[i].gameObject.SetActive(false);
        }
    }

    public void  ShowDataCopyButton()
    {
        dataCopyButton.gameObject.SetActive(true);
    }

    public void OFFDataCopyButton()
    {
        dataCopyButton.gameObject.SetActive(false);
    }

    #region É{É^ÉìÇ…ÇÊÇÈèàóù
    public void OnChoiceButtonNum(int choiceNumber)
    {
        diagnosisElement.choiceNum = choiceNumber;
        diagnosisElement.answer = "";
        diagnosisElement.answerTime = timer;

        DiagnosisSave.Instance.SaveChoiceToText(diagnosisElement);
        finishChoice = true;
        OFFChoiceButton();
        compCollback?.Invoke();
    }

    public void End1_Happy()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.Happy;
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    public void End1_Bad()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.Bad;
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    public void End1_MerryBad()
    {
        if (isEnd1Choice == true) { return; }
        isEnd1Choice = true;
        end1 = End.MerryBad;
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    
    public void End2_douzyou()
    {
        if(isEnd1Choice == false) { return; }
        end2 = End.douzyou;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    public void End2_nodouzyou()
    {
        if (isEnd1Choice == false) { return; }
        end2 = End.nodouzyou;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    public void End2_hannhann()
    {
        if (isEnd1Choice == false) { return; }
        end2 = End.hannhann;
        JudgeEnding.Instance.SaveEnding(end1, end2);
        finishChoice = true;
        OFFEndingButton();
        compCollback?.Invoke();
    }
    #endregion
}