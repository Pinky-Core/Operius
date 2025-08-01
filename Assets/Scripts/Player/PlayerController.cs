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
            
        // Cargar sensibilidad del giroscopio desde las opciones
        LoadGyroSensitivity();
    }
    
    /// <summary>
    /// Carga la sensibilidad del giroscopio desde OptionsManager
    /// </summary>
    private void LoadGyroSensitivity()
    {
        if (OptionsManager.Instance != null)
        {
            gyroSensitivity = OptionsManager.Instance.GetGyroSensitivity();
        }
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
            
            // Vibración para powerup
            VibrationHelper.PlayPowerupVibration();
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
        else if (other.CompareTag("Asteroid"))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;

        GetComponent<PlayerShooting>().canShoot = false;

        // Reproducir sonido de muerte
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayDeathSound();
        }
        
        // Vibración para muerte
        VibrationHelper.PlayDeathVibration();

        // Guardar highscore
        int score = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
        
        // Convertir puntos restantes a monedas al morir
        if (score > 0)
        {
            // Convertir puntos finales a monedas
            int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
            int newCoins = score / 100; // 100 puntos = 1 moneda
            PlayerPrefs.SetInt("PlayerCoins", currentCoins + newCoins);
            PlayerPrefs.Save();
            Debug.Log($"Jugador muerto - Convertidos {score} puntos finales a {newCoins} monedas de la tienda");
        }

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

        // Reproducir sonido de game over
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayGameOverSound();
        }
        
        // Vibración para game over
        VibrationHelper.PlayGameOverVibration();

        // Detener música del juego antes de mostrar game over
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.StopGameMusic();
        }

        // Mostrar panel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Finalmente, destruir jugador si lo deseás
        Destroy(gameObject);
    }
}
