using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float baseBulletSpeed = 15f;
    public float baseFireInterval = 0.3f;

    [Header("Power-up Visuals")]
    public Image[] powerupBars;

    public float powerupDuration = 15f;

    private int powerupLevel = 0;
    private float powerupTimer = 0f;
    private float currentFireInterval;
    private float lastShootTime = 0f;

    public bool canShoot = true;

    private int sectorLevel = 0;

    public static event Action<int> SectorLevelUpEvent;

    void Start()
    {
        currentFireInterval = baseFireInterval;
        UpdatePowerupVisual();
    }

    void Update()
    {
        if (!canShoot) return;

        if (Time.time - lastShootTime >= currentFireInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }

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
        int bullets = Mathf.Clamp(powerupLevel, 1, 3);
        
        // Vibración para disparo
        VibrationHelper.PlayShootVibration();
        float spread = 0.5f;

        // Reproducir sonido de disparo
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayShootSound();
        }

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
        {
            powerupLevel++;
        }
        else
        {
            sectorLevel++;
            powerupLevel = 0;
            SectorLevelUpEvent?.Invoke(sectorLevel);
        }

        UpdatePowerupVisual();
        UpdateFireSettings();
        powerupTimer = powerupDuration;
    }

    void UpdateFireSettings()
    {
        currentFireInterval = (powerupLevel == 0) ? baseFireInterval : baseFireInterval * 0.7f;
    }

    void UpdatePowerupVisual()
    {
        for (int i = 0; i < powerupBars.Length; i++)
        {
            powerupBars[i].enabled = (i < powerupLevel);
        }
    }
}
