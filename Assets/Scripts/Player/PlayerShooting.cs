using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float baseBulletSpeed = 15f;
    public float baseFireInterval = 0.3f;

    [Header("Power-up Visuals")]
    public Image[] powerupBars; // Asigna aquí las 3 imágenes del HUD (en orden)

    public float powerupDuration = 15f; // Tiempo antes de perder un nivel

    private int powerupLevel = 0; // 0: normal, 1: más rápido, 2: dos balas, 3: tres balas
    private float powerupTimer = 0f;
    private float currentFireInterval;
    private float lastShootTime = 0f;

    void Start()
    {
        currentFireInterval = baseFireInterval;
        UpdatePowerupVisual();
    }

    void Update()
    {
        // Disparo automático según intervalo
        if (Time.time - lastShootTime >= currentFireInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }

        // Timer de degradación de powerup
        if (powerupLevel > 0)
        {
            powerupTimer -= Time.deltaTime;
            if (powerupTimer <= 0f)
            {
                powerupLevel--;
                UpdatePowerupVisual();
                UpdateFireSettings();

                if (powerupLevel > 0)
                    powerupTimer = powerupDuration;
            }
        }
    }

    void Shoot()
    {
        int bullets = Mathf.Clamp(powerupLevel, 1, 3); // Dispara 1, 2 o 3 balas
        float spread = 0.5f; // Separación entre balas

        for (int i = 0; i < bullets; i++)
        {
            float offset = (i - (bullets - 1) / 2f) * spread;
            Vector3 spawnPos = shootPoint.position + shootPoint.right * offset;

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, shootPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = -shootPoint.forward * baseBulletSpeed;
            }
        }
    }

    public void CollectPowerup()
    {
        if (powerupLevel < 3)
            powerupLevel++;

        UpdatePowerupVisual();
        UpdateFireSettings();
        powerupTimer = powerupDuration;
    }

    void UpdateFireSettings()
    {
        if (powerupLevel == 0)
            currentFireInterval = baseFireInterval;
        else
            currentFireInterval = baseFireInterval * 0.7f; // Más rápido a partir de nivel 1
    }

    void UpdatePowerupVisual()
    {
        for (int i = 0; i < powerupBars.Length; i++)
        {
            powerupBars[i].enabled = (i < powerupLevel);
        }
    }
}
