using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float baseBulletSpeed = 15f;
    public float baseFireInterval = 0.3f;
    public Slider powerupBar; // Asigna el slider de UI en el inspector
    public float powerupDuration = 15f; // Tiempo antes de perder un nivel

    private int powerupLevel = 0; // 0: normal, 1: más rápido, 2: dos balas, 3: tres balas
    private float powerupTimer = 0f;
    private float currentFireInterval;
    private float lastShootTime = 0f;

    void Start()
    {
        currentFireInterval = baseFireInterval;
        powerupBar.maxValue = 3;
        powerupBar.value = 0;
    }

    void Update()
    {
        if (Time.time - lastShootTime >= currentFireInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }

        // Powerup timer
        if (powerupLevel > 0)
        {
            powerupTimer -= Time.deltaTime;
            if (powerupTimer <= 0f)
            {
                powerupLevel--;
                powerupBar.value = powerupLevel;
                UpdateFireSettings();
                if (powerupLevel > 0)
                    powerupTimer = powerupDuration;
            }
        }
    }

    void Shoot()
    {
        int bullets = Mathf.Clamp(powerupLevel, 1, 3); // 1, 2 o 3 balas
        float spread = 0.5f; // separación entre balas

        for (int i = 0; i < bullets; i++)
        {
            // Offset horizontal de la bala respecto al centro
            float offset = (i - (bullets - 1) / 2f) * spread;

            // Posición desplazada en el eje X local
            Vector3 spawnPos = shootPoint.position + shootPoint.right * offset;

            // Instanciar bala mirando hacia adelante
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
        powerupBar.value = powerupLevel;
        UpdateFireSettings();
        powerupTimer = powerupDuration;
    }

    void UpdateFireSettings()
    {
        // Cambia la cadencia según el nivel
        if (powerupLevel == 0)
            currentFireInterval = baseFireInterval;
        else if (powerupLevel == 1)
            currentFireInterval = baseFireInterval * 0.7f; // Más rápido
        else
            currentFireInterval = baseFireInterval * 0.7f; // Mantén la cadencia rápida
    }
}
