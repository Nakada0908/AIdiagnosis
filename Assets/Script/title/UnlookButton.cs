using UnityEngine;
using UnityEngine.UI;

public class UnlookButton : MonoBehaviour
{
    private int nowLookEnd = 0;
    private int endingListUnlock = 1;
    private int trueendUnlock = 3;
    [SerializeField] private Button endingListButton;
    [SerializeField] private Button trueendButton;

    private void Start()
    {
        nowLookEnd = UnlockTrueEnd.Instance.GetlookEndCnt();
        endingListButton.interactable = nowLookEnd > endingListUnlock;
        trueendButton.interactable = nowLookEnd > trueendUnlock;
    }
}
