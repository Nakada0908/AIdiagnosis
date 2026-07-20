using UnityEngine;

public class JudgeEnding : MonoBehaviour
{
    public static JudgeEnding Instance;

    private string endSceneName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //エンディングをテキストに保存する関数
    public void SaveEnding(End end1,End end2)
    {
        endSceneName = EndNameJudge(end1,end2);
        Debug.Log("エンディング決定: " + endSceneName);
    }

    public string GetEndSceneName()
    {
        return endSceneName;
    }

    private string EndNameJudge(End end1, End end2)
    {
        switch (end1)
        {
            case End.Happy:
                switch (end2)
                {
                    case End.douzyou:
                        return "Ending_Hd";
                    case End.nodouzyou:
                        return "Ending_Hn";
                    case End.hannhann:
                        return "Ending_Hh";
                }
                break;

            case End.Bad:
                switch (end2)
                {
                    case End.douzyou:
                        return "Ending_Bd";
                    case End.nodouzyou:
                        return "Ending_Bn";
                    case End.hannhann:
                        return "Ending_Bh";
                }
                break;

            case End.MerryBad:
                switch (end2)
                {
                    case End.douzyou:
                        return "Ending_Md";
                    case End.nodouzyou:
                        return "Ending_Mn";
                    case End.hannhann:
                        return "Ending_Mh";
                }
                break;
        }
        //どの条件にも当てはまらない場合
        return "Title";
    }
}
