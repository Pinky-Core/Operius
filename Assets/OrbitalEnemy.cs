using UnityEngine;

public class OrbitalEnemy : MonoBehaviour
{
    public Transform center;
    public float radius = 4f;
    public float angularSpeed = 90f;
    public float forwardSpeed = 5f;
    public float lifetime = 15f;

    private float angle;
    private float lifeTimer;

    void Start()
    {
        // Inicializar ángulo aleatorio (opcional)
        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        lifeTimer = lifetime;
    }

    void Update()
    {
        // Movimiento orbital
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        transform.position = new Vector3(offset.x, offset.y, transform.position.z);

        // Mirar hacia el centro
        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);

        // Vida limitada
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
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
