using UnityEngine;

public class AudioDebugger : MonoBehaviour
{
    [Header("Debug de Audio")]
    [SerializeField] private bool debugOnStart = true;
    [SerializeField] private bool testSectorMusic = true;
    [SerializeField] private bool testGameAudio = true;
    
    private void Start()
    {
        if (debugOnStart)
        {
            DebugAudioSystem();
        }
    }
    
    /// <summary>
    /// Debug del sistema de audio
    /// </summary>
    public void DebugAudioSystem()
    {
        Debug.Log("=== DEBUG DEL SISTEMA DE AUDIO ===");
        
        // Verificar SectorMusicManager
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log("✅ SectorMusicManager encontrado");
            Debug.Log($"   - Sector actual: {sectorMusicManager.GetCurrentSector()}");
            Debug.Log($"   - Música actual: {sectorMusicManager.GetCurrentMusicName()}");
            Debug.Log($"   - Volumen: {sectorMusicManager.GetMusicVolume()}");
        }
        else
        {
            Debug.LogError("❌ SectorMusicManager NO encontrado");
        }
        
        // Verificar GameAudioManager
        GameAudioManager gameAudioManager = FindObjectOfType<GameAudioManager>();
        if (gameAudioManager != null)
        {
            Debug.Log("✅ GameAudioManager encontrado");
            Debug.Log($"   - Volumen SFX: {gameAudioManager.GetSFXVolume()}");
        }
        else
        {
            Debug.LogError("❌ GameAudioManager NO encontrado");
        }
        
        // Verificar MainMenuAudioManager
        MainMenuAudioManager menuAudioManager = FindObjectOfType<MainMenuAudioManager>();
        if (menuAudioManager != null)
        {
            Debug.Log("✅ MainMenuAudioManager encontrado");
            Debug.Log($"   - Música actual: {menuAudioManager.GetCurrentMusicName()}");
        }
        else
        {
            Debug.Log("ℹ️ MainMenuAudioManager NO encontrado (normal si estás en el juego)");
        }
        
        // Verificar AudioSceneManager
        AudioSceneManager audioSceneManager = FindObjectOfType<AudioSceneManager>();
        if (audioSceneManager != null)
        {
            Debug.Log("✅ AudioSceneManager encontrado");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioSceneManager NO encontrado");
        }
        
        Debug.Log("=== FIN DEBUG ===");
    }
    
    /// <summary>
    /// Probar música por sectores
    /// </summary>
    public void TestSectorMusic()
    {
        Debug.Log("=== PROBANDO MÚSICA POR SECTORES ===");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            Debug.LogError("SectorMusicManager no encontrado");
            return;
        }
        
        // Probar diferentes sectores
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"Probando sector {i}");
            sectorMusicManager.PlaySectorMusic(i);
        }
        
        Debug.Log("=== FIN PRUEBA MÚSICA ===");
    }
    
    /// <summary>
    /// Probar efectos de sonido del juego
    /// </summary>
    public void TestGameAudio()
    {
        Debug.Log("=== PROBANDO EFECTOS DE SONIDO ===");
        
        GameAudioManager gameAudioManager = FindObjectOfType<GameAudioManager>();
        if (gameAudioManager == null)
        {
            Debug.LogError("GameAudioManager no encontrado");
            return;
        }
        
        // Probar diferentes efectos
        Debug.Log("Probando sonido de disparo");
        gameAudioManager.PlayShootSound();
        
        Debug.Log("Probando sonido de powerup");
        gameAudioManager.PlayPowerupCollectSound();
        
        Debug.Log("Probando sonido de muerte");
        gameAudioManager.PlayDeathSound();
        
        Debug.Log("=== FIN PRUEBA EFECTOS ===");
    }
    
    /// <summary>
    /// Forzar reproducción de música del sector 0
    /// </summary>
    public void ForcePlaySector0()
    {
        Debug.Log("Forzando reproducción de música del sector 0");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            sectorMusicManager.PlaySectorMusic(0);
        }
        else
        {
            Debug.LogError("SectorMusicManager no encontrado");
        }
    }
    
    /// <summary>
    /// Verificar configuración de música
    /// </summary>
    public void CheckMusicConfiguration()
    {
        Debug.Log("=== VERIFICANDO CONFIGURACIÓN DE MÚSICA ===");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            Debug.LogError("SectorMusicManager no encontrado");
            return;
        }
        
        // Usar reflexión para acceder a sectorMusicClips
        var sectorMusicClipsField = typeof(SectorMusicManager).GetField("sectorMusicClips", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (sectorMusicClipsField != null)
        {
            var sectorMusicClips = sectorMusicClipsField.GetValue(sectorMusicManager) as AudioClip[];
            
            if (sectorMusicClips != null)
            {
                Debug.Log($"Total de clips configurados: {sectorMusicClips.Length}");
                
                for (int i = 0; i < sectorMusicClips.Length; i++)
                {
                    var clip = sectorMusicClips[i];
                    Debug.Log($"Sector {i}: {(clip != null ? clip.name : "NULL")}");
                }
            }
            else
            {
                Debug.LogError("sectorMusicClips es null");
            }
        }
        else
        {
            Debug.LogError("No se pudo acceder al campo sectorMusicClips");
        }
        
        Debug.Log("=== FIN VERIFICACIÓN ===");
    }
    
    /// <summary>
    /// Crear SectorMusicManager si no existe
    /// </summary>
    public void CreateSectorMusicManager()
    {
        Debug.Log("Creando SectorMusicManager");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            GameObject sectorMusicObj = new GameObject("SectorMusicManager");
            sectorMusicManager = sectorMusicObj.AddComponent<SectorMusicManager>();
            Debug.Log("SectorMusicManager creado");
        }
        else
        {
            Debug.Log("SectorMusicManager ya existe");
        }
    }
    
    /// <summary>
    /// Crear GameAudioManager si no existe
    /// </summary>
    public void CreateGameAudioManager()
    {
        Debug.Log("Creando GameAudioManager");
        
        GameAudioManager gameAudioManager = FindObjectOfType<GameAudioManager>();
        if (gameAudioManager == null)
        {
            GameObject gameAudioObj = new GameObject("GameAudioManager");
            gameAudioManager = gameAudioObj.AddComponent<GameAudioManager>();
            Debug.Log("GameAudioManager creado");
        }
        else
        {
            Debug.Log("GameAudioManager ya existe");
        }
    }
    
    /// <summary>
    /// Probar control de volumen
    /// </summary>
    public void TestVolumeControl()
    {
        Debug.Log("=== PROBANDO CONTROL DE VOLUMEN ===");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log($"Volumen actual: {sectorMusicManager.GetMusicVolume()}");
            
            // Probar diferentes volúmenes
            sectorMusicManager.SetMusicVolume(0.3f);
            Debug.Log("Volumen cambiado a 0.3");
            
            sectorMusicManager.SetMusicVolume(0.7f);
            Debug.Log("Volumen cambiado a 0.7");
        }
        else
        {
            Debug.LogError("SectorMusicManager no encontrado");
        }
        
        Debug.Log("=== FIN PRUEBA VOLUMEN ===");
    }
    
    /// <summary>
    /// Probar control de crossfade
    /// </summary>
    public void TestCrossfadeControl()
    {
        Debug.Log("=== PROBANDO CONTROL DE CROSSFADE ===");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            // Deshabilitar crossfade
            sectorMusicManager.SetUseCrossfade(false);
            Debug.Log("Crossfade deshabilitado");
            
            // Cambiar a sector 1
            sectorMusicManager.PlaySectorMusic(1);
            
            // Habilitar crossfade
            sectorMusicManager.SetUseCrossfade(true);
            Debug.Log("Crossfade habilitado");
            
            // Cambiar a sector 2
            sectorMusicManager.PlaySectorMusic(2);
        }
        else
        {
            Debug.LogError("SectorMusicManager no encontrado");
        }
        
        Debug.Log("=== FIN PRUEBA CROSSFADE ===");
    }
} 