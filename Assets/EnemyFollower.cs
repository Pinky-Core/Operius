using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    public Transform leader;
    public float followDelay = 0.2f;

    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private float recordInterval = 0.02f;
    private float recordTimer;

    void Update()
    {
        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            positionHistory.Enqueue(leader.position);
            recordTimer = 0f;
        }

        if (positionHistory.Count > Mathf.RoundToInt(followDelay / recordInterval))
        {
            transform.position = positionHistory.Dequeue();
        }

        // Apuntar al centro también
        transform.up = (leader.position - transform.position).normalized;
    }
}
