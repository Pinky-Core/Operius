using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs y Centro")]
    [Tooltip("Lista de prefabs de enemigos que se generarán aleatoriamente")]
    public GameObject[] enemyPrefabs; // Ahora acepta múltiples tipos
    public Transform center;

    [Header("Posición Inicial")]
    [Tooltip("Radio desde el centro del tubo donde se generarán los enemigos")]
    public float radius = 5f;
    public float spawnZOffset = -30f;

    [Header("Configuración de Oleadas")]
    [Tooltip("Tiempo inicial antes de comenzar a generar oleadas")]
    public float initialDelay = 3f;
    public float minDelayBetweenWaves = 2f;
    public float maxDelayBetweenWaves = 4f;
    public int enemiesPerWave = 5;
    public float zSpacing = 2f;
    public float spawnDelayBetweenEnemies = 0.3f;

    [Header("Dificultad")]
    [Tooltip("Intervalo de tiempo para aumentar la dificultad")]
    public float difficultyIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;
    public float minSpawnDelayLimit = 0.5f;

    [Header("Velocidad Base")]
    [Tooltip("Velocidad angular base de los enemigos")]
    public float baseAngularSpeed = 90f;
    public float baseForwardSpeed = 5f;    
    private float currentMinDelay;
    private float currentMaxDelay;

    void Start()
    {
        currentMinDelay = minDelayBetweenWaves;
        currentMaxDelay = maxDelayBetweenWaves;

        StartCoroutine(SpawnWaveLoop());
        StartCoroutine(IncreaseDifficultyOverTime());
    }

    IEnumerator SpawnWaveLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float angle = Random.Range(0f, 360f);
            yield return StartCoroutine(SpawnEnemyLine(angle));

            float waitTime = Random.Range(currentMinDelay, currentMaxDelay);
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

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
                enemyScript.Initialize(center, radius, baseAngularSpeed, baseForwardSpeed);

            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

    IEnumerator IncreaseDifficultyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            baseAngularSpeed += speedIncreaseAmount;
            baseForwardSpeed += speedIncreaseAmount;

            currentMinDelay = Mathf.Max(minSpawnDelayLimit, currentMinDelay - 0.2f);
            currentMaxDelay = Mathf.Max(minSpawnDelayLimit, currentMaxDelay - 0.2f);
        }
    }
}
