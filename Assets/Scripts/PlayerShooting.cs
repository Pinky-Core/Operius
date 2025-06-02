using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;     // Punto desde donde se disparan las balas
    public float bulletSpeed = 15f;
    public float fireInterval = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, fireInterval);
    }

    void Shoot()
    {
        // Instancia el proyectil desde el shootPoint con su rotación
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Aplica velocidad hacia adelante desde el shootPoint
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = -shootPoint.forward * bulletSpeed;
        }
    }
}
