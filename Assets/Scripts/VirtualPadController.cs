// Assets/Scripts/VirtualPad/VirtualPadController.cs
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // Requerido para leer el PlayerInput genérico

public class VirtualPadController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private TMP_Text inputVectorText;
    [SerializeField] private TMP_Text normalizedText;
    
    // Este vector lo actualizará un On-Screen Stick del Input System o el Unity Event asociado a la Action "Move"
    private Vector2 currentInputVector = Vector2.zero;

    // Puedes asociarlo vía Invoke Unity Events desde el componente Player Input -> Events -> Player -> Move
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        currentInputVector = context.ReadValue<Vector2>();
        
        if (inputVectorText != null)
        {
            inputVectorText.text = $"Vector Crudo UI: {currentInputVector}";
        }
        
        if (normalizedText != null)
        {
            normalizedText.text = $"Normalized Dir: {currentInputVector.normalized}";
        }
    }

    private void Update()
    {
        // En 3D, es común mover personaje sobre el eje X y Z desde el vector2 origen de UI X y Y.
        Vector3 movement = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        
        // Aplica el movimiento en espacio global
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
