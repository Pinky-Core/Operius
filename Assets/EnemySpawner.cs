using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform center;
    public float radius = 5f;
    public float spawnZOffset = -30f;
    public float spawnInterval = 0.4f;  // ya no se usa para espaciado entre enemigos
    public int enemiesPerWave = 5;
    public float zSpacing = 2f;
    public float spawnDelayBetweenEnemies = 0.3f;
    public float initialDelay = 3f;  // segundos de espera antes de la primera oleada
    public float minDelayBetweenWaves = 2f;
    public float maxDelayBetweenWaves = 4f;


    void Start()
    {
        StartCoroutine(SpawnWaveLoop());
    }

    IEnumerator SpawnWaveLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float baseAngle = Random.Range(0f, 360f);
            yield return StartCoroutine(SpawnEnemyLine(baseAngle));

            // Tiempo aleatorio entre oleadas
            float waitTime = Random.Range(minDelayBetweenWaves, maxDelayBetweenWaves);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnEnemyLine(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            float zOffset = spawnZOffset - i * zSpacing;
            Vector3 spawnPos = center.position + dir * radius + Vector3.forward * zOffset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            OrbitalEnemy enemyScript = enemy.GetComponent<OrbitalEnemy>();
            enemyScript.center = center;
            enemyScript.radius = radius;
            enemyScript.angularSpeed = 90f;
            enemyScript.forwardSpeed = 5f;

            // Aquí agregamos el delay entre la aparición de cada enemigo
            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

}
