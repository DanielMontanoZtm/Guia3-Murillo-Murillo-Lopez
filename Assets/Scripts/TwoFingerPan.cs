using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class TwoFingerPan : MonoBehaviour
{
    [SerializeField] private Transform cameraRig;
    [SerializeField] private float panScale = 0.003f;
    private Vector2 previousCenter;
    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();
    private void Update()
    {
        if (Touch.activeTouches.Count < 2)
        {
            previousCenter = Vector2.zero;
            return;
        }

        Vector2 a = Touch.activeTouches[0].screenPosition;
        Vector2 b = Touch.activeTouches[1].screenPosition;
        Vector2 center = (a + b) * 0.5f;
        if (previousCenter != Vector2.zero)
        {
            Vector2 delta = center - previousCenter;
            Vector3 move = new Vector3(-delta.x, 0f, -delta.y) * panScale;
            cameraRig.Translate(move, Space.Self);
        }
        previousCenter = center;
    }
}