using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsHovering { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovering = false;
    }

    private void OnDisable()
    {
        IsHovering = false;
    }
}