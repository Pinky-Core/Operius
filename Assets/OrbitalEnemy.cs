using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public enum EnemyType
{
    Orbital,
    Straight,
    ZigZag
}



public class OrbitalEnemy : MonoBehaviour
{
    public Transform center;
    public float radius = 4f;
    public float angularSpeed = 90f;
    public float forwardSpeed = 5f;
    public float lifetime = 15f;

    public EnemyType type = EnemyType.Orbital;

    private float angle;
    private float lifeTimer;
    private float zigzagTimer;

    void Start()
    {
        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        lifeTimer = lifetime;
        zigzagTimer = 0f;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        switch (type)
        {
            case EnemyType.Orbital:
                MoveOrbital();
                break;
            case EnemyType.Straight:
                MoveStraight();
                break;
            case EnemyType.ZigZag:
                MoveZigZag();
                break;
        }
    }

    void MoveOrbital()
    {
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        transform.position = new Vector3(offset.x, offset.y, transform.position.z);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);
    }

    void MoveStraight()
    {
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    }

    void MoveZigZag()
    {
        zigzagTimer += Time.deltaTime;
        float side = Mathf.Sin(zigzagTimer * 2f) * radius;
        Vector3 offset = new Vector3(side, Mathf.Cos(zigzagTimer), 0f);
        transform.position += offset.normalized * forwardSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, offset.normalized);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
