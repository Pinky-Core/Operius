using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform center;
    public float radius = 5f;
    public float spawnZOffset = -30f;

    public float baseSpawnInterval = 0.5f;
    public int baseEnemiesPerWave = 3;

    void Start()
    {
        StartCoroutine(SpawnWaveLoop());
    }

    IEnumerator SpawnWaveLoop()
    {
        while (true)
        {
            float difficulty = DifficultyManager.Instance.difficultyMultiplier;

            int enemiesThisWave = Mathf.RoundToInt(baseEnemiesPerWave * difficulty);
            float interval = baseSpawnInterval / difficulty;

            StartCoroutine(SpawnEnemyLine(enemiesThisWave, interval));
            yield return new WaitForSeconds(Random.Range(2f, 4f) / difficulty);
        }
    }

    IEnumerator SpawnEnemyLine(int count, float interval)
    {
        float baseAngle = Random.Range(0f, 360f);

        for (int i = 0; i < count; i++)
        {
            float rad = baseAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f);
            Vector3 spawnPos = center.position + dir * radius + Vector3.forward * spawnZOffset;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            var script = enemy.GetComponent<OrbitalEnemy>();

            script.center = center;
            script.radius = radius;
            script.angularSpeed = Random.Range(60f, 120f);
            script.forwardSpeed = Random.Range(4f, 7f) * DifficultyManager.Instance.difficultyMultiplier;

            // Asigna tipo aleatorio según dificultad
            float r = Random.value;
            if (r < 0.4f)
                script.type = EnemyType.Orbital;
            else if (r < 0.75f)
                script.type = EnemyType.Straight;
            else
                script.type = EnemyType.ZigZag;

            yield return new WaitForSeconds(interval);
        }
    }
}
