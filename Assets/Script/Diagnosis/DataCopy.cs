using UnityEngine;
using System.Runtime.InteropServices;

public class DataCopy : MonoBehaviour
{
    //WebGL用のJavaScript関数をインポート
    [DllImport("__Internal")]
    private static extern void CopyToClipboard(string text);

    //ボタンが押された時
    public void CopyDiagnosisData()
    {
        string diagnosisText = DiagnosisSave.Instance.GetFinalJsonData();

#if UNITY_WEBGL && !UNITY_EDITOR
        //WebGLビルド実行時の処理
        CopyToClipboard(diagnosisText);
#else
        GUIUtility.systemCopyBuffer = diagnosisText;
#endif

        Debug.Log("クリップボードにコピーしました:\n" + diagnosisText);
    }
}