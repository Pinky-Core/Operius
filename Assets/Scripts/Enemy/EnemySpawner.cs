using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab y Centro")]
    public GameObject enemyPrefab;
    public Transform center;

    [Header("Radio y Posici�n Inicial")]
    public float radius = 5f;
    public float spawnZOffset = -30f;

    [Header("Configuraci�n de Oleadas")]
    [Tooltip("Tiempo antes de que aparezca la primera oleada.")]
    public float initialDelay = 3f;

    [Tooltip("Tiempo m�nimo entre oleadas.")]
    public float minDelayBetweenWaves = 2f;

    [Tooltip("Tiempo m�ximo entre oleadas.")]
    public float maxDelayBetweenWaves = 4f;

    [Space(10)]
    [Header("Configuraci�n de Enemigos")]
    public int enemiesPerWave = 5;

    [Tooltip("Separaci�n entre enemigos en Z.")]
    public float zSpacing = 2f;

    [Tooltip("Delay entre aparici�n de enemigos en una fila.")]
    public float spawnDelayBetweenEnemies = 0.3f;


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

            // Aqu� agregamos el delay entre la aparici�n de cada enemigo
            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

}
