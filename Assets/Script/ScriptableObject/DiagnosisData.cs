using UnityEngine;
using System.Collections.Generic;

public enum End
{
    Happy,Bad,MerryBad,
    douzyou,nodouzyou,hannhann
}

[CreateAssetMenu(fileName = "DiagnosisData", menuName = "DiagnosisData")]
public class DiagnosisData : ScriptableObject
{
    public List<Diagnosis> diagnoses = new List<Diagnosis>();
}

[System.Serializable]
public class Diagnosis
{
    public string question1;
    public string question2;
    public string question3;
    public int choiceNum;

    public string answer;
    public float answerTime;
}
