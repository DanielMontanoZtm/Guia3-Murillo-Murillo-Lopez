using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TwoFingerRotateCube : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float degreesScale = 1f;
    private float previousAngle;
    private bool rotating;
    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();
    private void Update()
    {
        if (Touch.activeTouches.Count < 2)
        {
            rotating = false;
            return;
        }

        Vector2 a = Touch.activeTouches[0].screenPosition;
        Vector2 b = Touch.activeTouches[1].screenPosition;
        Vector2 direction = b - a;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (rotating)
        {
            float deltaAngle = Mathf.DeltaAngle(previousAngle, angle);
            target.Rotate(Vector3.up, deltaAngle * degreesScale, Space.World);
        }
        previousAngle = angle;
        rotating = true;
    }
}