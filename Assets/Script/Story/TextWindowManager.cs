using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWindowManager : MonoBehaviour
{
    public static TextWindowManager Instance;

    [SerializeField] private Image background;
    [SerializeField] private Image[] characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Image characterNameBG;
    [SerializeField] private TextMeshProUGUI characterName;

    [SerializeField] private Canvas novelCanvas;

    [SerializeField] private Image posOne;
    [SerializeField] private Image[] posTwo;
    [SerializeField] private Image[] posThree;

    private float textSpeed = 0.01f;
    private Coroutine nowCoroutine;
    private bool isSkip;

    private void Awake()
    {
        if(Instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetBuckground(StoryData sd)
    {
        if(sd.useBackground)
        {
            background.enabled = true; 
            if(sd.backGround!=null)
            {
                background.sprite= sd.backGround;
            }
        }
        else
        {
            background.enabled = false;
        }
    }

    public void ShowText(Story story ,Action onComplete)
    {
        if (nowCoroutine != null)
        {
            StopCoroutine(nowCoroutine);
            nowCoroutine = null;
        }
        if (novelCanvas != null)
        {
            novelCanvas.gameObject.SetActive(true);
        }

        if (!string.IsNullOrEmpty(story.characterName))
        {
            characterName.text = story.characterName;
            characterNameBG.enabled = true;
        }
        else
        {
            characterName.text = null;
            characterNameBG.enabled = false;
        }
        SetCharacter(story);
        if (story.voiceClip != null)
        {
            SoundManager.Instance.PlayVoice(story.voiceClip);
        }
        if (story.seClip != null)
        {
            SoundManager.Instance.PlaySE(story.seClip);
        }

        storyText.text = "";
        isSkip = false;

        nowCoroutine = StartCoroutine(TypeSentence(story.storyText, onComplete));
    }

    private void SetCharacter(Story s)
    {
        ResetImages();

        int charaCnt = 0;
        for (int i = 0; i < s.characterImage.Length; i++)
        {
            if (s.characterImage[i] != null)
            {
                charaCnt++;
            }
        }

        switch (charaCnt)
        {
            case 0:
                break;
            case 1:
                posOne.enabled = true;
                posOne.sprite=s.characterImage[0];
                break;
            case 2:
                for(int i=0; i < 2; ++i)
                {
                    posTwo[i].enabled = true;
                    posTwo[i].sprite = s.characterImage[i];
                }
                break;
            case 3:
                for (int i = 0; i <3; ++i)
                {
                    posThree[i].enabled = true;
                    posThree[i].sprite = s.characterImage[i];
                }
                break;
            default:
                break;
        }
    }

    private void ResetImages()
    {
        if (posOne != null)
        {
            posOne.enabled = false;
        }
        for (int i = 0; i < posTwo.Length; i++)
        {
            if (posTwo[i] != null)
            {
                posTwo[i].enabled = false;
            }
        }
        for (int i = 0; i < posThree.Length; i++)
        {
            if (posThree[i] != null)
            {
                posThree[i].enabled = false;
            }
        }
    }

    private IEnumerator TypeSentence(string text, Action onComplete)
    {
        //TMPの機能で文字を一文字ずつ表示する
        storyText.text = text;
        storyText.maxVisibleCharacters = 0;
        for(int i= 0; i < text.Length; i++)
        {
            if (isSkip)
            {
                break;
            }
            storyText.maxVisibleCharacters = i+1;
            yield return new WaitForSeconds(textSpeed);
        }
        //最後に全文表示にする
        storyText.maxVisibleCharacters = text.Length;
        nowCoroutine = null;
        onComplete?.Invoke();
    }

    public void SkipText()
    {
        isSkip = true;
    }

    public void HideText()
    {
        if (novelCanvas != null)
        {
            novelCanvas.gameObject.SetActive(false);
        }
    }
}
