using UnityEngine;

public class DataCopy : MonoBehaviour
{
    //ボタンが押された時
    public void CopyDiagnosisData()
    {
        string diagnosisText = ChoiceWritingOutput.Instance.GetFinalJsonData();

        //Unityの標準機能を使ってクリップボードに直接コピー
        GUIUtility.systemCopyBuffer = diagnosisText;

        Debug.Log("クリップボードにコピーしました:\n" + diagnosisText);
    }
}
