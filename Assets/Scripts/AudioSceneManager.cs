using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSceneManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private bool muteGameAudioOnMenu = true;
    [SerializeField] private bool muteMenuAudioOnGame = true;
    [SerializeField] private bool muteAllAudioOnSceneChange = false;
    
    private void Awake()
    {
        // Suscribirse a eventos de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Desuscribirse de eventos
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// Se ejecuta cuando se carga una nueva escena
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name.ToLower();
        
        // Si es escena del menú
        if (sceneName.Contains("menu") || sceneName.Contains("main"))
        {
            HandleMenuScene();
        }
        // Si es escena del juego
        else if (sceneName.Contains("game") || sceneName.Contains("sample") || sceneName.Contains("level"))
        {
            HandleGameScene();
        }
        // Para cualquier otra escena
        else
        {
            HandleOtherScene();
        }
    }
    
    /// <summary>
    /// Maneja la escena del menú
    /// </summary>
    private void HandleMenuScene()
    {
        Debug.Log("Cargando escena del menú - Silenciando audio del juego");
        
        // Silenciar audio del juego
        if (muteGameAudioOnMenu)
        {
            MuteGameAudio();
        }
        
        // El MainMenuAudioManager se maneja automáticamente en el menú
        // No necesitamos crearlo aquí
    }
    
    /// <summary>
    /// Maneja la escena del juego
    /// </summary>
    private void HandleGameScene()
    {
        Debug.Log("Cargando escena del juego - Silenciando audio del menú");
        
        // Silenciar audio del menú
        if (muteMenuAudioOnGame)
        {
            MuteMenuAudio();
        }
        
        // Asegurar que el audio del juego esté funcionando
        EnsureGameAudio();
    }
    
    /// <summary>
    /// Maneja otras escenas
    /// </summary>
    private void HandleOtherScene()
    {
        Debug.Log("Cargando escena desconocida");
        
        if (muteAllAudioOnSceneChange)
        {
            MuteAllAudio();
        }
    }
    
    /// <summary>
    /// Silencia el audio del juego
    /// </summary>
    private void MuteGameAudio()
    {
        // Silenciar SectorMusicManager
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            sectorMusicManager.SetMusicVolume(0f);
        }
        
        // Silenciar GameAudioManager
        GameAudioManager gameAudioManager = FindObjectOfType<GameAudioManager>();
        if (gameAudioManager != null)
        {
            gameAudioManager.SetSFXVolume(0f);
        }
        
        Debug.Log("Audio del juego silenciado");
    }
    
    /// <summary>
    /// Silencia el audio del menú
    /// </summary>
    private void MuteMenuAudio()
    {
        // Silenciar MainMenuAudioManager (solo si existe)
        MainMenuAudioManager menuAudioManager = FindObjectOfType<MainMenuAudioManager>();
        if (menuAudioManager != null)
        {
            menuAudioManager.SetMusicVolume(0f);
            menuAudioManager.SetSFXVolume(0f);
            Debug.Log("Audio del menú silenciado");
        }
    }
    
    /// <summary>
    /// Silencia todo el audio
    /// </summary>
    private void MuteAllAudio()
    {
        MuteGameAudio();
        MuteMenuAudio();
        Debug.Log("Todo el audio silenciado");
    }
    
    /// <summary>
    /// Asegura que el audio del menú esté funcionando
    /// </summary>
    private void EnsureMenuAudio()
    {
        MainMenuAudioManager menuAudioManager = FindObjectOfType<MainMenuAudioManager>();
        if (menuAudioManager == null)
        {
            Debug.Log("Creando MainMenuAudioManager");
            GameObject menuAudioObj = new GameObject("MainMenuAudioManager");
            menuAudioObj.AddComponent<MainMenuAudioManager>();
        }
        else
        {
            // Restaurar volumen del menú
            menuAudioManager.SetMusicVolume(0.7f);
            menuAudioManager.SetSFXVolume(0.8f);
            Debug.Log("Volumen del menú restaurado");
        }
    }
    
    /// <summary>
    /// Asegura que el audio del juego esté funcionando
    /// </summary>
    private void EnsureGameAudio()
    {
        GameAudioManager gameAudioManager = FindObjectOfType<GameAudioManager>();
        if (gameAudioManager == null)
        {
            Debug.Log("Creando GameAudioManager");
            GameObject gameAudioObj = new GameObject("GameAudioManager");
            gameAudioObj.AddComponent<GameAudioManager>();
        }
        else
        {
            // Restaurar volumen del juego
            gameAudioManager.SetSFXVolume(0.8f);
        }
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            Debug.Log("Creando SectorMusicManager");
            GameObject sectorMusicObj = new GameObject("SectorMusicManager");
            sectorMusicObj.AddComponent<SectorMusicManager>();
        }
        else
        {
            // Restaurar volumen de música
            sectorMusicManager.SetMusicVolume(0.7f);
            
            // Verificar si la música está reproduciéndose
            if (!sectorMusicManager.IsMusicPlaying())
            {
                Debug.Log("SectorMusicManager existe pero no está reproduciendo música - iniciando...");
                sectorMusicManager.ForceStartMusic();
            }
            else
            {
                Debug.Log("SectorMusicManager ya está reproduciendo música");
            }
        }
    }
    
    /// <summary>
    /// Método público para silenciar audio manualmente
    /// </summary>
    public void MuteAudio()
    {
        string currentScene = SceneManager.GetActiveScene().name.ToLower();
        
        if (currentScene.Contains("menu") || currentScene.Contains("main"))
        {
            MuteGameAudio();
        }
        else
        {
            MuteMenuAudio();
        }
    }
    
    /// <summary>
    /// Método público para restaurar audio
    /// </summary>
    public void RestoreAudio()
    {
        string currentScene = SceneManager.GetActiveScene().name.ToLower();
        
        if (currentScene.Contains("menu") || currentScene.Contains("main"))
        {
            // El MainMenuAudioManager se maneja automáticamente en el menú
            Debug.Log("En menú - MainMenuAudioManager se maneja automáticamente");
        }
        else
        {
            EnsureGameAudio();
        }
    }
    
    /// <summary>
    /// Método público para restaurar música del sector (útil después de reiniciar)
    /// </summary>
    public void RestoreSectorMusic()
    {
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            // Restaurar volumen
            sectorMusicManager.SetMusicVolume(0.7f);
            
            // Verificar si la música está reproduciéndose
            if (!sectorMusicManager.IsMusicPlaying())
            {
                // Forzar inicio de música del sector 0
                sectorMusicManager.ForceStartMusic();
                Debug.Log("Música del sector restaurada e iniciada");
            }
            else
            {
                Debug.Log("Música del sector ya está reproduciéndose - solo volumen restaurado");
            }
        }
        else
        {
            Debug.LogWarning("SectorMusicManager no encontrado para restaurar música");
        }
    }
} 