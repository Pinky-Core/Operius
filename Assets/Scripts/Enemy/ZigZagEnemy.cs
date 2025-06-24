using UnityEngine;

public class ZigZagEnemy : MonoBehaviour, IEnemy
{
    public Transform center;
    private float radius = 5f;
    private float angularSpeed = 90f;
    private float forwardSpeed = 4f;
    private float frequency = 2f;
    private float amplitude = 1.5f;
    private float lifetime = 15f;

    private float angle;
    private float time;

    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        this.center = center;
        this.radius = radius;
        this.angularSpeed = angularSpeed;
        this.forwardSpeed = forwardSpeed;

        amplitude = Mathf.Clamp(radius * 0.3f, 0.5f, radius * 0.7f);
        frequency = angularSpeed / 45f; // control más suave del zigzag

        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (center == null) return;

        time += Time.deltaTime;

        // Avance angular en grados
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        // Zigzag radial: fluctúa el radio base
        float currentRadius = radius + Mathf.Sin(time * frequency) * amplitude;

        // Posición orbital con zigzag
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * currentRadius;

        // Mover en Z también (avanza)
        transform.position = center.position + offset + Vector3.forward * forwardSpeed * time;

        // Rotación mirando hacia el centro (puede cambiarse si querés que mire hacia adelante)
        Vector3 toCenter = (transform.position - center.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, toCenter);

        // Tiempo de vida
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
