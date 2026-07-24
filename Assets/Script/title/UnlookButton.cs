using UnityEngine;
using UnityEngine.UI;

public class UnlookButton : MonoBehaviour
{
    private int unlook = 3;
    [SerializeField] private Button trueendButton;

    private void Start()
    {
        int nowLookEnd = UnlockTrueEnd.Instance.GetlookEndCnt();
        if (nowLookEnd > unlook)
        {
            trueendButton.gameObject.SetActive(true);
        }
        else
        {
            trueendButton.gameObject.SetActive(false);
        }
    }
}
