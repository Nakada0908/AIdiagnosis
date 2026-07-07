using UnityEngine;
using UnityEngine.UI;

public class WritingManeger : MonoBehaviour
{
    public static WritingManeger Instance;

    [SerializeField] private InputField answerInputField;
    [SerializeField] private Button finishButton;
    private Diagnosis diagnosisElement;

    float timer = 0f;

    public bool finishWriting { get; private set; }

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

    public void ONInputField(Diagnosis currentData)
    {
        InputManager.Instance.SwitchMode(InputMode.UI);
        finishWriting = false;

        diagnosisElement = currentData;
        timer = 0f;

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
        diagnosisElement.choiceNum = 0;
        diagnosisElement.answer = answerInputField.text;
        diagnosisElement.answerTime = timer;

        DiagnosisSave.Instance.SaveChoiceToText(diagnosisElement);
        OFFInputField();
        finishWriting = true;
    }
}