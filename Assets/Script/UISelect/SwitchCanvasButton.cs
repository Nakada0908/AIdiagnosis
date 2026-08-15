using UnityEngine;

public class SwitchCanvasButton : MonoBehaviour
{
    [SerializeField] private GameObject fesCanvas;
    [SerializeField] private GameObject templeCanvas;

    void Start()
    {
        fesCanvas.SetActive(true);
        templeCanvas.SetActive(false);
    }

    public void SwitchCanvas()
    {
        fesCanvas.SetActive(!fesCanvas.activeSelf);
        templeCanvas.SetActive(!templeCanvas.activeSelf);
    }
}