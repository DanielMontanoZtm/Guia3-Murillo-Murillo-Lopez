using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class PinchDepthControl : MonoBehaviour
{
    [SerializeField] private Transform controlledAnchor;
    [SerializeField] private float minDistance = 0.8f;
    [SerializeField] private float maxDistance = 2.5f;
    [SerializeField] private float sensitivity = 0.004f;
    private float previousDistance;
    private float currentDepth = 1.4f;
    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();
    
    private void Update()
    {
        if (Touch.activeTouches.Count < 2)
        {
            previousDistance = 0f;
            return;
        }


        Vector2 a = Touch.activeTouches[0].screenPosition;
        Vector2 b = Touch.activeTouches[1].screenPosition;
        float distance = Vector2.Distance(a, b);
        if (previousDistance > 0f)
        {
            float delta = distance - previousDistance;
            currentDepth = Mathf.Clamp(currentDepth + delta * sensitivity, minDistance,
            maxDistance);
            controlledAnchor.localPosition = new Vector3(0f, -0.25f, currentDepth);
        }
        previousDistance = distance;
    }
 }