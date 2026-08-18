using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class TwoFingerChord : MonoBehaviour
{
    [SerializeField] private float maxStartWindow = 0.18f;
    private float firstTouchTime = -1f;
    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();
    private void Update()
    {
        if (Touch.activeTouches.Count == 0)
        {
            firstTouchTime = -1f;
            return;
        }
        if (Touch.activeTouches.Count == 1 && firstTouchTime < 0f)
        firstTouchTime = Time.time;
        if (Touch.activeTouches.Count >= 2 && Time.time - firstTouchTime <= maxStartWindow)
        SendMessage("OnTwoFingerChord", SendMessageOptions.DontRequireReceiver);
    }
}