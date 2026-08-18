using UnityEngine;
using UnityEngine.InputSystem;

public class UIButtonFeedback : MonoBehaviour
{
    [SerializeField] private InputActionReference grab;
    [SerializeField] private GameObject grabHint;
    private void OnEnable()
    {
        grab.action.Enable();
        grab.action.started += _ => grabHint.SetActive(true);
        grab.action.canceled += _ => grabHint.SetActive(false);
    }
}
