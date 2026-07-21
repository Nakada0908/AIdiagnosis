using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class StoryDataEditorWindow : EditorWindow
{
    private StoryData targetData;
    private int selectedIndex = -1;
    private Vector2 leftScrollPos;
    private Vector2 rightScrollPos;

    private float leftPaneWidth = 250f;
    private bool isResizing = false;
    private bool inheritCharacterName = true;
    private bool inheritStoryType = true;

    //UIのテキスト枠に合わせた制限値の定義
    private const int maxLines = 3;
    private const int maxCharsPerLine = 34;
    private const int maxCharsPerQuestionLine = 26;

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

    //外部から対象のアセットを指定して自動アタッチした状態でウィンドウを開く
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
        DrawResizer();
        DrawRightPane();

        EditorGUILayout.EndHorizontal();

        //変更があった場合はアセットの保存フラグを立てる
        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetData);
        }
    }

    private void DrawResizer()
    {
        Rect resizerRect = GUILayoutUtility.GetRect(1f, EditorGUIUtility.currentViewWidth, GUILayout.ExpandHeight(true));

        Rect hitRect = resizerRect;
        hitRect.xMin -= 4f;
        hitRect.xMax += 4f;

        EditorGUIUtility.AddCursorRect(hitRect, MouseCursor.ResizeHorizontal);

        if (Event.current.type == EventType.MouseDown && hitRect.Contains(Event.current.mousePosition))
        {
            isResizing = true;
        }

        if (isResizing)
        {
            leftPaneWidth = Event.current.mousePosition.x;
            leftPaneWidth = Mathf.Clamp(leftPaneWidth, 100f, position.width - 200f);
            Repaint();
        }

        if (Event.current.type == EventType.MouseUp)
        {
            isResizing = false;
        }

        EditorGUI.DrawRect(resizerRect, Color.gray);
    }

    private void DrawLeftPane()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.Width(leftPaneWidth), GUILayout.ExpandHeight(true));

        targetData.bgmClip = (AudioClip)EditorGUILayout.ObjectField("全体のBGM", targetData.bgmClip, typeof(AudioClip), false);
        targetData.useBackground = EditorGUILayout.Toggle("背景を使用する", targetData.useBackground);
        if (targetData.useBackground)
        {
            targetData.backGround = (Sprite)EditorGUILayout.ObjectField("背景画像", targetData.backGround, typeof(Sprite), false);
        }

        EditorGUILayout.Space();
        inheritCharacterName = EditorGUILayout.ToggleLeft("キャラ名を引き継ぐ", inheritCharacterName);
        inheritStoryType = EditorGUILayout.ToggleLeft("進行タイプを引き継ぐ", inheritStoryType);
        EditorGUILayout.Space();

        //リスト操作用のボタン群（末尾追加、選択行の下に追加、削除）
        if (GUILayout.Button("＋ 末尾に追加"))
        {
            Story newStory = new Story();
            if (targetData.storys.Count > 0)
            {
                Story lastStory = targetData.storys[targetData.storys.Count - 1];
                if (inheritCharacterName)
                {
                    newStory.characterName = lastStory.characterName;
                }
                if (inheritStoryType)
                {
                    newStory.storyType = lastStory.storyType;
                }
            }
            targetData.storys.Add(newStory);
            selectedIndex = targetData.storys.Count - 1;
        }
        if (selectedIndex >= 0 && selectedIndex < targetData.storys.Count)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("＋ 下に挿入"))
            {
                Story newStory = new Story();
                Story selectedStory = targetData.storys[selectedIndex];
                if (inheritCharacterName)
                {
                    newStory.characterName = selectedStory.characterName;
                }
                if (inheritStoryType)
                {
                    newStory.storyType = selectedStory.storyType;
                }
                targetData.storys.Insert(selectedIndex + 1, newStory);
                selectedIndex++;
            }
            if (GUILayout.Button("－ 削除"))
            {
                targetData.storys.RemoveAt(selectedIndex);
                selectedIndex = Mathf.Clamp(selectedIndex - 1, -1, targetData.storys.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        //選択中のセリフを上下に入れ替える並べ替えボタン群
        if (selectedIndex >= 0 && selectedIndex < targetData.storys.Count)
        {
            EditorGUILayout.BeginHorizontal();
            if (selectedIndex > 0)
            {
                if (GUILayout.Button("▲ 上へ"))
                {
                    Story temp = targetData.storys[selectedIndex];
                    targetData.storys.RemoveAt(selectedIndex);
                    targetData.storys.Insert(selectedIndex - 1, temp);
                    selectedIndex--;
                }
            }
            if (selectedIndex < targetData.storys.Count - 1)
            {
                if (GUILayout.Button("▼ 下へ"))
                {
                    Story temp = targetData.storys[selectedIndex];
                    targetData.storys.RemoveAt(selectedIndex);
                    targetData.storys.Insert(selectedIndex + 1, temp);
                    selectedIndex++;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();

        //左側にリスト一覧を描画
        float scrollHeight = Mathf.Max(100f, position.height - 160f);
        leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos, GUILayout.Height(scrollHeight));
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
        float scrollHeight = Mathf.Max(100f, position.height - 40f);
        rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos, GUILayout.Height(scrollHeight));

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

            if (lines.Length > maxLines)
            {
                EditorGUILayout.HelpBox($"行数が上限を超えています！ (現在: {lines.Length}行 / 最大: {maxLines}行)", MessageType.Error);
            }

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

        if (story.characterImage == null || story.characterImage.Length != 3)
        {
            System.Array.Resize(ref story.characterImage, 3);
        }

        EditorGUILayout.BeginVertical("helpbox");
        EditorGUILayout.LabelField("キャラクター立ち絵 (画面に出す人物を毎回指定)", EditorStyles.boldLabel);

        story.characterImage[0] = (Sprite)EditorGUILayout.ObjectField(" [1人目] 画像", story.characterImage[0], typeof(Sprite), false);

        if (story.characterImage[0] != null)
        {
            story.characterImage[1] = (Sprite)EditorGUILayout.ObjectField(" [2人目] 画像", story.characterImage[1], typeof(Sprite), false);
        }
        if (story.characterImage[0] != null && story.characterImage[1] != null)
        {
            story.characterImage[2] = (Sprite)EditorGUILayout.ObjectField(" [3人目] 画像", story.characterImage[2], typeof(Sprite), false);
        }
        EditorGUILayout.EndVertical();

        story.voiceClip = (AudioClip)EditorGUILayout.ObjectField("ボイス (Voice)", story.voiceClip, typeof(AudioClip), false);
        story.seClip = (AudioClip)EditorGUILayout.ObjectField("効果音 (SE)", story.seClip, typeof(AudioClip), false);

        //進行タイプに応じて診断データの入力枠を表示
        if (story.storyType == StoryType.Choice || story.storyType == StoryType.JudgeEnding)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.LabelField($"診断・選択肢・エンディングデータ (Diagnosis) [1行あたり最大 {maxCharsPerQuestionLine}文字]", EditorStyles.boldLabel);

            if (story.diagnosis == null)
            {
                story.diagnosis = new Diagnosis();
            }

            EditorGUILayout.LabelField("質問 / 選択肢 1");
            story.diagnosis.question1 = EditorGUILayout.TextArea(story.diagnosis.question1, GUILayout.Height(40));
            CheckQuestionCharLimit(story.diagnosis.question1, "質問 / 選択肢 1");
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("質問 / 選択肢 2");
            story.diagnosis.question2 = EditorGUILayout.TextArea(story.diagnosis.question2, GUILayout.Height(40));
            CheckQuestionCharLimit(story.diagnosis.question2, "質問 / 選択肢 2");
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("質問 / 選択肢 3");
            story.diagnosis.question3 = EditorGUILayout.TextArea(story.diagnosis.question3, GUILayout.Height(40));
            CheckQuestionCharLimit(story.diagnosis.question3, "質問 / 選択肢 3");
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    //質問テキストの各行の文字数をチェックし、超過時にエラーボックスを表示する
    private void CheckQuestionCharLimit(string text, string label)
    {
        if (!string.IsNullOrEmpty(text))
        {
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > maxCharsPerQuestionLine)
                {
                    EditorGUILayout.HelpBox($"{label}の{i + 1}行目の文字数が上限を超えています！ (現在: {lines[i].Length}文字 / 最大: {maxCharsPerQuestionLine}文字)", MessageType.Error);
                }
            }
        }
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
            //Inspectorで選択中の対象をStoryDataにキャストし、自動でアタッチさせて開く
            StoryDataEditorWindow.OpenWindowWithTarget((StoryData)target);
        }
        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}