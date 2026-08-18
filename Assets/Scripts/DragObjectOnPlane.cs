using UnityEngine;
using UnityEngine.InputSystem;
public class DragObjectOnPlane : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputActionReference pointerPosition;
    [SerializeField] private InputActionReference press;
    [SerializeField] private LayerMask draggableLayer;
    private Transform draggedObject;
    private Plane dragPlane;
    private Vector3 grabOffset;

    
    private void OnEnable()
    {
        pointerPosition.action.Enable();
        press.action.Enable();
        press.action.started += StartDrag;
        press.action.canceled += EndDrag;
    }

    private void Update()
    {
        if (draggedObject == null) return;
        Ray ray = playerCamera.ScreenPointToRay(pointerPosition.action.ReadValue<Vector2>());
        if (dragPlane.Raycast(ray, out float distance))
        draggedObject.position = ray.GetPoint(distance) + grabOffset;
    }

    private void StartDrag(InputAction.CallbackContext context)
    {
        Ray ray = playerCamera.ScreenPointToRay(pointerPosition.action.ReadValue<Vector2>());
        if (!Physics.Raycast(ray, out RaycastHit hit, 4f, draggableLayer)) return;
        draggedObject = hit.transform;
        dragPlane = new Plane(Vector3.up, hit.point);
        grabOffset = draggedObject.position - hit.point;
    }

    private void EndDrag(InputAction.CallbackContext context)
    {
        draggedObject = null;
    }
}