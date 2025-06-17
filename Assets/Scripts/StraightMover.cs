using UnityEngine;

public class StraightMover : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 15f;

    private float timer = 0f;

    void Update()
    {
        // Mover en la dirección +Z del mundo
        transform.position += Vector3.forward * speed * Time.deltaTime;

        // Destruir después de cierto tiempo
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
