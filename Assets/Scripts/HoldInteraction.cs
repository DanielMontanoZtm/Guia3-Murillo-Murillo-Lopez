using UnityEngine;
using UnityEngine.InputSystem;
public class HoldInteraction : MonoBehaviour


{
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private float holdThreshold = 0.45f;
    private float holdStartedAt;
    private bool isHolding;
    private bool interactionActive;

    private void OnEnable()
    {
        holdAction.action.Enable();
        holdAction.action.started += OnPressStarted;
        holdAction.action.canceled += OnPressCanceled;
    }

    private void OnDisable()
    {
        holdAction.action.started -= OnPressStarted;
        holdAction.action.canceled -= OnPressCanceled;
    }

    private void OnPressStarted(InputAction.CallbackContext context)
    {
        holdStartedAt = Time.time;
        isHolding = true;
    }

    private void Update()
    {
        if (!isHolding || interactionActive) return;
        if (Time.time - holdStartedAt >= holdThreshold)
        BeginHeldInteraction();
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
            isHolding = false;
            if (interactionActive) EndHeldInteraction();
    }

    private void BeginHeldInteraction()
    {
     interactionActive = true;
     SendMessage("OnHoldStarted", SendMessageOptions.DontRequireReceiver);
    }

    private void EndHeldInteraction()
    {
        interactionActive = false;
        SendMessage("OnHoldEnded", SendMessageOptions.DontRequireReceiver);
    }
}