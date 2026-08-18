using UnityEngine;
using UnityEngine.UI;

public class FPSGrabSystem : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float grabDistance = 5.0f;

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

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Crear HoldPoint a una distancia segura si no existe
        if (holdPoint == null && playerCamera != null)
        {
            GameObject hp = new GameObject("HoldPoint");
            hp.transform.SetParent(playerCamera.transform);
            hp.transform.localPosition = new Vector3(0f, -0.3f, 2.2f); // Distancia cómoda
            hp.transform.localRotation = Quaternion.identity;
            holdPoint = hp.transform;
        }
    }

    private void Update()
    {
        UpdateCrosshairFeedback();

        // Atajo para PC
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            ToggleGrab();
        }

        // Mantener la caja flotando suavemente en el HoldPoint
        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
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
        if (heldObject == null)
        {
            TryGrab();
        }
        else
        {
            DropObject();
        }
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

                // 1. Apagar velocidades residuales para que no salga disparada
                heldObject.linearVelocity = Vector3.zero;
                heldObject.angularVelocity = Vector3.zero;
                heldObject.isKinematic = true;
                heldObject.useGravity = false;

                // 2. Ignorar colisión entre el jugador y la caja mientras se sostiene
                if (playerController != null && heldCollider != null)
                {
                    Physics.IgnoreCollision(playerController, heldCollider, true);
                }

                Debug.Log("📦 Agarrada: " + heldObject.name);
            }
        }
    }

    public void DropObject()
    {
        if (heldObject != null)
        {
            // Reactivar colisión con el jugador
            if (playerController != null && heldCollider != null)
            {
                Physics.IgnoreCollision(playerController, heldCollider, false);
            }

            // Reactivar física y gravedad
            heldObject.isKinematic = false;
            heldObject.useGravity = true;
            heldObject.linearVelocity = Vector3.zero; // Soltarla quieta

            Debug.Log("📦 Soltada: " + heldObject.name);

            heldObject = null;
            heldCollider = null;
        }
    }
}