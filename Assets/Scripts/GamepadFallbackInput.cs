using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadFallbackInput : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform player;

    private void Update()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null) return;
        Vector2 move = pad.leftStick.ReadValue();
        Vector2 look = pad.rightStick.ReadValue();
        bool grab = pad.rightTrigger.wasPressedThisFrame;
        bool place = pad.buttonSouth.wasPressedThisFrame;
        player.Translate(new Vector3(move.x, 0f, move.y) * moveSpeed * Time.deltaTime);
        if (grab) SendMessage("TryGrab", SendMessageOptions.DontRequireReceiver);
        if (place) SendMessage("TryPlace", SendMessageOptions.DontRequireReceiver);
    }
}