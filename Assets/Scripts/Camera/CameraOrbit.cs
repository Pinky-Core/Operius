using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
    [Header("Configuración de Órbita")]
    public Transform target; // Centro del mapa
    public float radius = 10f; // Distancia desde el centro
    public float height = 5f; // Altura de la cámara
    public float orbitSpeed = 2f; // Velocidad de rotación (aumentada)
    public float heightVariation = 2f; // Variación en altura
    public float heightSpeed = 0.5f; // Velocidad de cambio de altura
    public float lookAngle = 45f; // Ángulo de vista (45 grados para ver mejor el movimiento)

    [Header("Tipos de Movimiento")]
    public OrbitType orbitType = OrbitType.Circular;
    public bool enableSmoothMovement = true;
    public float smoothSpeed = 5f;

    [Header("Configuración Avanzada")]
    public bool enableZoom = false;
    public float minRadius = 5f;
    public float maxRadius = 15f;
    public float zoomSpeed = 2f;

    public enum OrbitType
    {
        Circular,       // Órbita circular simple
        Elliptical,     // Órbita elíptica
        Figure8,        // Movimiento en forma de 8
        Spiral,         // Movimiento en espiral
        Wave,           // Movimiento ondulante
        Random          // Movimiento aleatorio suave
    }

    private float currentAngle = 0f;
    private float currentHeight = 0f;
    private float targetRadius;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    void Start()
    {
        // Si no hay target, crear uno en el origen
        if (target == null)
        {
            GameObject targetObj = new GameObject("CameraTarget");
            target = targetObj.transform;
            target.position = Vector3.zero;
        }

        // Inicializar valores
        currentHeight = height;
        targetRadius = radius;
        targetPosition = CalculateTargetPosition();
        
        Debug.Log("CameraOrbit iniciado con órbita: " + orbitType);
    }

    void Update()
    {
        // Actualizar posición objetivo según el tipo de órbita
        targetPosition = CalculateTargetPosition();

        // Mover la cámara suavemente hacia la posición objetivo
        if (enableSmoothMovement)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / smoothSpeed);
        }
        else
        {
            transform.position = targetPosition;
        }

        // Hacer que la cámara mire al centro con un ángulo fijo para movimiento consistente
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        // Usar un ángulo fijo hacia abajo para evitar problemas en rotaciones específicas
        Vector3 lookDirection = Vector3.Lerp(directionToTarget, Vector3.down, 0.3f).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // Actualizar ángulo para la siguiente frame - MOVIMIENTO INFINITO
        currentAngle += orbitSpeed * Time.deltaTime;

        // Mantener el ángulo entre 0 y 360 para evitar overflow
        if (currentAngle > 360f)
        {
            currentAngle -= 360f;
        }
    }

    Vector3 CalculateTargetPosition()
    {
        Vector3 position = Vector3.zero;

        switch (orbitType)
        {
            case OrbitType.Circular:
                position = CalculateCircularPosition();
                break;

            case OrbitType.Elliptical:
                position = CalculateEllipticalPosition();
                break;

            case OrbitType.Figure8:
                position = CalculateFigure8Position();
                break;

            case OrbitType.Spiral:
                position = CalculateSpiralPosition();
                break;

            case OrbitType.Wave:
                position = CalculateWavePosition();
                break;

            case OrbitType.Random:
                position = CalculateRandomPosition();
                break;
        }

        return position;
    }

    Vector3 CalculateCircularPosition()
    {
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * targetRadius;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * targetRadius;
        float y = currentHeight + Mathf.Sin(currentAngle * heightSpeed * Mathf.Deg2Rad) * heightVariation;

        return target.position + new Vector3(x, y, z);
    }

    Vector3 CalculateEllipticalPosition()
    {
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * targetRadius * 1.5f;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * targetRadius * 0.8f;
        float y = currentHeight + Mathf.Sin(currentAngle * heightSpeed * Mathf.Deg2Rad) * heightVariation;

        return target.position + new Vector3(x, y, z);
    }

    Vector3 CalculateFigure8Position()
    {
        float t = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Sin(t) * targetRadius;
        float z = Mathf.Sin(t) * Mathf.Cos(t) * targetRadius;
        float y = currentHeight + Mathf.Sin(currentAngle * heightSpeed * Mathf.Deg2Rad) * heightVariation;

        return target.position + new Vector3(x, y, z);
    }

    Vector3 CalculateSpiralPosition()
    {
        // ESPIRAL INFINITA que crece y decrece
        float spiralRadius = targetRadius + Mathf.Sin(currentAngle * 0.1f) * 2f;
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * spiralRadius;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * spiralRadius;
        float y = currentHeight + Mathf.Sin(currentAngle * heightSpeed * Mathf.Deg2Rad) * heightVariation;

        return target.position + new Vector3(x, y, z);
    }

    Vector3 CalculateWavePosition()
    {
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * targetRadius;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * targetRadius;
        float y = currentHeight + Mathf.Sin(currentAngle * heightSpeed * Mathf.Deg2Rad) * heightVariation * 2f;

        // Agregar ondulación infinita al movimiento
        x += Mathf.Sin(currentAngle * 2f * Mathf.Deg2Rad) * 1f;
        z += Mathf.Cos(currentAngle * 2f * Mathf.Deg2Rad) * 1f;

        return target.position + new Vector3(x, y, z);
    }

    Vector3 CalculateRandomPosition()
    {
        // Usar Perlin noise para movimiento aleatorio infinito y suave
        float time = Time.time * 0.5f;
        float x = Mathf.PerlinNoise(time, 0f) * 2f - 1f;
        float z = Mathf.PerlinNoise(0f, time) * 2f - 1f;
        float y = Mathf.PerlinNoise(time * 0.5f, time * 0.5f) * 2f - 1f;

        return target.position + new Vector3(x * targetRadius, currentHeight + y * heightVariation, z * targetRadius);
    }

    // Métodos públicos para controlar la cámara - SIN PAUSAS
    public void SetOrbitType(OrbitType newType)
    {
        orbitType = newType;
        Debug.Log("Cambiando a órbita: " + newType);
    }

    public void SetOrbitSpeed(float speed)
    {
        orbitSpeed = speed;
        Debug.Log("Velocidad de órbita: " + speed);
    }

    public void SetRadius(float newRadius)
    {
        targetRadius = Mathf.Clamp(newRadius, minRadius, maxRadius);
        Debug.Log("Radio de órbita: " + targetRadius);
    }

    public void SetHeight(float newHeight)
    {
        currentHeight = newHeight;
        Debug.Log("Altura de cámara: " + newHeight);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Debug.Log("Nuevo target establecido");
    }

    // Método para zoom suave
    public void ZoomIn()
    {
        if (enableZoom)
        {
            targetRadius = Mathf.Max(targetRadius - zoomSpeed, minRadius);
            Debug.Log("Zoom in - Radio: " + targetRadius);
        }
    }

    public void ZoomOut()
    {
        if (enableZoom)
        {
            targetRadius = Mathf.Min(targetRadius + zoomSpeed, maxRadius);
            Debug.Log("Zoom out - Radio: " + targetRadius);
        }
    }

    // Método para cambiar a un movimiento específico - MOVIMIENTO SIEMPRE ACTIVO
    public void ChangeToCircular()
    {
        orbitType = OrbitType.Circular;
        Debug.Log("Cambiando a Circular");
    }

    public void ChangeToElliptical()
    {
        orbitType = OrbitType.Elliptical;
        Debug.Log("Cambiando a Elliptical");
    }

    public void ChangeToFigure8()
    {
        orbitType = OrbitType.Figure8;
        Debug.Log("Cambiando a Figure8");
    }

    public void ChangeToSpiral()
    {
        orbitType = OrbitType.Spiral;
        Debug.Log("Cambiando a Spiral");
    }

    public void ChangeToWave()
    {
        orbitType = OrbitType.Wave;
        Debug.Log("Cambiando a Wave");
    }

    public void ChangeToRandom()
    {
        orbitType = OrbitType.Random;
        Debug.Log("Cambiando a Random");
    }

    // Método para acelerar/desacelerar el movimiento (sin pausar)
    public void AccelerateMovement(float multiplier = 2f)
    {
        orbitSpeed *= multiplier;
        Debug.Log("Acelerando órbita: " + orbitSpeed);
    }

    public void SlowDownMovement(float multiplier = 0.5f)
    {
        orbitSpeed *= multiplier;
        Debug.Log("Desacelerando órbita: " + orbitSpeed);
    }

    public void SetMovementSpeed(float speed)
    {
        orbitSpeed = speed;
        Debug.Log("Velocidad establecida: " + speed);
    }

    // Método para obtener información de debug
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.position, radius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
} 