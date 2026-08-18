using UnityEngine;
using UnityEngine.UI;

public class FPSGrabSystem : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float grabDistance = 10.0f; // Distancia máxima para alcanzar la caja
    [SerializeField] private float holdDistance = 4.0f;  // Distancia caja una vez agarrada
    [SerializeField] private float followSpeed = 25.0f;  // Velocidad mientras está agarrada

    [Header("Feedback Visual")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color targetColor = new Color(0.2f, 1f, 0.2f, 1f);

    private Rigidbody heldObject;
    private Collider heldCollider;
    private CharacterController playerController;

    public bool IsHolding => heldObject != null;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();

        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null) playerCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (heldObject != null && playerCamera != null)
        {
            // Calcular el punto exacto a 'holdDistance' metros enfrente de la cámara
            Vector3 targetPosition = playerCamera.transform.position + (playerCamera.transform.forward * holdDistance);

            Vector3 directionToPoint = targetPosition - heldObject.position;
            heldObject.linearVelocity = directionToPoint * followSpeed;
            heldObject.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        UpdateCrosshairFeedback();

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            ToggleGrab();
        }
    }

    private void UpdateCrosshairFeedback()
    {
        if (crosshairImage == null || playerCamera == null) return;

        if (heldObject != null)
        {
            crosshairImage.color = targetColor;
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            if (hit.collider.GetComponentInParent<Rigidbody>() != null)
            {
                crosshairImage.color = targetColor;
                return;
            }
        }
        crosshairImage.color = normalColor;
    }

    public void ToggleGrab()
    {
        if (heldObject == null) TryGrab();
        else DropObject();
    }

    private void TryGrab()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
        {
            Rigidbody rb = hit.collider.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                heldObject = rb;
                heldCollider = hit.collider;

                // Configurar física de agarre
                heldObject.isKinematic = false; 
                heldObject.useGravity = false;
                heldObject.collisionDetectionMode = CollisionDetectionMode.Continuous;


                Vector3 startHoldPosition = playerCamera.transform.position + (playerCamera.transform.forward * holdDistance);
                heldObject.transform.position = startHoldPosition;

                // Ignorar colisiones entre la cápsula del Player y la caja
                if (playerController != null && heldCollider != null)
                {
                    Physics.IgnoreCollision(playerController, heldCollider, true);
                }
            }
        }
    }

    public void DropObject()
    {
        if (heldObject != null)
        {
            if (playerController != null && heldCollider != null)
            {
                Physics.IgnoreCollision(playerController, heldCollider, false);
            }

            heldObject.useGravity = true;
            heldObject.linearVelocity = Vector3.zero;

            heldObject = null;
            heldCollider = null;
        }
    }
}