using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSelect : MonoBehaviour
{
    [SerializeField] private Vector3 defaultButtonScale = Vector3.one;

    private StoryManager storyManager;

    private void Start()
    {
        storyManager = GetComponent<StoryManager>();
    }

    public void ChoiceUIButton(int index)
    {
        if (storyManager != null)
        {
            storyManager.ChangeSelectStoryElent(index);
        }

        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj != null)
        {
            Button targetButton = clickedObj.GetComponent<Button>();
            if (targetButton != null)
            {
                targetButton.interactable = false;
            }

            //動いている演出を止めて、元のサイズに戻す
            DOTween.Kill(clickedObj);
            clickedObj.transform.localScale = defaultButtonScale;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}