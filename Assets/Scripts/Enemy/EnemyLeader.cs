using UnityEngine;

public class EnemyLeader : MonoBehaviour
{
    public Transform player;  // El jugador al que deben moverse
    public Transform spawnCenter;  // El centro del spawn
    public float angularSpeed = 50f;  // Velocidad de rotación (orbital)
    public float radialSpeed = 0.5f;  // Velocidad para acercarse al jugador

    private float angle;
    private float radius;

    void Start()
    {
        if (spawnCenter == null)
        {
            spawnCenter = GameObject.Find("Spawner").transform;  // El objeto central desde donde nacen los enemigos
        }
        if (player == null)
        {
            player = GameObject.Find("Player").transform;  // El objeto central desde donde nacen los enemigos
        }

        // Determinamos la distancia desde el centro de spawn
        Vector3 offset = transform.position - spawnCenter.position;
        radius = offset.magnitude;  // Calcula el radio inicial

        // Calculamos el ángulo de rotación inicial
        angle = Mathf.Atan2(offset.y, offset.x);
    }

    void Update()
    {
        // Calculamos la dirección hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Giramos al enemigo hacia el jugador, pero con un ligero movimiento orbital
        angle += angularSpeed * Time.deltaTime * Mathf.Deg2Rad;  // Gira el enemigo alrededor

        // Reducimos el radio a medida que el enemigo se acerca al jugador
        radius -= radialSpeed * Time.deltaTime;

        // Calculamos la nueva posición orbital
        Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
        transform.position = spawnCenter.position + newPos;

        // Apuntamos al jugador (puedes hacer esto si quieres que los enemigos siempre miren al jugador)
        transform.up = directionToPlayer;
    }
}
