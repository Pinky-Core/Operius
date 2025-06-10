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
            // Aqu� puede ir l�gica de vida, restart, etc.
        }
    }
}
