using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartNovel", menuName = "StoryData")]
public class StoryData : ScriptableObject
{
    public List<Story> storys= new List<Story>();
    public AudioClip bgmClip;
}

[System.Serializable]
public class Story
{
    public Sprite BackGround;
    public Sprite CharacterImage;
    [TextArea]
    public string StoryText;
    public string CharacterName;
    public bool isChoice;
    public bool isWriting;
    public AudioClip seClip;
}