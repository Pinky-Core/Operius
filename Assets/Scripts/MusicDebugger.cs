using UnityEngine;

public class MusicDebugger : MonoBehaviour
{
    [Header("Pruebas de Música")]
    [SerializeField] private bool testOnStart = false;
    
    private void Start()
    {
        if (testOnStart)
        {
            Invoke("TestMusicSystem", 2f);
        }
    }
    
    /// <summary>
    /// Prueba el sistema de música completo
    /// </summary>
    public void TestMusicSystem()
    {
        Debug.Log("=== PRUEBA DEL SISTEMA DE MÚSICA ===");
        
        // Verificar GameAudioManager
        if (GameAudioManager.Instance != null)
        {
            Debug.Log("✅ GameAudioManager encontrado");
            GameAudioManager.Instance.ForceStartMusic();
        }
        else
        {
            Debug.LogError("❌ GameAudioManager no encontrado");
        }
        
        // Verificar SectorMusicManager
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log("✅ SectorMusicManager encontrado");
            Debug.Log($"   - Clips configurados: {sectorMusicManager.GetMusicClipsCount()}");
            Debug.Log($"   - Música reproduciéndose: {sectorMusicManager.IsMusicPlaying()}");
            Debug.Log($"   - PlayMusicOnStart: {sectorMusicManager.GetPlayMusicOnStart()}");
            sectorMusicManager.ForceStartMusic();
        }
        else
        {
            Debug.LogError("❌ SectorMusicManager no encontrado");
        }
        
        // Verificar MusicStartupHelper
        MusicStartupHelper musicHelper = FindObjectOfType<MusicStartupHelper>();
        if (musicHelper != null)
        {
            Debug.Log("✅ MusicStartupHelper encontrado");
        }
        else
        {
            Debug.LogWarning("⚠️ MusicStartupHelper no encontrado - se creará automáticamente");
        }
        
        // Verificar AudioSceneManager
        AudioSceneManager audioSceneManager = FindObjectOfType<AudioSceneManager>();
        if (audioSceneManager != null)
        {
            Debug.Log("✅ AudioSceneManager encontrado");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioSceneManager no encontrado");
        }
        
        Debug.Log("=== FIN PRUEBA ===");
    }
    
    /// <summary>
    /// Fuerza el inicio de música (útil para probar después de reiniciar)
    /// </summary>
    public void ForceStartMusic()
    {
        Debug.Log("Forzando inicio de música...");
        
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.ForceStartMusic();
        }
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            sectorMusicManager.ForceStartMusic();
        }
    }
    
    /// <summary>
    /// Verifica el estado actual del sistema de música
    /// </summary>
    public void CheckMusicStatus()
    {
        Debug.Log("=== ESTADO DEL SISTEMA DE MÚSICA ===");
        
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log($"SectorMusicManager:");
            Debug.Log($"  - Clips configurados: {sectorMusicManager.GetMusicClipsCount()}");
            Debug.Log($"  - Música reproduciéndose: {sectorMusicManager.IsMusicPlaying()}");
            Debug.Log($"  - PlayMusicOnStart: {sectorMusicManager.GetPlayMusicOnStart()}");
            Debug.Log($"  - Volumen actual: {sectorMusicManager.GetMusicVolume()}");
            Debug.Log($"  - Sector actual: {sectorMusicManager.GetCurrentSector()}");
            Debug.Log($"  - Música actual: {sectorMusicManager.GetCurrentMusicName()}");
        }
        else
        {
            Debug.LogError("SectorMusicManager no encontrado");
        }
        
        GameAudioManager gameAudioManager = GameAudioManager.Instance;
        if (gameAudioManager != null)
        {
            Debug.Log($"GameAudioManager:");
            Debug.Log($"  - Volumen SFX: {gameAudioManager.GetSFXVolume()}");
        }
        else
        {
            Debug.LogError("GameAudioManager no encontrado");
        }
        
        Debug.Log("=== FIN ESTADO ===");
    }
    
    /// <summary>
    /// Verifica la configuración de música
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
    /// Prueba el control de volumen
    /// </summary>
    public void TestVolumeControl()
    {
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log("Probando control de volumen...");
            sectorMusicManager.SetMusicVolume(0.5f);
            Debug.Log($"Volumen actual: {sectorMusicManager.GetMusicVolume()}");
        }
    }
    
    /// <summary>
    /// Prueba el control de crossfade
    /// </summary>
    public void TestCrossfadeControl()
    {
        SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager != null)
        {
            Debug.Log("Probando control de crossfade...");
            sectorMusicManager.SetUseCrossfade(true);
            sectorMusicManager.SetCrossfadeDuration(1.5f);
        }
    }
    
    /// <summary>
    /// Restaura la música del sector (útil cuando vuelves del menú al juego)
    /// </summary>
    public void RestoreSectorMusic()
    {
        Debug.Log("Restaurando música del sector...");
        
        AudioSceneManager audioSceneManager = FindObjectOfType<AudioSceneManager>();
        if (audioSceneManager != null)
        {
            audioSceneManager.RestoreSectorMusic();
        }
        else
        {
            Debug.LogWarning("AudioSceneManager no encontrado - usando método directo");
            SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
            if (sectorMusicManager != null)
            {
                sectorMusicManager.ForceStartMusic();
            }
        }
    }
} 