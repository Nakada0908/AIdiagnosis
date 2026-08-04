//このコードは生成AIによって生成されました
//一括で使用フォントを差し替えるエディタ拡張ツールです

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public class ChangeTMPFontTool : EditorWindow
{
    //差し替え先のフォントアセット
    private TMP_FontAsset newFont;

    [MenuItem("Tools/Change All TMP Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ChangeTMPFontTool>("TMP Font Changer");
    }

    private void OnGUI()
    {
        //フォントアセットを入力するフィールド
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Change in Active Scene"))
        {
            ChangeFontsInActiveScene();
        }

        if (GUILayout.Button("Change in All Scenes"))
        {
            ChangeFontsInAllScenes();
        }
    }

    private void ChangeFontsInActiveScene()
    {
        if (newFont == null)
        {
            Debug.LogWarning("Font Asset is not selected.");
            return;
        }

        //アクティブなシーンの全TMP_Textを取得
        TMP_Text[] textComponents = Resources.FindObjectsOfTypeAll<TMP_Text>();

        foreach (TMP_Text textComponent in textComponents)
        {
            if (textComponent.gameObject.scene.isLoaded)
            {
                //Undo操作を登録
                Undo.RecordObject(textComponent, "Change TMP Font");
                //フォントを差し替え
                textComponent.font = newFont;
                //変更をマーク
                EditorUtility.SetDirty(textComponent);
            }
        }

        Debug.Log("Active Scene replacement completed.");
    }

    private void ChangeFontsInAllScenes()
    {
        if (newFont == null)
        {
            Debug.LogWarning("Font Asset is not selected.");
            return;
        }

        //現在のシーンの未保存の変更を確認して保存処理
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        //元のシーンのパスを保持
        string originalScenePath = EditorSceneManager.GetActiveScene().path;

        //プロジェクト内の全シーンのGUIDを取得
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new string[] { "Assets" });

        foreach (string guid in sceneGuids)
        {
            //GUIDからアセットパスを取得
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);

            //シーンを開く
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(scenePath);

            //シーン内のルートオブジェクトを取得
            GameObject[] rootObjects = scene.GetRootGameObjects();
            bool isModified = false;

            foreach (GameObject root in rootObjects)
            {
                //非アクティブなオブジェクトも含めてTMP_Textを取得
                TMP_Text[] textComponents = root.GetComponentsInChildren<TMP_Text>(true);

                foreach (TMP_Text textComponent in textComponents)
                {
                    Undo.RecordObject(textComponent, "Change TMP Font");
                    textComponent.font = newFont;
                    EditorUtility.SetDirty(textComponent);
                    isModified = true;
                }
            }

            if (isModified)
            {
                //変更があった場合のみシーンを保存
                EditorSceneManager.SaveScene(scene);
            }
        }

        //元のシーンを開き直す
        if (!string.IsNullOrEmpty(originalScenePath))
        {
            EditorSceneManager.OpenScene(originalScenePath);
        }

        Debug.Log("All Scenes replacement completed.");
    }
}