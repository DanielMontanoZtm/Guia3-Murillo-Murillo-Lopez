// Assets/Scripts/Multitouch/MultiTouchManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Collections.Generic;

public class MultiTouchManager : MonoBehaviour
{
    [SerializeField] private TMP_Text activeTouchesCountText;
    [SerializeField] private TMP_Text[] fingerPositionTexts; 
    [SerializeField] private TMP_Text distanceText;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        // Conteo total activo en la pantalla
        int activeTouches = Touch.activeTouches.Count;
        if (activeTouchesCountText != null)
        {
            activeTouchesCountText.text = $"Toques activos simultáneos: {activeTouches}";
        }

        // Asegurarse de limpiar la UI en cada frame
        foreach (var textItem in fingerPositionTexts)
        {
            if (textItem != null) textItem.text = "Disponible";
        }

        // Iterar pero clasificar la información utilizando el FINGER INDEX del hardware.
        // NUNCA basarse en el orden del array activeTouches ya que puede variar en cada frame o cruces de toques.
        foreach (var touch in Touch.activeTouches)
        {
            int fIndex = touch.finger.index;
            if (fIndex < fingerPositionTexts.Length && fingerPositionTexts[fIndex] != null)
            {
                fingerPositionTexts[fIndex].text = $"FINGER ID {fIndex} | Fase: {touch.phase} | Pos: {touch.screenPosition}";
            }
        }

        // Si existen al menos dos toques simultáneos activos, calculamos la distancia entre ellos.
        // Ideal para sentar las bases mecánicas de un "Pinch".
        if (activeTouches >= 2)
        {
            Vector2 positionFirst = Touch.activeTouches[0].screenPosition;
            Vector2 positionSecond = Touch.activeTouches[1].screenPosition;
            
            float dist = Vector2.Distance(positionFirst, positionSecond);
            if (distanceText != null) distanceText.text = $"Distancia (Toque0 y Toque1): {dist:F2} px";
        }
        else
        {
            if (distanceText != null) distanceText.text = "Distancia: Esperando 2 dedos...";
        }
    }
}
