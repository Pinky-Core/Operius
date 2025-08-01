using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuAudioManager : MonoBehaviour
{
    [Header("Configuración de Música de Fondo")]
    [SerializeField] private AudioClip[] backgroundMusicClips;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private bool playMusicOnStart = true;
    [SerializeField] private bool loopMusic = false; // Si es false, cambiará automáticamente
    
    [Header("Efectos de Sonido")]
    [SerializeField] private AudioClip menuRotateSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private float sfxVolume = 0.8f;
    
    [Header("Configuración de Transiciones")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float crossfadeDuration = 2f;
    
    private int currentMusicIndex = 0;
    private bool isTransitioning = false;
    private Coroutine musicTransitionCoroutine;
    
    // Singleton pattern
    public static MainMenuAudioManager Instance { get; private set; }
    
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
        
        // Configurar AudioSources si no están asignados
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configurar música
        musicSource.loop = loopMusic;
        musicSource.volume = musicVolume;
        
        // Configurar efectos de sonido
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }
    
    private void Start()
    {
        // Cargar configuración de volumen desde OptionsManager
        LoadVolumeSettings();
        
        if (playMusicOnStart && backgroundMusicClips.Length > 0)
        {
            PlayBackgroundMusic();
        }
    }
    
    /// <summary>
    /// Carga la configuración de volumen desde OptionsManager
    /// </summary>
    private void LoadVolumeSettings()
    {
        if (OptionsManager.Instance != null)
        {
            musicVolume = OptionsManager.Instance.GetMusicVolume();
            sfxVolume = OptionsManager.Instance.GetSFXVolume();
            
            // Aplicar volúmenes
            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
            }
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }
    }
    
    private void Update()
    {
        // Verificar si la música actual terminó y cambiar automáticamente
        if (!loopMusic && !isTransitioning && backgroundMusicClips.Length > 1)
        {
            if (!musicSource.isPlaying && musicSource.clip != null)
            {
                PlayNextMusic();
            }
        }
    }
    
    /// <summary>
    /// Reproduce la música de fondo
    /// </summary>
    public void PlayBackgroundMusic()
    {
        if (backgroundMusicClips.Length == 0) return;
        
        if (currentMusicIndex >= backgroundMusicClips.Length)
        {
            currentMusicIndex = 0;
        }
        
        musicSource.clip = backgroundMusicClips[currentMusicIndex];
        musicSource.Play();
        
        Debug.Log($"Reproduciendo música: {backgroundMusicClips[currentMusicIndex].name}");
    }
    
    /// <summary>
    /// Reproduce la siguiente canción en la lista
    /// </summary>
    public void PlayNextMusic()
    {
        if (backgroundMusicClips.Length <= 1) return;
        
        currentMusicIndex = (currentMusicIndex + 1) % backgroundMusicClips.Length;
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        musicTransitionCoroutine = StartCoroutine(CrossfadeToNextMusic());
    }
    
    /// <summary>
    /// Reproduce la canción anterior en la lista
    /// </summary>
    public void PlayPreviousMusic()
    {
        if (backgroundMusicClips.Length <= 1) return;
        
        currentMusicIndex--;
        if (currentMusicIndex < 0)
        {
            currentMusicIndex = backgroundMusicClips.Length - 1;
        }
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        musicTransitionCoroutine = StartCoroutine(CrossfadeToNextMusic());
    }
    
    /// <summary>
    /// Reproduce una canción específica por índice
    /// </summary>
    public void PlayMusicAtIndex(int index)
    {
        if (index < 0 || index >= backgroundMusicClips.Length) return;
        
        currentMusicIndex = index;
        
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }
        
        musicTransitionCoroutine = StartCoroutine(CrossfadeToNextMusic());
    }
    
    /// <summary>
    /// Transición suave entre canciones
    /// </summary>
    private IEnumerator CrossfadeToNextMusic()
    {
        isTransitioning = true;
        
        float startVolume = musicSource.volume;
        float elapsedTime = 0f;
        
        // Fade out
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeOutDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }
        
        // Cambiar canción
        musicSource.clip = backgroundMusicClips[currentMusicIndex];
        musicSource.Play();
        
        Debug.Log($"Cambiando a música: {backgroundMusicClips[currentMusicIndex].name}");
        
        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeInDuration;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, progress);
            yield return null;
        }
        
        musicSource.volume = musicVolume;
        isTransitioning = false;
    }
    
    /// <summary>
    /// Reproduce el sonido de rotación del menú
    /// </summary>
    public void PlayMenuRotateSound()
    {
        if (menuRotateSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(menuRotateSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de clic de botón
    /// </summary>
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de hover de botón
    /// </summary>
    public void PlayButtonHoverSound()
    {
        if (buttonHoverSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonHoverSound);
        }
    }
    
    /// <summary>
    /// Reproduce un efecto de sonido personalizado
    /// </summary>
    public void PlayCustomSound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// Pausa la música
    /// </summary>
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    /// <summary>
    /// Reanuda la música
    /// </summary>
    public void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    
    /// <summary>
    /// Detiene la música
    /// </summary>
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
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
    /// Obtiene el nombre de la canción actual
    /// </summary>
    public string GetCurrentMusicName()
    {
        if (backgroundMusicClips.Length > 0 && currentMusicIndex < backgroundMusicClips.Length)
        {
            return backgroundMusicClips[currentMusicIndex].name;
        }
        return "Sin música";
    }
    
    /// <summary>
    /// Obtiene el índice de la canción actual
    /// </summary>
    public int GetCurrentMusicIndex()
    {
        return currentMusicIndex;
    }
    
    /// <summary>
    /// Obtiene el total de canciones disponibles
    /// </summary>
    public int GetTotalMusicCount()
    {
        return backgroundMusicClips.Length;
    }
} 