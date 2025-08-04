using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja el reset completo del juego cuando se reinicia el nivel
/// </summary>
public class GameResetManager : MonoBehaviour
{
    [Header("Configuración de Reset")]
    [SerializeField] private Color defaultSkyboxColor = Color.white;
    [SerializeField] private bool resetOnSceneLoad = true;
    
    // Singleton pattern
    public static GameResetManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Desuscribirse del evento
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// Se ejecuta cuando se carga una nueva escena
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (resetOnSceneLoad && scene.name == "SampleScene")
        {
            // Esperar un frame para que todos los componentes se inicialicen
            StartCoroutine(ResetGameDelayed());
        }
    }
    
    /// <summary>
    /// Reset del juego con delay para asegurar que todos los componentes estén listos
    /// </summary>
    private System.Collections.IEnumerator ResetGameDelayed()
    {
        yield return null; // Esperar un frame
        
        Debug.Log("GameResetManager: Iniciando reset completo del juego");
        
        // 1. Resetear puntaje
        ResetScore();
        
        // 2. Resetear sector al Sector 1 (índice 0)
        ResetSector();
        
        // 3. Resetear skybox
        ResetSkybox();
        
        // 4. Resetear música al Sector 1 (índice 0)
        ResetMusic();
        
        Debug.Log("GameResetManager: Reset completo del juego completado");
    }
    
    /// <summary>
    /// Resetea el puntaje del juego
    /// </summary>
    private void ResetScore()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
            Debug.Log("GameResetManager: Puntaje reseteado a 0");
        }
        else
        {
            Debug.LogWarning("GameResetManager: ScoreManager no encontrado");
        }
    }
    
    /// <summary>
    /// Resetea el sector al sector inicial (Sector 1 - índice 0)
    /// </summary>
    private void ResetSector()
    {
        // Resetear SectorManager
        SectorManager sectorManager = FindObjectOfType<SectorManager>();
        if (sectorManager != null)
        {
            // Usar reflexión para resetear el sector interno
            var currentSectorField = typeof(SectorManager).GetField("currentSector", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (currentSectorField != null)
            {
                currentSectorField.SetValue(sectorManager, 0);
                Debug.Log("GameResetManager: Sector reseteado al Sector 1 (índice 0)");
            }
        }
        
        // Resetear PlayerShooting sector
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            // Usar reflexión para resetear el sector interno
            var sectorLevelField = typeof(PlayerShooting).GetField("sectorLevel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (sectorLevelField != null)
            {
                sectorLevelField.SetValue(playerShooting, 0);
                Debug.Log("GameResetManager: PlayerShooting sector reseteado al Sector 1 (índice 0)");
            }
        }
    }
    
    /// <summary>
    /// Resetea el skybox al color original
    /// </summary>
    private void ResetSkybox()
    {
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetColor("_Tint", defaultSkyboxColor);
            DynamicGI.UpdateEnvironment();
            Debug.Log($"GameResetManager: Skybox reseteado al color {defaultSkyboxColor}");
        }
        else
        {
            Debug.LogWarning("GameResetManager: No hay skybox configurado");
        }
    }
    
    /// <summary>
    /// Resetea la música al sector inicial (Sector 1 - índice 0) y la inicia
    /// </summary>
    private void ResetMusic()
    {
        // Resetear SectorMusicManager
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            sectorMusic.PlaySectorMusic(0);
            Debug.Log("GameResetManager: Música reseteada al Sector 1 (índice 0)");
        }
        
        // Resetear SectorMusicSetup
        SectorMusicSetup sectorMusicSetup = FindObjectOfType<SectorMusicSetup>();
        if (sectorMusicSetup != null)
        {
            sectorMusicSetup.ChangeToSector(0);
            Debug.Log("GameResetManager: SectorMusicSetup reseteado al Sector 1 (índice 0)");
        }
        
        // Forzar inicio de música después de un pequeño delay
        StartCoroutine(StartMusicDelayed());
    }
    
    /// <summary>
    /// Corrutina para iniciar la música con delay
    /// </summary>
    private System.Collections.IEnumerator StartMusicDelayed()
    {
        // Esperar un frame para asegurar que todo esté configurado
        yield return null;
        
        // Intentar iniciar música desde SectorMusicManager
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            sectorMusic.ForceStartMusic();
            Debug.Log("GameResetManager: Música iniciada desde SectorMusicManager");
        }
        
        // Intentar iniciar música desde SectorMusicSetup
        SectorMusicSetup sectorMusicSetup = FindObjectOfType<SectorMusicSetup>();
        if (sectorMusicSetup != null)
        {
            // Usar reflexión para forzar el inicio de música
            var setupSectorMusicMethod = typeof(SectorMusicSetup).GetMethod("SetupSectorMusic", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            
            if (setupSectorMusicMethod != null)
            {
                setupSectorMusicMethod.Invoke(sectorMusicSetup, null);
                Debug.Log("GameResetManager: Música iniciada desde SectorMusicSetup");
            }
        }
    }
    
    /// <summary>
    /// Método público para forzar un reset manual
    /// </summary>
    [ContextMenu("Forzar Reset del Juego")]
    public void ForceResetGame()
    {
        Debug.Log("GameResetManager: Forzando reset manual del juego");
        StartCoroutine(ResetGameDelayed());
    }
    
    /// <summary>
    /// Método para resetear solo el puntaje
    /// </summary>
    [ContextMenu("Resetear Solo Puntaje")]
    public void ResetScoreOnly()
    {
        ResetScore();
    }
    
    /// <summary>
    /// Método para resetear solo el sector
    /// </summary>
    [ContextMenu("Resetear Solo Sector")]
    public void ResetSectorOnly()
    {
        ResetSector();
        ResetSkybox();
        ResetMusic();
    }
    
    /// <summary>
    /// Método para iniciar música manualmente
    /// </summary>
    [ContextMenu("Iniciar Música")]
    public void StartMusic()
    {
        StartCoroutine(StartMusicDelayed());
    }
} 