using UnityEngine;

public class SectorMusicSetup : MonoBehaviour
{
    [Header("Configuración Rápida")]
    [SerializeField] private bool setupOnStart = true;
    [SerializeField] private bool useCrossfade = true;
    
    [Header("Configuración de Música por Sectores")]
    [SerializeField] private AudioClip[] sectorMusicClips;
    [SerializeField] private float musicVolume = 0.7f;
    
    [Header("Configuración de Transiciones")]
    [SerializeField] private float crossfadeDuration = 2f;
    [SerializeField] private bool playMusicOnStart = true;
    
    private SectorMusicManager sectorMusicManager;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupSectorMusic();
        }
    }
    
    /// <summary>
    /// Configura el sistema de música por sectores
    /// </summary>
    public void SetupSectorMusic()
    {
        // Buscar o crear el SectorMusicManager
        sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            GameObject sectorMusicObj = new GameObject("SectorMusicManager");
            sectorMusicManager = sectorMusicObj.AddComponent<SectorMusicManager>();
        }
        
        // Configurar música por sectores usando reflexión
        SetupSectorMusicClips();
        
        Debug.Log("Sistema de música por sectores configurado correctamente");
    }
    
    /// <summary>
    /// Configura los clips de música por sectores
    /// </summary>
    private void SetupSectorMusicClips()
    {
        if (sectorMusicClips == null || sectorMusicClips.Length == 0) return;
        
        // Asignar el array al SectorMusicManager usando reflexión
        var sectorMusicClipsField = typeof(SectorMusicManager).GetField("sectorMusicClips", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (sectorMusicClipsField != null)
        {
            sectorMusicClipsField.SetValue(sectorMusicManager, sectorMusicClips);
            Debug.Log($"Configurados {sectorMusicClips.Length} clips de música");
        }
        else
        {
            Debug.LogError("No se pudo acceder al campo sectorMusicClips");
        }
        
        // Configurar otras propiedades
        var musicVolumeField = typeof(SectorMusicManager).GetField("musicVolume", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var useCrossfadeField = typeof(SectorMusicManager).GetField("useCrossfade", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var crossfadeDurationField = typeof(SectorMusicManager).GetField("crossfadeDuration", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var playMusicOnStartField = typeof(SectorMusicManager).GetField("playMusicOnStart", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (musicVolumeField != null) musicVolumeField.SetValue(sectorMusicManager, musicVolume);
        if (useCrossfadeField != null) useCrossfadeField.SetValue(sectorMusicManager, useCrossfade);
        if (crossfadeDurationField != null) crossfadeDurationField.SetValue(sectorMusicManager, crossfadeDuration);
        if (playMusicOnStartField != null) playMusicOnStartField.SetValue(sectorMusicManager, playMusicOnStart);
        
        Debug.Log($"Configurado - Volumen: {musicVolume}, Crossfade: {useCrossfade}, Duración: {crossfadeDuration}, PlayOnStart: {playMusicOnStart}");
        
        // Forzar inicio de música después de configurar (solo si playMusicOnStart está habilitado)
        if (playMusicOnStart)
        {
            sectorMusicManager.ForceStartMusic();
        }
        else
        {
            Debug.Log("SectorMusicSetup: playMusicOnStart está deshabilitado - no se iniciará música automáticamente");
        }
    }
    
    /// <summary>
    /// Prueba el cambio de música entre sectores
    /// </summary>
    public void TestSectorMusic()
    {
        if (sectorMusicManager == null) return;
        
        Debug.Log("Probando música por sectores...");
        
        // Probar diferentes sectores
        for (int i = 0; i < Mathf.Min(5, sectorMusicClips.Length); i++)
        {
            int sector = i;
            StartCoroutine(TestSectorDelayed(sector, i * 3f));
        }
    }
    
    /// <summary>
    /// Corrutina para probar sector con delay
    /// </summary>
    private System.Collections.IEnumerator TestSectorDelayed(int sector, float delay)
    {
        yield return new WaitForSeconds(delay);
        sectorMusicManager.PlaySectorMusic(sector);
        Debug.Log($"Cambiando a sector {sector}");
    }
    
    /// <summary>
    /// Cambia al sector especificado
    /// </summary>
    public void ChangeToSector(int sectorLevel)
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.PlaySectorMusic(sectorLevel);
        }
    }
    
    /// <summary>
    /// Obtiene el sector actual
    /// </summary>
    public int GetCurrentSector()
    {
        if (sectorMusicManager != null)
        {
            return sectorMusicManager.GetCurrentSector();
        }
        return 0;
    }
    
    /// <summary>
    /// Obtiene el nombre de la música actual
    /// </summary>
    public string GetCurrentMusicName()
    {
        if (sectorMusicManager != null)
        {
            return sectorMusicManager.GetCurrentMusicName();
        }
        return "Sin música";
    }
    
    /// <summary>
    /// Detiene la música actual
    /// </summary>
    public void StopMusic()
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.StopMusic();
        }
    }
    
    /// <summary>
    /// Pausa la música actual
    /// </summary>
    public void PauseMusic()
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.PauseMusic();
        }
    }
    
    /// <summary>
    /// Reanuda la música actual
    /// </summary>
    public void ResumeMusic()
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.ResumeMusic();
        }
    }
    
    /// <summary>
    /// Ajusta el volumen de la música
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.SetMusicVolume(volume);
        }
    }
    
    /// <summary>
    /// Obtiene el volumen actual
    /// </summary>
    public float GetMusicVolume()
    {
        if (sectorMusicManager != null)
        {
            return sectorMusicManager.GetMusicVolume();
        }
        return 0f;
    }
    
    /// <summary>
    /// Habilita o deshabilita el crossfade
    /// </summary>
    public void SetUseCrossfade(bool use)
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.SetUseCrossfade(use);
        }
    }
    
    /// <summary>
    /// Ajusta la duración del crossfade
    /// </summary>
    public void SetCrossfadeDuration(float duration)
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.SetCrossfadeDuration(duration);
        }
    }
    
    /// <summary>
    /// Habilita o deshabilita la reproducción automática de música al iniciar
    /// </summary>
    public void SetPlayMusicOnStart(bool playOnStart)
    {
        playMusicOnStart = playOnStart;
        if (sectorMusicManager != null)
        {
            sectorMusicManager.SetPlayMusicOnStart(playOnStart);
        }
    }
    
    /// <summary>
    /// Obtiene si la música se reproduce automáticamente al iniciar
    /// </summary>
    public bool GetPlayMusicOnStart()
    {
        return playMusicOnStart;
    }
} 