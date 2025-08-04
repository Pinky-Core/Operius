using UnityEngine;
using TMPro;

/// <summary>
/// Script de ejemplo para demostrar el uso del GameResetManager
/// </summary>
public class GameResetExample : MonoBehaviour
{
    [Header("UI de Testing")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI sectorText;
    
    [Header("Configuración")]
    [SerializeField] private bool showDebugInfo = true;
    
    private void Start()
    {
        UpdateStatusText("GameResetExample iniciado");
    }
    
    private void Update()
    {
        if (showDebugInfo)
        {
            UpdateDebugInfo();
        }
    }
    
    /// <summary>
    /// Actualiza la información de debug en tiempo real
    /// </summary>
    private void UpdateDebugInfo()
    {
        if (scoreText != null)
        {
            int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetCurrentScore() : 0;
            scoreText.text = $"Score: {currentScore}";
        }
        
        if (sectorText != null)
        {
            SectorManager sectorManager = FindObjectOfType<SectorManager>();
            if (sectorManager != null)
            {
                // Usar reflexión para obtener el sector actual
                var currentSectorField = typeof(SectorManager).GetField("currentSector", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (currentSectorField != null)
                {
                    int currentSector = (int)currentSectorField.GetValue(sectorManager);
                    sectorText.text = $"Sector: {currentSector + 1:D3} (índice {currentSector})";
                }
            }
        }
    }
    
    /// <summary>
    /// Actualiza el texto de estado
    /// </summary>
    private void UpdateStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            Debug.Log($"GameResetExample: {message}");
        }
    }
    
    /// <summary>
    /// Método de contexto para forzar un reset completo
    /// </summary>
    [ContextMenu("Forzar Reset Completo")]
    public void ForceCompleteReset()
    {
        if (GameResetManager.Instance != null)
        {
            GameResetManager.Instance.ForceResetGame();
            UpdateStatusText("Reset completo forzado");
        }
        else
        {
            UpdateStatusText("ERROR: GameResetManager no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para resetear solo el puntaje
    /// </summary>
    [ContextMenu("Resetear Solo Puntaje")]
    public void ResetScoreOnly()
    {
        if (GameResetManager.Instance != null)
        {
            GameResetManager.Instance.ResetScoreOnly();
            UpdateStatusText("Solo puntaje reseteado");
        }
        else
        {
            UpdateStatusText("ERROR: GameResetManager no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para resetear solo el sector
    /// </summary>
    [ContextMenu("Resetear Solo Sector")]
    public void ResetSectorOnly()
    {
        if (GameResetManager.Instance != null)
        {
            GameResetManager.Instance.ResetSectorOnly();
            UpdateStatusText("Solo sector reseteado");
        }
        else
        {
            UpdateStatusText("ERROR: GameResetManager no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para mostrar información del sistema
    /// </summary>
    [ContextMenu("Mostrar Info del Sistema")]
    public void ShowSystemInfo()
    {
        string info = "=== INFORMACIÓN DEL SISTEMA ===\n";
        
        // ScoreManager
        if (ScoreManager.Instance != null)
        {
            info += $"✅ ScoreManager: {ScoreManager.Instance.GetCurrentScore()} puntos\n";
        }
        else
        {
            info += "❌ ScoreManager: No encontrado\n";
        }
        
        // SectorManager
        SectorManager sectorManager = FindObjectOfType<SectorManager>();
        if (sectorManager != null)
        {
            info += "✅ SectorManager: Encontrado\n";
        }
        else
        {
            info += "❌ SectorManager: No encontrado\n";
        }
        
        // PlayerShooting
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            info += "✅ PlayerShooting: Encontrado\n";
        }
        else
        {
            info += "❌ PlayerShooting: No encontrado\n";
        }
        
        // SectorMusicManager
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            int sectorIndex = sectorMusic.GetCurrentSector();
            info += $"✅ SectorMusicManager: Sector {sectorIndex + 1} (índice {sectorIndex})\n";
        }
        else
        {
            info += "❌ SectorMusicManager: No encontrado\n";
        }
        
        // SectorMusicSetup
        SectorMusicSetup sectorMusicSetup = FindObjectOfType<SectorMusicSetup>();
        if (sectorMusicSetup != null)
        {
            int sectorIndex = sectorMusicSetup.GetCurrentSector();
            info += $"✅ SectorMusicSetup: Sector {sectorIndex + 1} (índice {sectorIndex})\n";
        }
        else
        {
            info += "❌ SectorMusicSetup: No encontrado\n";
        }
        
        // GameResetManager
        if (GameResetManager.Instance != null)
        {
            info += "✅ GameResetManager: Activo\n";
        }
        else
        {
            info += "❌ GameResetManager: No encontrado\n";
        }
        
        // Skybox
        if (RenderSettings.skybox != null)
        {
            Color skyboxColor = RenderSettings.skybox.GetColor("_Tint");
            info += $"✅ Skybox: Color {skyboxColor}\n";
        }
        else
        {
            info += "❌ Skybox: No configurado\n";
        }
        
        // Highscore
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        info += $"✅ Highscore: {highscore}\n";
        
        // Monedas
        int coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        info += $"✅ Monedas: {coins}\n";
        
        UpdateStatusText(info);
        Debug.Log(info);
    }
    
    /// <summary>
    /// Método de contexto para simular muerte del jugador
    /// </summary>
    [ContextMenu("Simular Muerte del Jugador")]
    public void SimulatePlayerDeath()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            // Simular colisión con enemigo
            GameObject enemy = new GameObject("TestEnemy");
            enemy.tag = "Enemy";
            
            // Trigger la muerte
            player.SendMessage("OnTriggerEnter", enemy.GetComponent<Collider>());
            
            Destroy(enemy);
            UpdateStatusText("Muerte del jugador simulada");
        }
        else
        {
            UpdateStatusText("ERROR: PlayerController no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para agregar puntos de prueba
    /// </summary>
    [ContextMenu("Agregar 100 Puntos de Prueba")]
    public void AddTestPoints()
    {
        if (ScoreManager.Instance != null)
        {
            // Crear un objeto de prueba con tag que otorga puntos
            GameObject testObject = new GameObject("TestPoints");
            testObject.tag = "Enemy"; // Asumiendo que los enemigos dan puntos
            
            ScoreManager.Instance.AddPointsFrom(testObject);
            
            Destroy(testObject);
            UpdateStatusText("100 puntos agregados de prueba");
        }
        else
        {
            UpdateStatusText("ERROR: ScoreManager no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para cambiar de sector
    /// </summary>
    [ContextMenu("Cambiar a Sector 2")]
    public void ChangeToSector2()
    {
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            // Simular cambio de sector
            playerShooting.SendMessage("OnSectorLevelUp", 1); // Sector 2 (índice 1)
            UpdateStatusText("Cambiado a Sector 2 (índice 1)");
        }
        else
        {
            UpdateStatusText("ERROR: PlayerShooting no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para cambiar al Sector 1
    /// </summary>
    [ContextMenu("Cambiar a Sector 1")]
    public void ChangeToSector1()
    {
        PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting != null)
        {
            // Simular cambio de sector
            playerShooting.SendMessage("OnSectorLevelUp", 0); // Sector 1 (índice 0)
            UpdateStatusText("Cambiado a Sector 1 (índice 0)");
        }
        else
        {
            UpdateStatusText("ERROR: PlayerShooting no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para iniciar música
    /// </summary>
    [ContextMenu("Iniciar Música")]
    public void StartMusic()
    {
        if (GameResetManager.Instance != null)
        {
            GameResetManager.Instance.StartMusic();
            UpdateStatusText("Música iniciada manualmente");
        }
        else
        {
            UpdateStatusText("ERROR: GameResetManager no encontrado");
        }
    }
    
    /// <summary>
    /// Método de contexto para detener música
    /// </summary>
    [ContextMenu("Detener Música")]
    public void StopMusic()
    {
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            sectorMusic.StopMusic();
            UpdateStatusText("Música detenida");
        }
        else
        {
            UpdateStatusText("ERROR: SectorMusicManager no encontrado");
        }
    }
} 