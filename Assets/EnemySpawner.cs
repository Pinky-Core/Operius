using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform center;  // El centro del spawn, que puede ser el "túnel"
    public float radius = 5f;  // Radio de aparición
    public float spawnZOffset = -30f;  // Posición en Z para el spawn
    public float minSpawnDelay = 1.5f;  // Intervalo mínimo de spawn
    public float maxSpawnDelay = 3f;  // Intervalo máximo de spawn
    public int minEnemiesPerWave = 3;  // Enemigos por ola
    public int maxEnemiesPerWave = 6;  // Enemigos por ola máxima

    void Start()
    {
        StartCoroutine(SpawnWaveLoop());
    }

    IEnumerator SpawnWaveLoop()
    {
        while (true)
        {
            SpawnEnemyWave();
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);  // Espera aleatoria
            yield return new WaitForSeconds(delay);
        }
    }

    void SpawnEnemyWave()
    {
        int enemyCount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);  // Número aleatorio de enemigos por ola
        float baseAngle = Random.Range(0f, 360f);  // Ángulo base para los enemigos

        for (int i = 0; i < enemyCount; i++)
        {
            float angle = baseAngle + (360f / enemyCount) * i;  // Distribuir enemigos en un círculo
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 pos = center.position + dir * radius + Vector3.forward * spawnZOffset;

            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.transform.LookAt(center.position + Vector3.forward * 10f);  // Mirar hacia el centro
            Destroy(enemy, 10f);  // Destruir después de 10 segundos
        }
    }
}
