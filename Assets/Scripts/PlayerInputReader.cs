// Assets/Scripts/Player/PlayerInputReader.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    // Variables para cachear estados.
    private Vector2 _moveInput;
    private bool _isTapping;
    
    // Método que servirá en un esquema Send Messages o Invoke C Sharp Events puro (por código)
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Enlazando código estricto a las acciones (Si NO usamos Send Messages / Invoke Unity Events del Inspector)
        // Ejemplo alternativo suscribiéndose directamente a la acción.
        if (_playerInput != null && _playerInput.actions != null)
        {
            var moveAction = _playerInput.actions["Move"];
            if(moveAction != null)
            {
                 moveAction.performed += OnMoveExternal;
                 moveAction.canceled += OnMoveExternal;
            }
            
            var tapAction = _playerInput.actions["Tap"];
            if(tapAction != null)
            {
                 tapAction.started += ctx => _isTapping = true;
                 tapAction.canceled += ctx => _isTapping = false;
            }
        }
    }

    private void OnDisable()
    {
         if (_playerInput != null && _playerInput.actions != null)
         {
             var moveAction = _playerInput.actions["Move"];
             if(moveAction != null)
             {
                  moveAction.performed -= OnMoveExternal;
                  moveAction.canceled -= OnMoveExternal;
             }
         }
    }

    private void OnMoveExternal(InputAction.CallbackContext ctx)
    {
         _moveInput = ctx.ReadValue<Vector2>();
         Debug.Log($"Leyendo [Move] vía Delegate C#: {_moveInput}");
    }

    // -- SI UTILIZAN EL MODO "Send Messages" EN EL PLAYER INPUT COMPONENT --
    // El sistema busca métodos llamados On[NombreAccion].
    public void OnMove(InputValue value)
    {
         _moveInput = value.Get<Vector2>();
         // Debug.Log($"Leyendo [Move] vía Send Messages: {_moveInput}");
    }

    public void OnTap(InputValue value)
    {
         _isTapping = value.isPressed;
         // Debug.Log($"Leyendo [Tap] vía Send Messages: {_isTapping}");
    }
}
