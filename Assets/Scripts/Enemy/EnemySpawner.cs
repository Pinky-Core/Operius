using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs y Centros")]
    public Transform center;           // Centro general
    public Transform zigzagCenter;     // Nuevo centro para ZigZagEnemy

    [Header("Tipos de enemigos por fase")]
    public GameObject[] earlyEnemies;
    public GameObject[] midEnemies;
    public GameObject[] lateEnemies;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public Slider bossHealthBarSlider;
    private bool bossSpawned = false;
    private GameObject currentBossInstance;


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
    public float speedIncreaseAmount = 0.5f;
    public float minSpawnDelayLimit = 0.5f;

    [Header("Velocidad Base")]
    public float baseAngularSpeed = 90f;
    public float baseForwardSpeed = 5f;

    private float currentMinDelay;
    private float currentMaxDelay;
    private float elapsedTime = 0f;

    void OnEnable()
    {
        PlayerShooting.SectorLevelUpEvent += OnSectorLevelUp;
    }

    void OnDisable()
    {
        PlayerShooting.SectorLevelUpEvent -= OnSectorLevelUp;
    }

    void Start()
    {
        currentMinDelay = minDelayBetweenWaves;
        currentMaxDelay = maxDelayBetweenWaves;

        StartCoroutine(SpawnWaveLoop());

        PlayerShooting.SectorLevelUpEvent += OnSectorLevelUp;
    }

    IEnumerator SpawnWaveLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Esperar mientras el boss esté vivo
            if (currentBossInstance != null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            elapsedTime += currentMinDelay;

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
        Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f).normalized;

        GameObject[] possibleEnemies = GetEnemiesForTime(elapsedTime);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            float zOffset = spawnZOffset - i * zSpacing;

            Vector3 spawnPos = center.position + dir * radius + Vector3.forward * zOffset;

            // Evitar spawn muy cerca en XY
            float minDistanceXY = 8f;
            Vector2 deltaXY = new Vector2(spawnPos.x - center.position.x, spawnPos.y - center.position.y);
            if (deltaXY.magnitude < minDistanceXY)
            {
                deltaXY = deltaXY.normalized * minDistanceXY;
                spawnPos.x = center.position.x + deltaXY.x;
                spawnPos.y = center.position.y + deltaXY.y;
            }

            // Evitar spawn muy cerca en Z (siendo "atrás")
            float minDistanceZ = 15f;
            if (spawnPos.z > center.position.z - minDistanceZ)
            {
                spawnPos.z = center.position.z - minDistanceZ;
            }

            GameObject prefab = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
            {
                // Asigna el centro según el tipo de enemigo
                if (enemyScript is ZigZagEnemy)
                    enemyScript.Initialize(zigzagCenter, radius, baseAngularSpeed, baseForwardSpeed);
                else
                    enemyScript.Initialize(center, radius, baseAngularSpeed, baseForwardSpeed);
            }

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

    void SpawnBoss()
    {
        currentBossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        BossEnemy bossScript = currentBossInstance.GetComponent<BossEnemy>();
        if (bossScript != null)
        {
            bossScript.Initialize(center, radius, baseAngularSpeed * 0.5f, baseForwardSpeed * 0.5f);

            if (bossHealthBarSlider != null)
            {
                bossScript.healthBar = bossHealthBarSlider;
                bossHealthBarSlider.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator SpawnBossGuardWave()
    {
        int totalToSpawn = 20;
        float delayBetweenSpawns = 0.5f;
        GameObject[] enemies = GetEnemiesForTime(elapsedTime);

        for (int i = 0; i < totalToSpawn; i++)
        {
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 spawnPos = bossSpawnPoint.position + dir * 10f;

            GameObject prefab = enemies[Random.Range(0, enemies.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            if (enemyScript != null)
            {
                if (enemyScript is ZigZagEnemy)
                    enemyScript.Initialize(zigzagCenter, radius, baseAngularSpeed, baseForwardSpeed);
                else
                    enemyScript.Initialize(center, radius, baseAngularSpeed, baseForwardSpeed);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        Debug.Log("Oleada completa de escoltas del boss.");
    }


    IEnumerator SpawnBossGuardWavesLoop()
    {
        while (currentBossInstance != null)
        {
            yield return StartCoroutine(SpawnBossGuardWave());
            yield return new WaitForSeconds(4f); // Espera entre cada oleada
        }

        Debug.Log("El boss ha muerto, se detienen las oleadas de escoltas.");
    }


    void OnSectorLevelUp(int newSector)
    {
        baseAngularSpeed = 90f + newSector * 5f;
        baseForwardSpeed = 5f + newSector * 2f;

        minDelayBetweenWaves = Mathf.Max(minSpawnDelayLimit, 2f - newSector * 0.3f);
        maxDelayBetweenWaves = Mathf.Max(minSpawnDelayLimit, 4f - newSector * 0.3f);

        currentMinDelay = minDelayBetweenWaves;
        currentMaxDelay = maxDelayBetweenWaves;

        Debug.Log($"Sector {newSector} alcanzado: dificultad ajustada.");

        if (newSector == 3 && !bossSpawned)
        {
            bossSpawned = true;
            SpawnBoss();
            StartCoroutine(SpawnBossGuardWavesLoop());
        }
    }
}
