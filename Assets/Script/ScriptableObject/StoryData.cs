using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum End
{
    Happy, Bad, MerryBad,
    douzyou, nodouzyou, hannhann
}

public enum StoryType
{
    Story,
    Choice,
    Writing,
    JudgeEnding,
    None,
}

[CreateAssetMenu(fileName = "StoryData", menuName = "StoryData")]
public class StoryData : ScriptableObject
{
    public List<Story> storys= new List<Story>();
    public AudioClip bgmClip;
}

[System.Serializable]
public class Story
{
    public Sprite backGround;
    public Sprite characterImage;
    [TextArea]
    public string storyText;
    public string characterName;
    public StoryType storyType;
    public Diagnosis diagnosis;
    public AudioClip voiceClip;
    public AudioClip seClip;
}

[System.Serializable]
public class Diagnosis
{
    public string question1;
    public string question2;
    public string question3;
    //以下は今後ハイドインスペクターしようか
    public int choiceNum;
    public string answer;
    public float answerTime;
}