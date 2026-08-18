using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Componentes")]
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 60f;

    public Vector2 InputDirection { get; private set; }
    private RectTransform baseRect;

    private void Awake()
    {
        baseRect = GetComponent<RectTransform>();
        if (baseRect != null)
        {
            baseRect.pivot = new Vector2(0.5f, 0.5f);
        }

        if (handle == null && transform.childCount > 0)
        {
            handle = transform.GetChild(0) as RectTransform;
        }

        if (handle != null)
        {
            handle.pivot = new Vector2(0.5f, 0.5f);
            handle.anchoredPosition = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (baseRect == null) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseRect, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint))
        {
            localPoint = Vector2.ClampMagnitude(localPoint, handleRange);

            if (handle != null)
            {
                handle.anchoredPosition = localPoint;
            }

            InputDirection = localPoint / handleRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }
}