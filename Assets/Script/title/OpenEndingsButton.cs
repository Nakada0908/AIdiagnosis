using UnityEngine;
using UnityEngine.UI;

public class OpenEndingsButton : MonoBehaviour
{
    [SerializeField] private Canvas endingListCanvas;

    private void Start()
    {
        endingListCanvas.enabled = false;
    }

    public void OnCanvas()
    {
        endingListCanvas.enabled = true;
    }

    public void OFFCanvas()
    {
        endingListCanvas.enabled = false;
    }
}
