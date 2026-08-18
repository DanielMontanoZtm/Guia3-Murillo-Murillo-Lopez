Se configuró un mapa 3D con dos Quads: una zona roja (A), donde se ubican las cajas, y una zona azul (B), destinada a su entrega. Además, se incluyeron 4 cajas (box_01 a box_04), cada una equipada con un Box Collider y un Rigidbody con gravedad, para responder correctamente a la física del motor.

Como novedad, se implementó una retícula en pantalla (crosshair) que marca el centro de la vista de la cámara, facilitando el enfoque y reconocimiento de los objetos interactuables.

Al ejecutar el proyecto por primera vez, se inicializa un cronómetro (timer), que se detiene automáticamente cuando las 4 cajas se encuentran dentro del radio de tolerancia (zoneRadius) del Quad A y en estado de reposo (linearVelocity.magnitude < 0.5f), notificando al jugador que completó la tarea.

En cuanto a los controles, se implementó un joystick fijo anclado en la zona inferior izquierda de la pantalla, una zona táctil para el manejo de la cámara, un botón de reinicio anclado en la zona superior izquierda, y un botón de acción principal ("Agarrar/Soltar") anclado en la zona superior derecha.

Integrantes

Daniel Alejandro Murillo Corredor David Santiago Murillo Neme Daniel Felipe López Montaño
