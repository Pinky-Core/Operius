using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform center;
    public float radius = 5f;
    public float spawnZOffset = -30f;
    public float spawnInterval = 0.4f;
    public int enemiesPerWave = 5;

    void Start()
    {
        StartCoroutine(SpawnWaveLoop());
    }

    IEnumerator SpawnWaveLoop()
    {
        while (true)
        {
            StartCoroutine(SpawnEnemyLine());
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    IEnumerator SpawnEnemyLine()
    {
        float baseAngle = Random.Range(0f, 360f);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            float rad = baseAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 spawnPos = center.position + dir * radius + Vector3.forward * spawnZOffset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            OrbitalEnemy enemyScript = enemy.GetComponent<OrbitalEnemy>();
            enemyScript.center = center;
            enemyScript.radius = radius;
            enemyScript.angularSpeed = 90f;
            enemyScript.forwardSpeed = 5f;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
