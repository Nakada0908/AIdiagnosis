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
        background.sprite = story.backGround;
        characterImage.sprite = story.characterImage;
        characterName.text = story.characterName;
        //storyText.text = "";//Ç»ÇÒÇ≈Ç±ÇÍÇæÇµÇΩÅH

        StartCoroutine(TypeSentence(story.storyText, onComplete));
    }

    private IEnumerator TypeSentence(string text, Action onComplete)
    {
        foreach (char letter in text.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        onComplete?.Invoke();
    }
}
