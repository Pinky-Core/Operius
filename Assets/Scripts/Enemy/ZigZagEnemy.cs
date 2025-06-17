using UnityEngine;

public class ZigZagEnemy : MonoBehaviour, IEnemy
{
    private float speed = 5f;
    private float zigzagSpeed = 4f;
    private float frequency = 2f;
    private float lifetime = 15f;
    private float time = 0f;

    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        speed = forwardSpeed;
        zigzagSpeed = angularSpeed;
    }

    void Update()
    {
        time += Time.deltaTime;

        Vector3 move = Vector3.forward * speed * Time.deltaTime;
        move += Vector3.right * Mathf.Sin(time * frequency) * zigzagSpeed * Time.deltaTime;

        transform.position += move;

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
