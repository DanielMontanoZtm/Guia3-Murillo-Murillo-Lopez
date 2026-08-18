using UnityEngine;
using UnityEngine.InputSystem;

public class TiltMover : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 2.2f;
    [SerializeField] private float deadZone = 0.12f;
    [SerializeField] private float smoothing = 8f;
    private Vector2 smoothedTilt;

    private void Update()
    {
        if (Accelerometer.current == null) return;
        Vector3 raw = Accelerometer.current.acceleration.ReadValue();
        Vector2 tilt = new Vector2(raw.x, raw.y);
        if (tilt.magnitude < deadZone) tilt = Vector2.zero;
        smoothedTilt = Vector2.Lerp(smoothedTilt, tilt, Time.deltaTime * smoothing);
        Vector3 move = new Vector3(smoothedTilt.x, 0f, smoothedTilt.y);
        controller.Move(move * speed * Time.deltaTime);
    }
}