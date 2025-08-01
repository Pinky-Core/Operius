using UnityEngine;
using System.Collections;

public class SectorMusicManager : MonoBehaviour
{
    [Header("Música por Sectores")]
    [SerializeField] private AudioClip[] sectorMusicClips;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float musicVolume = 0.7f;
    
    [Header("Configuración")]
    [SerializeField] private bool useCrossfade = true;
    [SerializeField] private float crossfadeDuration = 2f;
    [SerializeField] private bool playMusicOnStart = true;
    
    // Singleton pattern
    public static SectorMusicManager Instance { get; private set; }
    
    private int currentSector = 0;
    private bool isTransitioning = false;
    private Coroutine musicTransitionCoroutine;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Configurar AudioSource si no está asignado
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configurar música
        musicSource.volume = musicVolume;
        musicSource.loop = true;
    }
    
    private void Start()
    {
        // Solo iniciar música si playMusicOnStart está habilitado
        if (playMusicOnStart)
        {
            // Esperar un frame para asegurar que SectorMusicSetup haya configurado los clips
            StartCoroutine(StartMusicDelayed());
        }
        else
        {
            Debug.Log("SectorMusicManager: playMusicOnStart está deshabilitado - música no iniciará automáticamente");
        }
    }
    
    private IEnumerator StartMusicDelayed()
    {
        // Esperar un frame para que SectorMusicSetup configure los clips
        yield return null;
        
        // Verificar que los clips estén configurados
        if (sectorMusicClips != null && sectorMusicClips.Length > 0)
        {
            Debug.Log($"SectorMusicManager: Iniciando música del sector 0. Clips disponibles: {sectorMusicClips.Length}");
            PlaySectorMusic(0);
        }
        else
        {
            Debug.LogWarning("SectorMusicManager: No hay clips de música configurados. Esperando configuración...");
            // Esperar hasta que los clips estén configurados
            yield return new WaitForSeconds(0.1f);
            
            if (sectorMusicClips != null && sectorMusicClips.Length > 0)
            {
                Debug.Log($"SectorMusicManager: Clips configurados. Iniciando música del sector 0");
                PlaySectorMusic(0);
            }
            else
            {
                Debug.LogError("SectorMusicManager: No se pudieron configurar los clips de música");
            }
        }
    }
    
    /// <summary>
    /// Fuerza la reproducción de música del sector 0 (útil después de reiniciar)
    /// </summary>
    public void ForceStartMusic()
    {
        Debug.Log("SectorMusicManager: Forzando inicio de música del sector 0");
        
        if (!HasMusicClips())
        {
            Debug.LogError("SectorMusicManager: No hay clips configurados para reproducir música");
            return;
        }
        
        // Verificar que el AudioSource esté configurado
        if (musicSource == null)
        {
            Debug.LogError("SectorMusicManager: AudioSource es null");
            return;
        }
        
        // Asegurar que el volumen no sea 0
        if (musicSource.volume <= 0f)
        {
            musicSource.volume = musicVolume;
        }
        
        PlaySectorMusic(0);
    }
    
    /// <summary>
    /// Reproduce la música del sector especificado
    /// </summary>
    public void PlaySectorMusic(int sectorLevel)
    {
        Debug.Log($"PlaySectorMusic llamado con sector: {sectorLevel}");
        
        if (isTransitioning) 
        {
            Debug.Log("Ya hay una transición en curso, ignorando");
            return;
        }
        
        // Verificar que el sector esté dentro del rango
        if (sectorLevel < 0 || sectorLevel >= sectorMusicClips.Length)
        {
            Debug.LogError($"Sector {sectorLevel} fuera de rango. Total de clips: {sectorMusicClips.Length}");
            return;
        }
        
        AudioClip newMusic = sectorMusicClips[sectorLevel];
        if (newMusic == null)
        {
            Debug.LogError($"El clip de música para el sector {sectorLevel} es null");
            return;
        }
        
        Debug.Log($"Reproduciendo música del sector {sectorLevel}: {newMusic.name}");
        
        currentSector = sectorLevel;
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        if (useCrossfade && musicSource.isPlaying)
        {
            musicTransitionCoroutine = StartCoroutine(CrossfadeToMusic(newMusic));
        }
        else
        {
            musicTransitionCoroutine = StartCoroutine(FadeToMusic(newMusic));
        }
    }
    
    /// <summary>
    /// Transición con crossfade
    /// </summary>
    private IEnumerator CrossfadeToMusic(AudioClip newMusic)
    {
        isTransitioning = true;
        
        // Crear un segundo AudioSource para el crossfade
        AudioSource newMusicSource = gameObject.AddComponent<AudioSource>();
        newMusicSource.clip = newMusic;
        newMusicSource.loop = true;
        newMusicSource.volume = 0f;
        newMusicSource.Play();
        
        float elapsedTime = 0f;
        float startVolume = musicSource.volume;
        
        while (elapsedTime < crossfadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / crossfadeDuration;
            
            // Fade out música actual
            musicSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            
            // Fade in nueva música
            newMusicSource.volume = Mathf.Lerp(0f, musicVolume, progress);
            
            yield return null;
        }
        
        // Reemplazar AudioSource
        Destroy(musicSource);
        musicSource = newMusicSource;
        
        isTransitioning = false;
        
        Debug.Log($"Cambio completado a música del sector {currentSector}: {newMusic.name}");
    }
    
    /// <summary>
    /// Transición simple con fade
    /// </summary>
    private IEnumerator FadeToMusic(AudioClip newMusic)
    {
        isTransitioning = true;
        
        // Fade out música actual
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float elapsedTime = 0f;
            
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / 1f;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, progress);
                yield return null;
            }
        }
        
        // Cambiar música
        musicSource.clip = newMusic;
        musicSource.volume = 0f;
        musicSource.Play();
        
        // Fade in nueva música
        float fadeInElapsed = 0f;
        while (fadeInElapsed < 1f)
        {
            fadeInElapsed += Time.deltaTime;
            float progress = fadeInElapsed / 1f;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, progress);
            yield return null;
        }
        
        isTransitioning = false;
        
        Debug.Log($"Cambio completado a música del sector {currentSector}: {newMusic.name}");
    }
    
    /// <summary>
    /// Detiene la música actual
    /// </summary>
    public void StopMusic()
    {
        Debug.Log("SectorMusicManager: Deteniendo música");
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
            Debug.Log("Corrutina de transición detenida");
        }
        
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = 0f;
            Debug.Log("AudioSource detenido y volumen puesto a 0");
        }
        else
        {
            Debug.LogWarning("AudioSource es null");
        }
        
        isTransitioning = false;
        
        Debug.Log("Música detenida completamente");
    }
    
    /// <summary>
    /// Baja el volumen de la música gradualmente
    /// </summary>
    public void FadeOutMusic(float fadeDuration = 1f)
    {
        Debug.Log($"SectorMusicManager: Bajando volumen de música en {fadeDuration}s");
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        musicTransitionCoroutine = StartCoroutine(FadeOutMusicCoroutine(fadeDuration));
    }
    
    /// <summary>
    /// Corrutina para bajar el volumen de la música
    /// </summary>
    private IEnumerator FadeOutMusicCoroutine(float fadeDuration)
    {
        if (musicSource == null || !musicSource.isPlaying)
        {
            Debug.Log("SectorMusicManager: No hay música reproduciéndose para bajar volumen");
            yield break;
        }
        
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;
        
        Debug.Log($"SectorMusicManager: Iniciando fade out de {startVolume} a 0 en {fadeDuration}s");
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            musicSource.volume = newVolume;
            yield return null;
        }
        
        musicSource.volume = 0f;
        Debug.Log("SectorMusicManager: Volumen de música bajado completamente");
    }
    
    /// <summary>
    /// Restaura el volumen de la música gradualmente
    /// </summary>
    public void FadeInMusic(float fadeDuration = 1f)
    {
        Debug.Log($"SectorMusicManager: Restaurando volumen de música en {fadeDuration}s");
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        musicTransitionCoroutine = StartCoroutine(FadeInMusicCoroutine(fadeDuration));
    }
    
    /// <summary>
    /// Corrutina para restaurar el volumen de la música
    /// </summary>
    private IEnumerator FadeInMusicCoroutine(float fadeDuration)
    {
        if (musicSource == null)
        {
            Debug.Log("SectorMusicManager: AudioSource es null");
            yield break;
        }
        
        float startVolume = musicSource.volume;
        float targetVolume = musicVolume;
        float elapsedTime = 0f;
        
        Debug.Log($"SectorMusicManager: Iniciando fade in de {startVolume} a {targetVolume} en {fadeDuration}s");
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeDuration);
            musicSource.volume = newVolume;
            yield return null;
        }
        
        musicSource.volume = targetVolume;
        Debug.Log("SectorMusicManager: Volumen de música restaurado completamente");
    }
    
    /// <summary>
    /// Fuerza la detención inmediata de la música
    /// </summary>
    public void ForceStopMusic()
    {
        Debug.Log("SectorMusicManager: Forzando detención de música");
        
        // Detener todas las corrutinas
        StopAllCoroutines();
        
        // Detener y limpiar AudioSource
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = 0f;
            musicSource.clip = null;
            Debug.Log("AudioSource forzado a detener");
        }
        
        isTransitioning = false;
        
        Debug.Log("Música forzada a detener completamente");
    }
    
    /// <summary>
    /// Pausa la música actual
    /// </summary>
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    /// <summary>
    /// Reanuda la música actual
    /// </summary>
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    
    /// <summary>
    /// Obtiene el sector actual
    /// </summary>
    public int GetCurrentSector()
    {
        return currentSector;
    }
    
    /// <summary>
    /// Obtiene el nombre de la música actual
    /// </summary>
    public string GetCurrentMusicName()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            return musicSource.clip.name;
        }
        return "Sin música";
    }
    
    /// <summary>
    /// Ajusta el volumen de la música
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    /// <summary>
    /// Obtiene el volumen actual
    /// </summary>
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    
    /// <summary>
    /// Habilita o deshabilita el crossfade
    /// </summary>
    public void SetUseCrossfade(bool use)
    {
        useCrossfade = use;
    }
    
    /// <summary>
    /// Ajusta la duración del crossfade
    /// </summary>
    public void SetCrossfadeDuration(float duration)
    {
        crossfadeDuration = Mathf.Max(0.1f, duration);
    }
    
    /// <summary>
    /// Habilita o deshabilita la reproducción automática de música al iniciar
    /// </summary>
    public void SetPlayMusicOnStart(bool playOnStart)
    {
        playMusicOnStart = playOnStart;
        Debug.Log($"SectorMusicManager: playMusicOnStart establecido en {playOnStart}");
    }
    
    /// <summary>
    /// Obtiene si la música se reproduce automáticamente al iniciar
    /// </summary>
    public bool GetPlayMusicOnStart()
    {
        return playMusicOnStart;
    }
    
    /// <summary>
    /// Verifica si la música está reproduciéndose actualmente
    /// </summary>
    public bool IsMusicPlaying()
    {
        if (musicSource != null)
        {
            return musicSource.isPlaying && musicSource.volume > 0f;
        }
        return false;
    }
    
    /// <summary>
    /// Verifica si hay clips de música configurados
    /// </summary>
    public bool HasMusicClips()
    {
        return sectorMusicClips != null && sectorMusicClips.Length > 0;
    }
    
    /// <summary>
    /// Obtiene el número de clips de música configurados
    /// </summary>
    public int GetMusicClipsCount()
    {
        return sectorMusicClips != null ? sectorMusicClips.Length : 0;
    }
} 