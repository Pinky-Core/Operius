using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador golpeado");
            // Aquí puede ir lógica de vida, restart, etc.
        }
    }
}
