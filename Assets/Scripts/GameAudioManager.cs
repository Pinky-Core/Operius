using UnityEngine;
using System.Collections;

public class GameAudioManager : MonoBehaviour
{
    [Header("Efectos de Sonido del Juego")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip powerupCollectSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip gameOverSound;
    
    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private float sfxVolume = 0.8f;
    [SerializeField] private bool useGeneratedSounds = true;
    
    [Header("Configuración de Efectos Generados")]
    [SerializeField] private float shootFrequency = 800f;
    [SerializeField] private float deathFrequency = 200f;
    [SerializeField] private float powerupFrequency = 1200f;
    
    [Header("Configuración de Prevención de Superposición")]
    [SerializeField] private bool preventSoundOverlap = false;
    [SerializeField] private float minTimeBetweenSounds = 0.1f;
    
    // Singleton pattern
    public static GameAudioManager Instance { get; private set; }
    
    private SimpleSoundEffects soundEffects;
    private SectorMusicManager sectorMusicManager;
    private float lastSoundTime = 0f;
    
    private void Awake()
    {
        // Singleton pattern (sin DontDestroyOnLoad para permitir destrucción entre escenas)
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
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configurar efectos de sonido
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
        
        // Inicializar generador de efectos
        if (useGeneratedSounds)
        {
            soundEffects = gameObject.AddComponent<SimpleSoundEffects>();
            SetupGeneratedSounds();
        }
        
        // Buscar o crear el SectorMusicManager
        sectorMusicManager = FindObjectOfType<SectorMusicManager>();
        if (sectorMusicManager == null)
        {
            GameObject sectorMusicObj = new GameObject("SectorMusicManager");
            sectorMusicManager = sectorMusicObj.AddComponent<SectorMusicManager>();
            DontDestroyOnLoad(sectorMusicObj);
        }
    }
    
    private void Start()
    {
        // Cargar volumen de SFX desde las opciones
        LoadSFXVolume();
    }
    
    /// <summary>
    /// Carga el volumen de SFX desde OptionsManager
    /// </summary>
    private void LoadSFXVolume()
    {
        if (OptionsManager.Instance != null)
        {
            sfxVolume = OptionsManager.Instance.GetSFXVolume();
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }
    }
    
    private void SetupGeneratedSounds()
    {
        if (soundEffects == null) return;
        
        // Generar efectos de sonido si no están asignados
        if (shootSound == null)
        {
            shootSound = soundEffects.CreateShootSound(shootFrequency, 0.4f);
        }
        
        if (deathSound == null)
        {
            deathSound = soundEffects.CreateDeathSound(deathFrequency, 0.6f);
        }
        
        if (powerupCollectSound == null)
        {
            powerupCollectSound = soundEffects.CreatePowerupSound(powerupFrequency, 0.5f);
        }
        
        if (enemyDeathSound == null)
        {
            enemyDeathSound = soundEffects.CreateEnemyDeathSound(0.3f);
        }
        
        if (gameOverSound == null)
        {
            gameOverSound = soundEffects.CreateGameOverSound(0.7f);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de disparo sin superposición
    /// </summary>
    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            if (preventSoundOverlap)
            {
                if (Time.time - lastSoundTime >= minTimeBetweenSounds)
                {
                    PlaySoundImmediately(shootSound);
                }
            }
            else
            {
                PlaySoundImmediately(shootSound);
            }
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de muerte del jugador
    /// </summary>
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            PlaySoundImmediately(deathSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de recolección de powerup
    /// </summary>
    public void PlayPowerupCollectSound()
    {
        if (powerupCollectSound != null)
        {
            PlaySoundImmediately(powerupCollectSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de muerte de enemigo
    /// </summary>
    public void PlayEnemyDeathSound()
    {
        if (enemyDeathSound != null)
        {
            PlaySoundImmediately(enemyDeathSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de game over
    /// </summary>
    public void PlayGameOverSound()
    {
        if (gameOverSound != null)
        {
            PlaySoundImmediately(gameOverSound);
        }
    }
    
    /// <summary>
    /// Reproduce un sonido inmediatamente
    /// </summary>
    private void PlaySoundImmediately(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
            lastSoundTime = Time.time;
        }
    }
    
    /// <summary>
    /// Reproduce un efecto de sonido personalizado
    /// </summary>
    public void PlayCustomSound(AudioClip clip)
    {
        if (clip != null)
        {
            PlaySoundImmediately(clip);
        }
    }
    
    /// <summary>
    /// Cambia la música al sector especificado
    /// </summary>
    public void ChangeToSector(int sectorLevel)
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.PlaySectorMusic(sectorLevel);
        }
    }
    
    /// <summary>
    /// Detiene la música del juego (para volver al menú)
    /// </summary>
    public void StopGameMusic()
    {
        Debug.Log("GameAudioManager: Deteniendo música del juego");
        
        if (sectorMusicManager != null)
        {
            sectorMusicManager.ForceStopMusic();
        }
        else
        {
            Debug.LogWarning("SectorMusicManager no encontrado");
        }
    }
    
    /// <summary>
    /// Baja el volumen de todos los efectos de sonido gradualmente
    /// </summary>
    public void FadeOutAllSoundEffects(float fadeDuration = 1f)
    {
        Debug.Log($"GameAudioManager: Bajando volumen de efectos de sonido en {fadeDuration}s");
        
        if (sfxSource != null)
        {
            StartCoroutine(FadeOutSoundEffects(fadeDuration));
        }
    }
    
    /// <summary>
    /// Corrutina para bajar el volumen de efectos de sonido
    /// </summary>
    private IEnumerator FadeOutSoundEffects(float fadeDuration)
    {
        float startVolume = sfxSource.volume;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            sfxSource.volume = newVolume;
            yield return null;
        }
        
        sfxSource.volume = 0f;
        Debug.Log("GameAudioManager: Volumen de efectos de sonido bajado completamente");
    }
    
    /// <summary>
    /// Baja el volumen de todo el audio gradualmente
    /// </summary>
    public void FadeOutAllAudio(float fadeDuration = 1f)
    {
        Debug.Log($"GameAudioManager: Bajando volumen de todo el audio en {fadeDuration}s");
        FadeOutGameMusic(fadeDuration);
        FadeOutAllSoundEffects(fadeDuration);
    }
    
    /// <summary>
    /// Baja el volumen de la música del juego gradualmente
    /// </summary>
    public void FadeOutGameMusic(float fadeDuration = 1f)
    {
        Debug.Log($"GameAudioManager: Bajando volumen de música del juego en {fadeDuration}s");
        
        if (sectorMusicManager != null)
        {
            sectorMusicManager.FadeOutMusic(fadeDuration);
        }
        else
        {
            Debug.LogWarning("SectorMusicManager no encontrado");
        }
    }
    
    /// <summary>
    /// Restaura el volumen de la música del juego gradualmente
    /// </summary>
    public void FadeInGameMusic(float fadeDuration = 1f)
    {
        Debug.Log($"GameAudioManager: Restaurando volumen de música del juego en {fadeDuration}s");
        
        if (sectorMusicManager != null)
        {
            sectorMusicManager.FadeInMusic(fadeDuration);
        }
        else
        {
            Debug.LogWarning("SectorMusicManager no encontrado");
        }
    }
    
    /// <summary>
    /// Detiene todos los efectos de sonido que se están reproduciendo
    /// </summary>
    public void StopAllSoundEffects()
    {
        Debug.Log("GameAudioManager: Deteniendo todos los efectos de sonido");
        
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
    
    /// <summary>
    /// Detiene tanto la música como todos los efectos de sonido
    /// </summary>
    public void StopAllAudio()
    {
        Debug.Log("GameAudioManager: Deteniendo todo el audio del juego");
        StopGameMusic();
        StopAllSoundEffects();
    }
    
    /// <summary>
    /// Fuerza el inicio de música del sector 0 (útil después de reiniciar)
    /// </summary>
    public void ForceStartMusic()
    {
        Debug.Log("GameAudioManager: Forzando inicio de música del sector 0");
        if (sectorMusicManager != null)
        {
            sectorMusicManager.ForceStartMusic();
        }
        else
        {
            Debug.LogWarning("GameAudioManager: SectorMusicManager no encontrado para forzar inicio de música");
        }
    }
    
    /// <summary>
    /// Pausa la música del juego
    /// </summary>
    public void PauseGameMusic()
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.PauseMusic();
        }
    }
    
    /// <summary>
    /// Reanuda la música del juego
    /// </summary>
    public void ResumeGameMusic()
    {
        if (sectorMusicManager != null)
        {
            sectorMusicManager.ResumeMusic();
        }
    }
    
    /// <summary>
    /// Ajusta el volumen de los efectos de sonido
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
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
    /// Reproduce múltiples disparos con variación de pitch
    /// </summary>
    public void PlayShootSoundWithVariation(float pitchVariation = 0.1f)
    {
        if (shootSound != null && sfxSource != null)
        {
            float originalPitch = sfxSource.pitch;
            sfxSource.pitch = originalPitch + Random.Range(-pitchVariation, pitchVariation);
            PlaySoundImmediately(shootSound);
            sfxSource.pitch = originalPitch;
        }
    }
    
    /// <summary>
    /// Reproduce un sonido con fade in
    /// </summary>
    public void PlaySoundWithFadeIn(AudioClip clip, float fadeInDuration = 0.5f)
    {
        if (clip != null && sfxSource != null)
        {
            StartCoroutine(FadeInSound(clip, fadeInDuration));
        }
    }
    
    private IEnumerator FadeInSound(AudioClip clip, float fadeInDuration)
    {
        sfxSource.clip = clip;
        sfxSource.volume = 0f;
        sfxSource.Play();
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeInDuration;
            sfxSource.volume = Mathf.Lerp(0f, sfxVolume, progress);
            yield return null;
        }
        
        sfxSource.volume = sfxVolume;
    }
    
    /// <summary>
    /// Habilita o deshabilita la prevención de superposición
    /// </summary>
    public void SetPreventSoundOverlap(bool prevent)
    {
        preventSoundOverlap = prevent;
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
} 