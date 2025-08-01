using UnityEngine;

public class DeathMenuAudio : MonoBehaviour
{
    [Header("Sonidos del Death Panel")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip buttonHoverSound;
    
    [Header("Configuración")]
    [SerializeField] private float volume = 0.5f;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        // Crear AudioSource si no existe
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.playOnAwake = false;
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de clic de botón
    /// </summary>
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, volume);
        }
        else
        {
            // Si no hay sonido personalizado, usar GameAudioManager
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayCustomSound(buttonClickSound);
            }
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de hover de botón
    /// </summary>
    public void PlayButtonHoverSound()
    {
        if (buttonHoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonHoverSound, volume);
        }
    }
    
    /// <summary>
    /// Reproduce un sonido personalizado
    /// </summary>
    public void PlayCustomSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
    
    /// <summary>
    /// Ajusta el volumen
    /// </summary>
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
    
    /// <summary>
    /// Obtiene el volumen actual
    /// </summary>
    public float GetVolume()
    {
        return volume;
    }
} 