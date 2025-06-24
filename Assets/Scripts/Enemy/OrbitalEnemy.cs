using UnityEngine;

public class OrbitalEnemy : MonoBehaviour, IEnemy
{
    public Transform center;
    public float radius = 4f;
    public float angularSpeed = 90f;
    public float forwardSpeed = 5f;
    public float lifetime = 15f;

    private float angle;
    private float lifeTimer;

    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        this.center = center;
        this.radius = radius;
        this.angularSpeed = angularSpeed;
        this.forwardSpeed = forwardSpeed;

        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        lifeTimer = lifetime;
    }

    void Update()
    {
        if (center == null) return; // Protección extra

        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        transform.position = new Vector3(offset.x, offset.y, transform.position.z);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);

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
