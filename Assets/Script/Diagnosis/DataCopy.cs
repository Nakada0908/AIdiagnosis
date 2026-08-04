using UnityEngine;
using System.Runtime.InteropServices;

public class DataCopy : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    //WebGL用のJavaScript関数をインポート
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);
#endif

    //ボタンが押された時
    public void CopyDiagnosisData()
    {
        string diagnosisText = MakeDiagnosisText();

#if UNITY_WEBGL && !UNITY_EDITOR
        //WebGLビルド実行時の処理
        CopyToClipboard(diagnosisText);
#else
        GUIUtility.systemCopyBuffer = diagnosisText;
#endif

        Debug.Log("クリップボードにコピーしました:\n" + diagnosisText);
    }

    private string MakeDiagnosisText()
    {
        string filePath = "PromptText/";

        string promptHeader = Resources.Load<TextAsset>(filePath + "PromptHeader").text;
        string data = DiagnosisSave.Instance.GetFinalJsonData();
        string promptFormat = Resources.Load<TextAsset>(filePath + "PromptFormat").text;

        string makedata = promptHeader + "\n" + data + "\n" + promptFormat;
        return makedata;
    }
}