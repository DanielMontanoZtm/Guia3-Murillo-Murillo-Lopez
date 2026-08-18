using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class SwipeImpulse : MonoBehaviour
{
    [SerializeField] private Rigidbody targetBody;
    [SerializeField] private float minSwipePixels = 100f;
    [SerializeField] private float impulseScale = 0.015f;
    private Vector2 startPosition;
    private bool tracking;
    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();



    private void Update()
    {
        if (Touch.activeTouches.Count == 0) return;
        Touch touch = Touch.activeTouches[0];
        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startPosition = touch.screenPosition;
            tracking = true;
        }
        if (tracking && touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            Vector2 delta = touch.screenPosition - startPosition;
            if (delta.magnitude < minSwipePixels) return;
            Vector3 worldDirection = new Vector3(delta.x, 0f, delta.y).normalized;
            float strength = Mathf.Clamp(delta.magnitude * impulseScale, 1f, 8f);
            targetBody.AddForce(worldDirection * strength, ForceMode.Impulse);
            tracking = false;
        }
    }

}
