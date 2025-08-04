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
    
    [Header("Configuración de Botones")]
    public bool enableButtonControls = true;
    public float buttonSensitivity = 1.0f;
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;
    public KeyCode leftTouchButton = KeyCode.LeftArrow;
    public KeyCode rightTouchButton = KeyCode.RightArrow;

    public GameObject deathEffect;         // Prefab de partículas
    public GameObject gameOverPanel;       // Panel a mostrar tras la muerte
    public CameraShake cameraShake;        // Referencia al componente CameraShake

    private float angle;
    private bool isDead = false;
    private bool hasGyroscope = false;
    private bool autoDetectedGyro = false;

    void Start()
    {
        // Detectar automáticamente si el dispositivo tiene giroscopio
        DetectGyroscope();
        
        // Cargar sensibilidad del giroscopio desde las opciones
        LoadGyroSensitivity();
    }
    
    /// <summary>
    /// Detecta automáticamente si el dispositivo tiene giroscopio
    /// </summary>
    private void DetectGyroscope()
    {
        hasGyroscope = SystemInfo.supportsGyroscope;
        autoDetectedGyro = hasGyroscope;
        
        if (hasGyroscope)
        {
            Input.gyro.enabled = true;
            useGyro = true;
            Debug.Log("PlayerController: Giroscopio detectado y habilitado");
        }
        else
        {
            useGyro = false;
            Debug.Log("PlayerController: No se detectó giroscopio - usando controles de botones");
        }
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

        // Usar giroscopio si está disponible y habilitado
        if (useGyro && hasGyroscope)
        {
            float gyroInput = -Input.gyro.rotationRateUnbiased.z;
            if (invertGyro) gyroInput *= -1f;

            float scaledInput = gyroInput * gyroSensitivity;
            input = Mathf.Abs(scaledInput) > gyroThreshold ? Mathf.Clamp(scaledInput, -1f, 1f) : 0f;
        }
        // Usar controles de botones como fallback
        else if (enableButtonControls)
        {
            input = GetButtonInput();
        }
        // Fallback a controles horizontales estándar
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
    
    /// <summary>
    /// Obtiene el input de los botones configurados
    /// </summary>
    private float GetButtonInput()
    {
        float input = 0f;
        
        // Botones de teclado
        if (Input.GetKey(leftButton) || Input.GetKey(leftTouchButton))
        {
            input -= buttonSensitivity;
        }
        if (Input.GetKey(rightButton) || Input.GetKey(rightTouchButton))
        {
            input += buttonSensitivity;
        }
        
        // Controles táctiles (para móviles)
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                
                // Dividir la pantalla en dos mitades
                if (touch.position.x < Screen.width * 0.5f)
                {
                    // Lado izquierdo de la pantalla
                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        input -= buttonSensitivity;
                    }
                }
                else
                {
                    // Lado derecho de la pantalla
                    if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        input += buttonSensitivity;
                    }
                }
            }
        }
        
        return Mathf.Clamp(input, -1f, 1f);
    }
    
    /// <summary>
    /// Fuerza el uso de giroscopio (útil para testing)
    /// </summary>
    [ContextMenu("Forzar Uso de Giroscopio")]
    public void ForceGyroUsage()
    {
        useGyro = true;
        hasGyroscope = true;
        Debug.Log("PlayerController: Uso de giroscopio forzado");
    }
    
    /// <summary>
    /// Fuerza el uso de botones (útil para testing)
    /// </summary>
    [ContextMenu("Forzar Uso de Botones")]
    public void ForceButtonUsage()
    {
        useGyro = false;
        hasGyroscope = false;
        Debug.Log("PlayerController: Uso de botones forzado");
    }
    
    /// <summary>
    /// Obtiene información sobre el estado de los controles
    /// </summary>
    [ContextMenu("Mostrar Info de Controles")]
    public void ShowControlInfo()
    {
        string info = "=== INFORMACIÓN DE CONTROLES ===\n";
        info += $"Giroscopio disponible: {SystemInfo.supportsGyroscope}\n";
        info += $"Giroscopio detectado: {hasGyroscope}\n";
        info += $"Usando giroscopio: {useGyro}\n";
        info += $"Controles de botones habilitados: {enableButtonControls}\n";
        info += $"Sensibilidad giroscopio: {gyroSensitivity}\n";
        info += $"Sensibilidad botones: {buttonSensitivity}\n";
        info += $"Botón izquierdo: {leftButton}\n";
        info += $"Botón derecho: {rightButton}\n";
        
        Debug.Log(info);
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

        // El highscore se maneja en DeathMenu para evitar duplicación
        
        // Convertir puntos restantes a monedas al morir
        int finalScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetCurrentScore() : 0;
        if (finalScore > 0)
        {
            // Convertir puntos finales a monedas
            int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
            int newCoins = finalScore / 100; // 100 puntos = 1 moneda
            PlayerPrefs.SetInt("PlayerCoins", currentCoins + newCoins);
            PlayerPrefs.Save();
            Debug.Log($"Jugador muerto - Convertidos {finalScore} puntos finales a {newCoins} monedas de la tienda");
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
