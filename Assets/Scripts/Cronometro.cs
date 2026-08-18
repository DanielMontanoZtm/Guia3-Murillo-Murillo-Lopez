using UnityEngine;
using TMPro;

public class cronometro : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Zona de Entrega (Punto B)")]
    [SerializeField] private Transform targetZone; // Asignar el QuadB
    [SerializeField] private float zoneRadius = 2.5f;
    [SerializeField] private Rigidbody[] cubes; // Asignar las 4 cajas

    private float timer = 0f;
    private bool isRunning = false;
    private bool completed = false;

    private void Start()
    {
        // Inicia automáticamente en cuanto carga el nivel
        isRunning = true;
        timer = 0f;
    }

    private void Update()
    {
        if (completed) return;

        if (isRunning)
        {
            timer += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = $"Tiempo: {timer:F2}s";
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
            // 1. Si la caja no existe o está agarrada en la mano (useGravity == false), no se cuenta
            if (cube == null || cube.useGravity == false) continue; 

            // 2. Medir distancia horizontal (Ejes X y Z) ignorando la altura del apilamiento
            float dist = Vector2.Distance(
                new Vector2(cube.position.x, cube.position.z),
                new Vector2(targetZone.position.x, targetZone.position.z)
            );

            // 3. Verificar si está dentro del radio y razonablemente quieta (tolerancia ajustada a 0.5)
            if (dist <= zoneRadius && cube.linearVelocity.magnitude < 0.5f)
            {
                cubesInZone++;
            }
        }

        // Si las 4 cajas están dentro de Quad B
        if (cubesInZone == 4)
        {
            completed = true;
            isRunning = false;
            if (timerText != null)
            {
                timerText.text = $"🎉 ¡COMPLETADO!\nTiempo final: {timer:F2}s";
            }
            Debug.Log("✅ ¡Felicidades! Se apilaron las 4 cajas en el Punto B.");
        }
    }

    // Dibuja un círculo verde en la pestaña Scene para visualizar el rango del Punto B
    private void OnDrawGizmosSelected()
    {
        if (targetZone != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetZone.position, zoneRadius);
        }
    }
}