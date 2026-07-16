using UnityEngine;
using UnityEngine.UI;
using System;

public class WritingManager : MonoBehaviour
{
    public static WritingManager Instance;

    [SerializeField] private InputField answerInputField;
    [SerializeField] private Button finishButton;
    private Diagnosis diagnosisElement;

    float timer = 0f;

    public bool finishWriting { get; private set; }

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

    private void Update()
    {
        if (answerInputField.gameObject.activeSelf)
        {
            timer += Time.deltaTime;
        }
    }

    public void ONInputField(Diagnosis currentData, Action onComplete)
    {
        InputManager.Instance.SwitchMode(InputMode.UI);
        finishWriting = false;

        diagnosisElement = currentData;
        timer = 0f;

        compCollback = onComplete;

        answerInputField.gameObject.SetActive(true);
        finishButton.gameObject.SetActive(true);
        answerInputField.text = "";
    }

    public void OFFInputField()
    {
        InputManager.Instance.SwitchMode(InputMode.Novel);
        answerInputField.gameObject.SetActive(false);
        finishButton.gameObject.SetActive(false);
    }

    public void FinishButton()
    {
        diagnosisElement.question2 = "";
        diagnosisElement.question3 = "";
        diagnosisElement.choiceNum = 0;
        diagnosisElement.answer = answerInputField.text;
        diagnosisElement.answerTime = timer;

        DiagnosisSave.Instance.SaveChoiceToText(diagnosisElement);
        OFFInputField();
        finishWriting = true;
        compCollback?.Invoke();
    }
}