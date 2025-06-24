using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs y Centro")]
    public Transform center;

    [Header("Tipos de enemigos por fase")]
    public GameObject[] earlyEnemies;
    public GameObject[] midEnemies;
    public GameObject[] lateEnemies;

    [Header("Posición Inicial")]
    public float radius = 5f;
    public float spawnZOffset = -30f;

    [Header("Configuración de Oleadas")]
    public float initialDelay = 3f;
    public float minDelayBetweenWaves = 2f;
    public float maxDelayBetweenWaves = 4f;
    public int enemiesPerWave = 5;
    public float zSpacing = 2f;
    public float spawnDelayBetweenEnemies = 0.3f;

    [Header("Dificultad")]
    public float difficultyIncreaseInterval = 10f;
    public float speedIncreaseAmount = 0.5f;
    public float minSpawnDelayLimit = 0.5f;

    [Header("Velocidad Base")]
    public float baseAngularSpeed = 90f;
    public float baseForwardSpeed = 5f;

    private float currentMinDelay;
    private float currentMaxDelay;
    private float elapsedTime = 0f;

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
            elapsedTime += currentMinDelay; // Suma aprox el tiempo de espera entre oleadas

            float angle = Random.Range(0f, 360f);
            yield return StartCoroutine(SpawnEnemyLine(angle));

            float waitTime = Random.Range(currentMinDelay, currentMaxDelay);
            yield return new WaitForSeconds(waitTime);

            elapsedTime += waitTime;
        }
    }

    IEnumerator SpawnEnemyLine(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);

        GameObject[] possibleEnemies = GetEnemiesForTime(elapsedTime);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            float zOffset = spawnZOffset - i * zSpacing;
            Vector3 spawnPos = center.position + dir * radius + Vector3.forward * zOffset;

            GameObject prefab = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
                enemyScript.Initialize(center, radius, baseAngularSpeed, baseForwardSpeed);

            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

    GameObject[] GetEnemiesForTime(float time)
    {
        if (time < 60f)
            return earlyEnemies;
        else if (time < 120f)
        {
            int totalLength = earlyEnemies.Length + midEnemies.Length;
            GameObject[] combined = new GameObject[totalLength];
            earlyEnemies.CopyTo(combined, 0);
            midEnemies.CopyTo(combined, earlyEnemies.Length);
            return combined;
        }
        else
        {
            int totalLength = earlyEnemies.Length + midEnemies.Length + lateEnemies.Length;
            GameObject[] combined = new GameObject[totalLength];
            earlyEnemies.CopyTo(combined, 0);
            midEnemies.CopyTo(combined, earlyEnemies.Length);
            lateEnemies.CopyTo(combined, earlyEnemies.Length + midEnemies.Length);
            return combined;
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
