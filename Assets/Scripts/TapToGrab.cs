using UnityEngine;
using UnityEngine.InputSystem;

public class TapToGrab : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputActionReference tapAction;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private Transform selectedObject;

    private void OnEnable()
    {
        tapAction.action.Enable();
        tapAction.action.performed += OnTap;
    }

    private void OnDisable()
    {
        tapAction.action.performed -= OnTap;
    }


    private void OnTap(InputAction.CallbackContext context)
    {
        if (selectedObject == null) TrySelect();
        else ClearSelection();
    }


    private void TrySelect()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
        selectedObject = hit.transform;
        selectedObject.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);
        }

    }

     private void ClearSelection()
    {
        selectedObject.SendMessage("OnDeselected", SendMessageOptions.DontRequireReceiver);
        selectedObject = null;
    }
}
