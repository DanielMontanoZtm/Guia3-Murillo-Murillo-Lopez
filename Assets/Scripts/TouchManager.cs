// Assets/Scripts/Touch/TouchManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text deltaText;
    [SerializeField] private TMP_Text directionText;
    [SerializeField] private TMP_Text magnitudeText;
    [SerializeField] private TMP_Text pressureText;
    [SerializeField] private TMP_Text speedText;

    [Header("Settings")]
    [SerializeField] private float swipeThreshold = 80f;

    private void OnEnable()
    {
        // Requisito clave para activar el rastreo de toques enriquecidos
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (Touch.activeTouches.Count > 0)
        {
            // Seleccionar el toque principal (primer contacto)
            Touch primaryTouch = Touch.activeTouches[0];

            // 1. Mostrar Fase, Posición y Delta (cambio crudo del frame anterior)
            if (phaseText != null) phaseText.text = $"Phase: {primaryTouch.phase}";
            if (positionText != null) positionText.text = $"Position: {primaryTouch.screenPosition}";
            if (deltaText != null) deltaText.text = $"Delta: {primaryTouch.delta}";

            // 2. Cálculos requeridos:
            // Normalize elimina la escala manteniendo la dirección (-1 a 1 en ejes)
            Vector2 direction = primaryTouch.delta.normalized;
            // Magnitude conserva la escala y nos da la distancia total de ese frame
            float magnitude = primaryTouch.delta.magnitude;
            
            if (directionText != null) directionText.text = $"Direction: {direction}";
            if (magnitudeText != null) magnitudeText.text = $"Magnitude: {magnitude:F2}";

            // 3. Velocidad aproximada
            float touchSpeed = magnitude / Time.deltaTime;
            if (speedText != null) speedText.text = $"Speed: {touchSpeed:F2} px/s";

            // 4. Leer Presión (pressure). En hardware que no lo soporte será 0, iOS puede dar valores reales
            if (pressureText != null)
            {
                if (primaryTouch.pressure > 0f)
                {
                    pressureText.text = $"Pressure: {primaryTouch.pressure:F2}";
                }
                else
                {
                    pressureText.text = "Pressure no disponible";
                }
            }

            // 5. Swipe (Validar cruce de umbral al levantar el dedo)
            if (magnitude > swipeThreshold && primaryTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Debug.Log($"[Swipe Detectado]: Dirección -> {direction}");
            }
        }
        else
        {
            // Valores por defecto al no haber toques activos
            if (phaseText != null) phaseText.text = "Phase: None";
            if (positionText != null) positionText.text = "Position: (0, 0)";
            if (deltaText != null) deltaText.text = "Delta: (0, 0)";
            if (directionText != null) directionText.text = "Direction: (0, 0)";
            if (magnitudeText != null) magnitudeText.text = "Magnitude: 0";
            if (speedText != null) speedText.text = "Speed: 0 px/s";
            if (pressureText != null) pressureText.text = "Pressure: -";
        }
    }
}
