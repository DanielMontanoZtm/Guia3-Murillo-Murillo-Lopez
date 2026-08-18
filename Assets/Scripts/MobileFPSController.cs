using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MobileFPSController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private VirtualJoystick joystick;

    [Header("Velocidades")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravity = -15f;

    [Header("Sensibilidad de Cámara")]
    [SerializeField] private float touchSensitivity = 0.2f;
    [SerializeField] private float mouseSensitivity = 2.0f;

    private CharacterController controller;
    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 verticalVelocity;

    private int lookFingerId = -1;
    private Vector2 lastTouchPos;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            playerCamera = Camera.main;

        yaw = transform.eulerAngles.y;
        if (playerCamera != null)
            pitch = playerCamera.transform.localEulerAngles.x;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        Vector2 lookDelta = Vector2.zero;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);
                if (t.position.x > Screen.width * 0.35f)
                {
                    if (t.phase == TouchPhase.Began && lookFingerId < 0)
                    {
                        lookFingerId = t.fingerId;
                        lastTouchPos = t.position;
                    }
                    else if (t.fingerId == lookFingerId && t.phase == TouchPhase.Moved)
                    {
                        lookDelta = (t.position - lastTouchPos) * touchSensitivity;
                        lastTouchPos = t.position;
                    }
                    else if (t.fingerId == lookFingerId && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                    {
                        lookFingerId = -1;
                    }
                }
            }
        }

        else if (Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width * 0.35f)
        {
            lookDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * 15f;
        }

        if (lookDelta.sqrMagnitude > 0.0001f)
        {
            yaw += lookDelta.x;
            pitch -= lookDelta.y;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.rotation = Quaternion.Euler(0f, yaw, 0f);

            if (playerCamera != null)
                playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (joystick != null && joystick.InputDirection.sqrMagnitude > 0.01f)
        {
            input = joystick.InputDirection;
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        Vector3 move = (transform.forward * input.y + transform.right * input.x);
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}