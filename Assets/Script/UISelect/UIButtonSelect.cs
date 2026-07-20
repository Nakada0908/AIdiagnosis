using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSelect : MonoBehaviour
{
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
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}