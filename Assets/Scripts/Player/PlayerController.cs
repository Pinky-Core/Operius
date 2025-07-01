using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform center;
    public float radius = 4f;
    public float maxRotationSpeed = 200f;
    public float gyroSensitivity = 1.5f;
    public float gyroThreshold = 0.05f;
    public bool useGyro = true;
    public bool invertGyro = false;

    public GameObject deathEffect;         // Prefab de partículas
    public GameObject gameOverPanel;       // Panel a mostrar tras la muerte
    public CameraShake cameraShake;        // Referencia al componente CameraShake

    private float angle;
    private bool isDead = false;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
    }

    void Update()
    {
        if (isDead) return;

        float input = 0f;

        if (useGyro && SystemInfo.supportsGyroscope)
        {
            float gyroInput = -Input.gyro.rotationRateUnbiased.z;
            if (invertGyro) gyroInput *= -1f;

            float scaledInput = gyroInput * gyroSensitivity;
            input = Mathf.Abs(scaledInput) > gyroThreshold ? Mathf.Clamp(scaledInput, -1f, 1f) : 0f;
        }
        else
        {
            input = Input.GetAxis("Horizontal");
        }

        angle += input * maxRotationSpeed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        transform.position = center.position + offset;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Powerup"))
        {
            GetComponent<PlayerShooting>().CollectPowerup();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        GetComponent<PlayerShooting>().canShoot = false;

        // Instanciar partículas de muerte
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Temblor de cámara
        if (cameraShake != null)
            cameraShake.Shake(0.3f, 0.2f); // duración, intensidad

        // Ocultar jugador
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Mostrar panel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Finalmente, destruir jugador si lo deseás
        Destroy(gameObject);
    }
}
