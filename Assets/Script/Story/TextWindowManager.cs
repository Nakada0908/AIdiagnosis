using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextWindowManager : MonoBehaviour
{
    public static TextWindowManager Instance;

    [SerializeField] private Image background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    [SerializeField] private Canvas novelCanvas;

    private Coroutine nowCoroutine;

    private void Awake()
    {
        if(Instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

        if (story.backGround != null)
        {
            background.sprite = story.backGround;
        }
        if (story.characterImage != null)
        {
            characterImage.sprite = story.characterImage;
        }
        if (!string.IsNullOrEmpty(story.characterName))
        {
            characterName.text = story.characterName;
        }

        if (story.voiceClip != null)
        {
            SoundManager.Instance.PlayVoice(story.voiceClip);
        }
        if (story.seClip != null)
        {
            SoundManager.Instance.PlaySE(story.seClip);
        }

        storyText.text = "";

        nowCoroutine = StartCoroutine(TypeSentence(story.storyText, onComplete));
    }

    private IEnumerator TypeSentence(string text, Action onComplete)
    {
        foreach (char letter in text.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        nowCoroutine = null;
        onComplete?.Invoke();
    }

    public void HideText()
    {
        if (novelCanvas != null)
        {
            novelCanvas.gameObject.SetActive(false);
        }
    }
}
