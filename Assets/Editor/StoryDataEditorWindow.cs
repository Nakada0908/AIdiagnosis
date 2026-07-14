using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class StoryDataEditorWindow : EditorWindow
{
    private StoryData targetData;
    private int selectedIndex = -1;
    private Vector2 leftScrollPos;
    private Vector2 rightScrollPos;

    //UIのテキスト枠に合わせた制限値の定義
    private const int maxLines = 3;
    private const int maxCharsPerLine = 34;

    //メニューの「Window」からエディタを開くための設定
    [MenuItem("Window/Story Editor")]
    public static void OpenWindow()
    {
        GetWindow<StoryDataEditorWindow>("Story Editor");
    }

    //ProjectウィンドウでStoryDataアセットをダブルクリックした際に自動でこのエディタを開く
    [OnOpenAsset(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        StoryData data = EditorUtility.InstanceIDToObject(instanceID) as StoryData;
        if (data != null)
        {
            StoryDataEditorWindow window = GetWindow<StoryDataEditorWindow>("Story Editor");
            window.targetData = data;
            window.Show();
            return true;
        }
        return false;
    }

    // 外部から対象のアセットを指定して自動アタッチした状態でウィンドウを開く
    public static void OpenWindowWithTarget(StoryData data)
    {
        StoryDataEditorWindow window = GetWindow<StoryDataEditorWindow>("Story Editor");
        if (data != null)
        {
            window.targetData = data;
        }
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        //編集対象のScriptableObjectをアタッチするフィールド
        targetData = (StoryData)EditorGUILayout.ObjectField("編集対象のStoryData", targetData, typeof(StoryData), false);

        if (targetData == null)
        {
            EditorGUILayout.HelpBox("編集するStoryDataアセットをセットしてください。", MessageType.Info);
            return;
        }

        //Undo（Ctrl+Z）操作に対応させるための記録
        Undo.RecordObject(targetData, "StoryData Edit");

        EditorGUILayout.BeginHorizontal();

        DrawLeftPane();
        DrawRightPane();

        EditorGUILayout.EndHorizontal();

        //変更があった場合はアセットの保存フラグを立てる
        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetData);
        }
    }

    private void DrawLeftPane()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(250), GUILayout.ExpandHeight(true));

        targetData.bgmClip = (AudioClip)EditorGUILayout.ObjectField("全体のBGM", targetData.bgmClip, typeof(AudioClip), false);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("＋ セリフ追加"))
        {
            targetData.storys.Add(new Story());
            selectedIndex = targetData.storys.Count - 1;
        }
        if (selectedIndex >= 0 && selectedIndex < targetData.storys.Count)
        {
            if (GUILayout.Button("－ 削除"))
            {
                targetData.storys.RemoveAt(selectedIndex);
                selectedIndex = Mathf.Clamp(selectedIndex - 1, -1, targetData.storys.Count - 1);
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //左側にリスト一覧を描画
        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos);
        for (int i = 0; i < targetData.storys.Count; i++)
        {
            Story story = targetData.storys[i];
            string label = $"{i:D2}: " + (string.IsNullOrEmpty(story.characterName) ? "名無し" : story.characterName);
            string textPreview = string.IsNullOrEmpty(story.storyText) ? "" : " - " + story.storyText;
            label += textPreview;

            if (label.Length > 20)
            {
                label = label.Substring(0, 18) + "...";
            }

            GUI.backgroundColor = (i == selectedIndex) ? Color.cyan : Color.white;
            if (GUILayout.Button(label, EditorStyles.toolbarButton, GUILayout.Height(25)))
            {
                selectedIndex = i;
                GUI.FocusControl(null);
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void DrawRightPane()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        if (selectedIndex < 0 || selectedIndex >= targetData.storys.Count)
        {
            EditorGUILayout.HelpBox("左のリストから編集したいセリフを選択してください。", MessageType.Info);
            EditorGUILayout.EndVertical();
            return;
        }

        Story story = targetData.storys[selectedIndex];
        rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos);

        EditorGUILayout.LabelField($"セリフ編集 [Index: {selectedIndex}]", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        story.characterName = EditorGUILayout.TextField("キャラクター名", story.characterName);
        story.storyType = (StoryType)EditorGUILayout.EnumPopup("進行タイプ (StoryType)", story.storyType);

        EditorGUILayout.Space();

        //制限ルールをラベルに表示
        EditorGUILayout.LabelField($"ストーリーテキスト (最大 {maxLines}行 / 1行あたり {maxCharsPerLine}文字):");
        story.storyText = EditorGUILayout.TextArea(story.storyText, GUILayout.Height(100));

        //入力されたテキストの行数と各行の文字数を監視し、超過時にエラーボックスを即時表示する
        if (!string.IsNullOrEmpty(story.storyText))
        {
            string[] lines = story.storyText.Split('\n');

            //行数が上限を超えている場合に赤色の警告メッセージを表示
            if (lines.Length > maxLines)
            {
                EditorGUILayout.HelpBox($"行数が上限を超えています！ (現在: {lines.Length}行 / 最大: {maxLines}行)", MessageType.Error);
            }

            //各行の文字数をチェックし、超過している行番号と文字数を赤色の警告メッセージで表示
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > maxCharsPerLine)
                {
                    EditorGUILayout.HelpBox($"{i + 1}行目の文字数が上限を超えています！ (現在: {lines[i].Length}文字 / 最大: {maxCharsPerLine}文字)", MessageType.Error);
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("画像・サウンド設定", EditorStyles.boldLabel);
        story.characterImage = (Sprite)EditorGUILayout.ObjectField("キャラクター画像", story.characterImage, typeof(Sprite), false);
        story.backGround = (Sprite)EditorGUILayout.ObjectField("背景画像", story.backGround, typeof(Sprite), false);
        story.voiceClip = (AudioClip)EditorGUILayout.ObjectField("ボイス (Voice)", story.voiceClip, typeof(AudioClip), false);
        story.seClip = (AudioClip)EditorGUILayout.ObjectField("効果音 (SE)", story.seClip, typeof(AudioClip), false);

        //進行タイプに応じて診断データの入力枠を表示
        if (story.storyType == StoryType.Choice || story.storyType == StoryType.Writing || story.storyType == StoryType.JudgeEnding)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.LabelField("診断・選択肢・エンディングデータ (Diagnosis)", EditorStyles.boldLabel);

            if (story.diagnosis == null)
            {
                story.diagnosis = new Diagnosis();
            }

            story.diagnosis.question1 = EditorGUILayout.TextField("質問 / 選択肢 1", story.diagnosis.question1);
            story.diagnosis.question2 = EditorGUILayout.TextField("質問 / 選択肢 2", story.diagnosis.question2);
            story.diagnosis.question3 = EditorGUILayout.TextField("質問 / 選択肢 3", story.diagnosis.question3);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}

//StoryDataのInspectorにエディタ起動用のボタンを配置する
[CustomEditor(typeof(StoryData))]
public class StoryDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("専用エディターウィンドウで開く", GUILayout.Height(30)))
        {
            // Inspectorで選択中の対象をStoryDataにキャストし、自動でアタッチさせて開く
            StoryDataEditorWindow.OpenWindowWithTarget((StoryData)target);
        }
        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}