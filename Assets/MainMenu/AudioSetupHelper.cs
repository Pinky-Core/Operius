using UnityEngine;
using UnityEngine.UI;

public class AudioSetupHelper : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private MenuAudioManager audioManager;
    [SerializeField] private SimpleSoundEffects soundEffects;
    
    [Header("Configuración de Música")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private bool useGeneratedSounds = true;
    
    [Header("Configuración de Efectos Generados")]
    [SerializeField] private float clickFrequency = 1200f;
    [SerializeField] private float hoverFrequency = 600f;
    [SerializeField] private float rotateStartFreq = 400f;
    [SerializeField] private float rotateEndFreq = 800f;
    
    [Header("Configuración de Volúmenes")]
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 0.8f;
    
    private void Start()
    {
        SetupAudioSystem();
    }
    
    /// <summary>
    /// Configura todo el sistema de audio
    /// </summary>
    public void SetupAudioSystem()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<MenuAudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("No se encontró MenuAudioManager en la escena!");
                return;
            }
        }
        
        if (soundEffects == null)
        {
            soundEffects = FindObjectOfType<SimpleSoundEffects>();
            if (soundEffects == null)
            {
                soundEffects = gameObject.AddComponent<SimpleSoundEffects>();
            }
        }
        
        // Configurar música
        if (musicClips != null && musicClips.Length > 0)
        {
            // Usar reflexión para acceder a los campos privados
            var backgroundMusicClipsField = typeof(MenuAudioManager).GetField("backgroundMusicClips", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (backgroundMusicClipsField != null)
            {
                backgroundMusicClipsField.SetValue(audioManager, musicClips);
            }
        }
        
        // Configurar efectos de sonido generados
        if (useGeneratedSounds)
        {
            SetupGeneratedSoundEffects();
        }
        
        // Configurar volúmenes
        audioManager.SetMusicVolume(musicVolume);
        audioManager.SetSFXVolume(sfxVolume);
        
        Debug.Log("Sistema de audio configurado correctamente");
    }
    
    /// <summary>
    /// Configura los efectos de sonido generados programáticamente
    /// </summary>
    private void SetupGeneratedSoundEffects()
    {
        if (soundEffects == null) return;
        
        // Generar efectos de sonido
        AudioClip clickSound = soundEffects.CreateClickSound(clickFrequency, 0.3f);
        AudioClip hoverSound = soundEffects.CreateHoverSound(hoverFrequency, 0.2f);
        AudioClip rotateSound = soundEffects.CreateRotateSound(rotateStartFreq, rotateEndFreq, 0.4f);
        
        // Asignar los efectos usando reflexión
        var clickField = typeof(MenuAudioManager).GetField("buttonClickSound", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var hoverField = typeof(MenuAudioManager).GetField("buttonHoverSound", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var rotateField = typeof(MenuAudioManager).GetField("menuRotateSound", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (clickField != null) clickField.SetValue(audioManager, clickSound);
        if (hoverField != null) hoverField.SetValue(audioManager, hoverSound);
        if (rotateField != null) rotateField.SetValue(audioManager, rotateSound);
        
        Debug.Log("Efectos de sonido generados y configurados");
    }
    
    /// <summary>
    /// Agrega el componente ButtonAudioHandler a todos los botones de la escena
    /// </summary>
    public void AddAudioHandlersToButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        
        foreach (Button button in buttons)
        {
            if (button.GetComponent<ButtonAudioHandler>() == null)
            {
                button.gameObject.AddComponent<ButtonAudioHandler>();
            }
        }
        
        Debug.Log($"Se agregaron audio handlers a {buttons.Length} botones");
    }
    
    /// <summary>
    /// Carga automáticamente las canciones de música desde la carpeta de assets
    /// </summary>
    public void LoadMusicFromAssets()
    {
        // Buscar archivos de música en la carpeta Importado/Sc-Fi Music
        string musicPath = "Assets/Importado/Sc-Fi Music/Music/WAVs";
        
        // Nota: En tiempo de ejecución no podemos cargar archivos directamente
        // Esto es solo para referencia. Los archivos deben ser asignados manualmente
        Debug.Log("Para cargar música automáticamente, asigna los archivos .wav desde:");
        Debug.Log(musicPath);
        Debug.Log("Archivos disponibles:");
        Debug.Log("- A New Planet.wav");
        Debug.Log("- Among Stars.wav");
        Debug.Log("- Cold Space.wav");
        Debug.Log("- Glitch Bot.wav");
        Debug.Log("- Little Engine.wav");
        Debug.Log("- Space Voyager.wav");
    }
    
    /// <summary>
    /// Método para probar los efectos de sonido
    /// </summary>
    public void TestSoundEffects()
    {
        if (audioManager == null) return;
        
        Debug.Log("Probando efectos de sonido...");
        
        // Probar efectos
        audioManager.PlayButtonClickSound();
        
        // Esperar un poco y probar hover
        Invoke(nameof(TestHoverSound), 0.5f);
        
        // Esperar un poco y probar rotate
        Invoke(nameof(TestRotateSound), 1f);
    }
    
    private void TestHoverSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayButtonHoverSound();
        }
    }
    
    private void TestRotateSound()
    {
        if (audioManager != null)
        {
            audioManager.PlayMenuRotateSound();
        }
    }
    
    /// <summary>
    /// Método para cambiar la música actual
    /// </summary>
    public void NextMusic()
    {
        if (audioManager != null)
        {
            audioManager.PlayNextMusic();
        }
    }
    
    /// <summary>
    /// Método para cambiar a la música anterior
    /// </summary>
    public void PreviousMusic()
    {
        if (audioManager != null)
        {
            audioManager.PlayPreviousMusic();
        }
    }
    
    /// <summary>
    /// Método para pausar/reanudar la música
    /// </summary>
    public void ToggleMusic()
    {
        if (audioManager != null)
        {
            // Esto es una implementación simple, podrías mejorarla
            audioManager.PauseMusic();
            Invoke(nameof(ResumeMusic), 2f);
        }
    }
    
    private void ResumeMusic()
    {
        if (audioManager != null)
        {
            audioManager.ResumeMusic();
        }
    }
} 