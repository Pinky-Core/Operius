using UnityEngine;

public class ZigZagEnemy : MonoBehaviour, IEnemy
{
    private Transform center;
    private float radius = 8f;
    private float speed = 8f;       // avance en Z
    private float frequency = 2f;   // frecuencia zigzag
    private float amplitude = 8f;   // amplitud zigzag
    private float lifetime = 15f;
    private float time = 0f;

    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        this.center = center;
        this.radius = radius;
        this.speed = forwardSpeed;
        this.amplitude = Mathf.Clamp(radius * 0.5f, 0.5f, radius); // zigzag hasta mitad del radio
        this.frequency = angularSpeed / 30f; // control suave del zigzag
    }

    void Update()
    {
        if (center == null) return;

        time += Time.deltaTime;

        // Posición base en Z (avance)
        float newZ = transform.position.z + speed * Time.deltaTime;

        // Zigzag lateral en X
        float offsetX = Mathf.Sin(time * frequency) * amplitude;

        // Mantener la posición Y en la posición del centro (o la podés variar si querés)
        float baseY = center.position.y;

        // Nueva posición combinando avance en Z + zigzag en X + Y fijo en centro
        transform.position = new Vector3(center.position.x + offsetX, baseY, newZ);

        // Mirar hacia adelante (en Z)
        transform.rotation = Quaternion.LookRotation(Vector3.forward);

        // Vida limitada
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
