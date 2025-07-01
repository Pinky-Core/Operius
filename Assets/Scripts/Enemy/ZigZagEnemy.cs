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
    private float startTime;

    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        this.center = center;
        this.radius = radius;
        this.angularSpeed = angularSpeed;
        this.forwardSpeed = forwardSpeed;

        amplitude = Mathf.Clamp(radius * 0.3f, 0.5f, radius * 0.7f);
        frequency = angularSpeed / 45f;

        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Guardamos el tiempo inicial para controlar el avance
        startTime = Time.time;
    }

    void Update()
    {
        if (center == null) return;

        float elapsed = Time.time - startTime;

        // Avance angular en grados
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        // Zigzag radial (radio fluctuante)
        float currentRadius = radius + Mathf.Sin(elapsed * frequency) * amplitude;

        // Offset orbital con zigzag
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * currentRadius;

        // Posición final: centro + offset + avance en Z por tiempo transcurrido
        transform.position = center.position + offset + Vector3.forward * forwardSpeed * elapsed;

        // Rotación mirando hacia el centro
        Vector3 toCenter = (transform.position - center.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, toCenter);

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
