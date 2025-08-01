using UnityEngine;

public class GameAudioSetup : MonoBehaviour
{
    [Header("Configuración Rápida")]
    [SerializeField] private bool setupOnStart = true;
    [SerializeField] private bool useGeneratedSounds = true;
    
    [Header("Configuración de Volúmenes")]
    [SerializeField] private float sfxVolume = 0.8f;
    
    [Header("Configuración de Efectos Generados")]
    [SerializeField] private float shootFrequency = 800f;
    [SerializeField] private float deathFrequency = 200f;
    [SerializeField] private float powerupFrequency = 1200f;
    
    private GameAudioManager audioManager;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupGameAudio();
        }
    }
    
    /// <summary>
    /// Configura el sistema de audio del juego
    /// </summary>
    public void SetupGameAudio()
    {
        // Buscar o crear el GameAudioManager
        audioManager = FindObjectOfType<GameAudioManager>();
        if (audioManager == null)
        {
            GameObject audioManagerObj = new GameObject("GameAudioManager");
            audioManager = audioManagerObj.AddComponent<GameAudioManager>();
            DontDestroyOnLoad(audioManagerObj);
        }
        
        // Configurar volúmenes
        audioManager.SetSFXVolume(sfxVolume);
        
        Debug.Log("Sistema de audio del juego configurado correctamente");
    }
    
    /// <summary>
    /// Prueba todos los efectos de sonido
    /// </summary>
    public void TestAllSounds()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<GameAudioManager>();
        }
        
        if (audioManager == null) return;
        
        Debug.Log("Probando efectos de sonido del juego...");
        
        // Probar disparo
        audioManager.PlayShootSound();
        
        // Probar powerup después de 0.5 segundos
        Invoke(nameof(TestPowerupSound), 0.5f);
        
        // Probar muerte después de 1 segundo
        Invoke(nameof(TestDeathSound), 1f);
        
        // Probar game over después de 2 segundos
        Invoke(nameof(TestGameOverSound), 2f);
    }
    
    private void TestPowerupSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayPowerupCollectSound();
        }
    }
    
    private void TestDeathSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayDeathSound();
        }
    }
    
    private void TestGameOverSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayGameOverSound();
        }
    }
    
    /// <summary>
    /// Cambia el volumen de efectos de sonido
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(sfxVolume);
        }
    }
    
    /// <summary>
    /// Obtiene el volumen actual de efectos de sonido
    /// </summary>
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    
    /// <summary>
    /// Reproduce sonido de disparo con variación
    /// </summary>
    public void PlayShootWithVariation()
    {
        if (audioManager != null)
        {
            audioManager.PlayShootSoundWithVariation(0.1f);
        }
    }
    
    /// <summary>
    /// Método para probar disparos múltiples
    /// </summary>
    public void TestMultipleShots()
    {
        if (audioManager == null) return;
        
        Debug.Log("Probando disparos múltiples...");
        
        // Disparar 5 veces con intervalos
        for (int i = 0; i < 5; i++)
        {
            Invoke(nameof(PlayShootWithVariation), i * 0.2f);
        }
    }
} 