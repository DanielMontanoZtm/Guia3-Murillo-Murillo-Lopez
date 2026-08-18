using UnityEngine;
using TMPro;

public class cronometro : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Zona de Entrega (Punto B)")]
    [SerializeField] private Transform targetZone; // El Quad o punto B
    [SerializeField] private float zoneRadius = 2.5f;
    [SerializeField] private Rigidbody[] cubes; // Las 4 cajas

    private float timer = 0f;
    private bool isRunning = false;
    private bool completed = false;

    private void Update()
    {
        if (completed) return;

        // Inicia el cronómetro en cuanto el jugador se mueva o interactúe
        if (!isRunning && (Input.anyKey || Input.touchCount > 0))
        {
            isRunning = true;
        }

        if (isRunning)
        {
            timer += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = $"⏱️ Tiempo: {timer:F2}s";
            }

            CheckStackingCondition();
        }
    }

    private void CheckStackingCondition()
    {
        if (cubes == null || cubes.Length < 4 || targetZone == null) return;

        int cubesInZone = 0;
        foreach (var cube in cubes)
        {
            if (cube == null || cube.isKinematic) return; // Si la tienes en la mano, no cuenta

            // Medir distancia horizontal hacia el Punto B
            float dist = Vector2.Distance(
                new Vector2(cube.position.x, cube.position.z),
                new Vector2(targetZone.position.x, targetZone.position.z)
            );

            if (dist <= zoneRadius && cube.linearVelocity.magnitude < 0.2f)
            {
                cubesInZone++;
            }
        }

        // Si las 4 cajas están en la zona B y quietas
        if (cubesInZone == 4)
        {
            completed = true;
            isRunning = false;
            if (timerText != null)
            {
                timerText.text = $"🎉 ¡COMPLETADO!\nTiempo final: {timer:F2}s";
            }
        }
    }
}