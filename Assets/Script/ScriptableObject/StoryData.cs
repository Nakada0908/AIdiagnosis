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
    public bool useBackground = false;
    public Sprite backGround;
    public AudioClip bgmClip;
}

[System.Serializable]
public class Story
{
    public Sprite[] characterImage;
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
    [TextArea] public string question1;
    [TextArea] public string question2;
    [TextArea] public string question3;
    [HideInInspector] public int choiceNum;
    [HideInInspector] public string answer;
    [HideInInspector] public float answerTime;
}