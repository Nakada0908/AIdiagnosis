using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameDiagnosisData
{
    public List<Diagnosis> records = new List<Diagnosis>();
}

public class ChoiceWritingOutput : MonoBehaviour
{
    public static ChoiceWritingOutput Instance;

    public static GameDiagnosisData currentData = new GameDiagnosisData();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void SaveChoiceToText(Diagnosis diagnosis)
    {
        Diagnosis record = new Diagnosis();

        record.question1 = diagnosis.question1;
        record.question2 = diagnosis.question2;
        record.question3 = diagnosis.question3;
        record.choiceNum = diagnosis.choiceNum;
        record.answer = diagnosis.answer;
        record.answerTime = diagnosis.answerTime;

        currentData.records.Add(record);
    }

    public string GetFinalJsonData()
    {
        return JsonUtility.ToJson(currentData);
    }
}
