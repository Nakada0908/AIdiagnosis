using UnityEngine;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField] private GameObject fesCanvas;
    [SerializeField] private GameObject templeCanvas;

    void Start()
    {
        templeCanvas.SetActive(false);
    }

    public void SwitchCanvas()
    {
        fesCanvas.SetActive(!fesCanvas.activeSelf);
        templeCanvas.SetActive(!templeCanvas.activeSelf);
    }
}