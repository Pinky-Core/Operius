using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ejemplo de cómo usar el sistema de opciones
/// </summary>
public class OptionsExample : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button testVibrationButton;
    [SerializeField] private Button testMusicButton;
    [SerializeField] private Button testSFXButton;
    [SerializeField] private Button testGyroButton;
    
    [Header("Test Audio")]
    [SerializeField] private AudioClip testMusicClip;
    [SerializeField] private AudioClip testSFXClip;
    
    private AudioSource testAudioSource;
    
    private void Start()
    {
        SetupButtons();
        SetupAudioSource();
    }
    
    /// <summary>
    /// Configura los botones de prueba
    /// </summary>
    private void SetupButtons()
    {
        if (testVibrationButton != null)
        {
            testVibrationButton.onClick.AddListener(TestVibration);
        }
        
        if (testMusicButton != null)
        {
            testMusicButton.onClick.AddListener(TestMusicVolume);
        }
        
        if (testSFXButton != null)
        {
            testSFXButton.onClick.AddListener(TestSFXVolume);
        }
        
        if (testGyroButton != null)
        {
            testGyroButton.onClick.AddListener(TestGyroSensitivity);
        }
    }
    
    /// <summary>
    /// Configura el AudioSource para pruebas
    /// </summary>
    private void SetupAudioSource()
    {
        testAudioSource = gameObject.AddComponent<AudioSource>();
        testAudioSource.playOnAwake = false;
    }
    
    /// <summary>
    /// Prueba diferentes tipos de vibración
    /// </summary>
    public void TestVibration()
    {
        Debug.Log("=== Probando Vibración ===");
        
        // Vibración simple
        VibrationHelper.PlayShootVibration();
        Debug.Log("Vibración de disparo");
        
        // Esperar un poco y probar otra
        Invoke(nameof(TestPowerupVibration), 0.5f);
    }
    
    private void TestPowerupVibration()
    {
        VibrationHelper.PlayPowerupVibration();
        Debug.Log("Vibración de powerup");
        
        // Esperar y probar patrón
        Invoke(nameof(TestPatternVibration), 0.5f);
    }
    
    private void TestPatternVibration()
    {
        // Patrón de vibración: corta, pausa, larga, pausa, corta
        float[] durations = { 0.1f, 0.3f, 0.1f };
        float[] delays = { 0.2f, 0.2f };
        
        VibrationHelper.PlayPatternVibration(durations, delays);
        Debug.Log("Patrón de vibración");
    }
    
    /// <summary>
    /// Prueba el volumen de música
    /// </summary>
    public void TestMusicVolume()
    {
        Debug.Log("=== Probando Volumen de Música ===");
        
        if (testMusicClip != null && testAudioSource != null)
        {
            // Obtener volumen actual
            float currentVolume = OptionsManager.Instance != null ? 
                OptionsManager.Instance.GetMusicVolume() : 0.7f;
            
            Debug.Log($"Volumen de música actual: {currentVolume:P0}");
            
            // Reproducir música de prueba
            testAudioSource.clip = testMusicClip;
            testAudioSource.volume = currentVolume;
            testAudioSource.Play();
            
            Debug.Log("Reproduciendo música de prueba");
        }
        else
        {
            Debug.LogWarning("No hay clip de música asignado para la prueba");
        }
    }
    
    /// <summary>
    /// Prueba el volumen de SFX
    /// </summary>
    public void TestSFXVolume()
    {
        Debug.Log("=== Probando Volumen de SFX ===");
        
        if (testSFXClip != null && testAudioSource != null)
        {
            // Obtener volumen actual
            float currentVolume = OptionsManager.Instance != null ? 
                OptionsManager.Instance.GetSFXVolume() : 0.8f;
            
            Debug.Log($"Volumen de SFX actual: {currentVolume:P0}");
            
            // Reproducir SFX de prueba
            testAudioSource.PlayOneShot(testSFXClip, currentVolume);
            
            Debug.Log("Reproduciendo SFX de prueba");
        }
        else
        {
            Debug.LogWarning("No hay clip de SFX asignado para la prueba");
        }
    }
    
    /// <summary>
    /// Prueba la sensibilidad del giroscopio
    /// </summary>
    public void TestGyroSensitivity()
    {
        Debug.Log("=== Probando Sensibilidad del Giroscopio ===");
        
        if (OptionsManager.Instance != null)
        {
            float currentSensitivity = OptionsManager.Instance.GetGyroSensitivity();
            Debug.Log($"Sensibilidad actual: {currentSensitivity:F1}");
            
            // Buscar el PlayerController y mostrar su sensibilidad
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                Debug.Log($"Sensibilidad del PlayerController: {player.gyroSensitivity:F1}");
                
                // Cambiar sensibilidad temporalmente para probar
                float originalSensitivity = player.gyroSensitivity;
                player.gyroSensitivity = currentSensitivity * 2f;
                
                Debug.Log($"Sensibilidad aumentada temporalmente a: {player.gyroSensitivity:F1}");
                
                // Restaurar después de 3 segundos
                Invoke(nameof(RestoreGyroSensitivity), 3f);
            }
        }
    }
    
    private void RestoreGyroSensitivity()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && OptionsManager.Instance != null)
        {
            player.gyroSensitivity = OptionsManager.Instance.GetGyroSensitivity();
            Debug.Log($"Sensibilidad restaurada a: {player.gyroSensitivity:F1}");
        }
    }
    
    /// <summary>
    /// Muestra la configuración actual
    /// </summary>
    [ContextMenu("Mostrar Configuración Actual")]
    public void ShowCurrentSettings()
    {
        if (OptionsManager.Instance != null)
        {
            Debug.Log("=== Configuración Actual ===");
            Debug.Log($"Música: {OptionsManager.Instance.GetMusicVolume():P0}");
            Debug.Log($"SFX: {OptionsManager.Instance.GetSFXVolume():P0}");
            Debug.Log($"Giroscopio: {OptionsManager.Instance.GetGyroSensitivity():F1}");
            Debug.Log($"Vibración: {(OptionsManager.Instance.GetVibration() ? "ON" : "OFF")}");
            
            // Mostrar información de los managers de audio
            ShowAudioManagersInfo();
        }
        else
        {
            Debug.LogWarning("OptionsManager no encontrado");
        }
    }
    
    /// <summary>
    /// Muestra información de los managers de audio
    /// </summary>
    private void ShowAudioManagersInfo()
    {
        // SectorMusicManager (juego)
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            Debug.Log($"SectorMusicManager - Volumen: {sectorMusic.GetMusicVolume():P0}");
        }
        
        // MainMenuAudioManager (menú)
        MainMenuAudioManager mainMenuAudio = FindObjectOfType<MainMenuAudioManager>();
        if (mainMenuAudio != null)
        {
            Debug.Log($"MainMenuAudioManager - Música actual: {mainMenuAudio.GetCurrentMusicName()}");
        }
        
        // GameAudioManager (juego)
        GameAudioManager gameAudio = FindObjectOfType<GameAudioManager>();
        if (gameAudio != null)
        {
            Debug.Log($"GameAudioManager - Volumen SFX: {gameAudio.GetSFXVolume():P0}");
        }
    }
    
    /// <summary>
    /// Resetea la configuración a valores por defecto
    /// </summary>
    [ContextMenu("Resetear a Valores por Defecto")]
    public void ResetToDefaults()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.ResetToDefaults();
            Debug.Log("Configuración reseteada a valores por defecto");
        }
    }
    
    /// <summary>
    /// Prueba diferentes configuraciones
    /// </summary>
    [ContextMenu("Probar Configuraciones")]
    public void TestConfigurations()
    {
        if (OptionsManager.Instance != null)
        {
            Debug.Log("=== Probando Diferentes Configuraciones ===");
            
            // Configuración 1: Volumen alto
            OptionsManager.Instance.SetMusicVolume(1f);
            OptionsManager.Instance.SetSFXVolume(1f);
            Debug.Log("Configuración 1: Volumen máximo");
            
            // Esperar y cambiar a configuración 2
            Invoke(nameof(TestConfiguration2), 2f);
        }
    }
    
    private void TestConfiguration2()
    {
        if (OptionsManager.Instance != null)
        {
            // Configuración 2: Volumen bajo
            OptionsManager.Instance.SetMusicVolume(0.3f);
            OptionsManager.Instance.SetSFXVolume(0.3f);
            Debug.Log("Configuración 2: Volumen bajo");
            
            // Esperar y restaurar
            Invoke(nameof(RestoreConfiguration), 2f);
        }
    }
    
    private void RestoreConfiguration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.ResetToDefaults();
            Debug.Log("Configuración restaurada");
        }
    }
} 