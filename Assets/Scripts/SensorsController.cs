// Assets/Scripts/Sensors/SensorsController.cs
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class SensorsController : MonoBehaviour
{
    [Header("Sensores Configuracion")]
    [SerializeField] private float speedMultiplier = 10f;
    [SerializeField] private float rotationSpeedMultiplier = 50f;
    
    [Header("UI (Opcional, para debug en compilación)")]
    [SerializeField] private TMP_Text accelText;
    [SerializeField] private TMP_Text gyroText;

    private void OnEnable()
    {
        // Es necesario explícitamente habilitarlos, de lo contrario en InputSystem pueden venir desactivados.
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            Debug.Log("Acelerómetro Habilitado.");
        }
        
        if (Gyroscope.current != null)
        {
            InputSystem.EnableDevice(Gyroscope.current);
            Debug.Log("Giroscopio Habilitado.");
        }
    }

    private void OnDisable()
    {
        if (Accelerometer.current != null)
        {
            InputSystem.DisableDevice(Accelerometer.current);
        }
        
        if (Gyroscope.current != null)
        {
            InputSystem.DisableDevice(Gyroscope.current);
        }
    }

    private void Update()
    {
        HandleAccelerometer();
        HandleGyroscope();
    }

    private void HandleAccelerometer()
    {
         if (Accelerometer.current != null)
         {
             // La acelearción lineal (incluyendo gravedad normalmente)
             Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
             
             // Por ejemplo, lo usamos para desplazar el objeto en X y Z sutilmente (estilo Rolling Ball).
             // Hay que suavizar / filtrar estos valores normalmente debido al 'ruido' sensorial, pero esta es la forma cruda:
             Vector3 movement = new Vector3(acceleration.x, 0, acceleration.y);
             transform.Translate(movement * speedMultiplier * Time.deltaTime, Space.World);
             
             if (accelText != null) accelText.text = $"Accel: {acceleration}";
         }
         else
         {
             if (accelText != null) accelText.text = "Acelerómetro: Desconectado/Null";
         }
    }

    private void HandleGyroscope()
    {
         if (Gyroscope.current != null)
         {
             // Velocidad angular cruda reportada por el dispositivo.
             Vector3 angularVel = Gyroscope.current.angularVelocity.ReadValue();
             
             // Aplicar rotación. Note: Es posible que en dispositivo real el mapeo de ejes XYZ varíe según portrait/landscape
             transform.Rotate(angularVel * rotationSpeedMultiplier * Time.deltaTime, Space.World);
             
             if (gyroText != null) gyroText.text = $"Gyro (AngularVel): {angularVel}";
         }
         else
         {
             if (gyroText != null) gyroText.text = "Giroscopio: Desconectado/Null";
         }
    }
}
