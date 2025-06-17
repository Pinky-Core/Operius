using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;
    public Transform center;
    public float radius = 4f;
    public float spawnZMin = -50f;
    public float spawnZMax = -10f;

    public float initialDelay = 15f;      // Delay antes del primer spawn
    public float spawnInterval = 15f;      // Tiempo entre spawns
    public float maxAdditionalDelay = 0f; // Tiempo extra aleatorio después del mínimo

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Delay inicial antes del primer spawn
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            SpawnPowerup();

            // Espera entre spawnInterval y spawnInterval + maxAdditionalDelay
            float waitTime = spawnInterval + Random.Range(0f, maxAdditionalDelay);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnPowerup()
    {
        // Generar posición alrededor del túnel (aro parado)
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        // Posición aleatoria en Z
        float z = Random.Range(spawnZMin, spawnZMax);

        Vector3 spawnPos = center.position + offset + Vector3.forward * z;

        Instantiate(powerupPrefab, spawnPos, Quaternion.identity);
    }
}
