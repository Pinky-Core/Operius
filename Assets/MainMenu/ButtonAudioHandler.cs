using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Configuración de Audio")]
    [SerializeField] private bool playHoverSound = true;
    [SerializeField] private bool playClickSound = true;
    [SerializeField] private AudioClip customHoverSound;
    [SerializeField] private AudioClip customClickSound;
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    
    /// <summary>
    /// Se ejecuta cuando el mouse entra en el botón
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playHoverSound && button != null && button.interactable)
        {
            if (MainMenuAudioManager.Instance != null)
            {
                if (customHoverSound != null)
                {
                    MainMenuAudioManager.Instance.PlayCustomSound(customHoverSound);
                }
                else
                {
                    MainMenuAudioManager.Instance.PlayButtonHoverSound();
                }
            }
        }
    }
    
    /// <summary>
    /// Se ejecuta cuando se hace clic en el botón
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (playClickSound && button != null && button.interactable)
        {
            if (MainMenuAudioManager.Instance != null)
            {
                if (customClickSound != null)
                {
                    MainMenuAudioManager.Instance.PlayCustomSound(customClickSound);
                }
                else
                {
                    MainMenuAudioManager.Instance.PlayButtonClickSound();
                }
            }
        }
    }
    
    /// <summary>
    /// Método público para reproducir sonido de hover manualmente
    /// </summary>
    public void PlayHoverSound()
    {
        if (MainMenuAudioManager.Instance != null)
        {
            if (customHoverSound != null)
            {
                MainMenuAudioManager.Instance.PlayCustomSound(customHoverSound);
            }
            else
            {
                MainMenuAudioManager.Instance.PlayButtonHoverSound();
            }
        }
    }
    
    /// <summary>
    /// Método público para reproducir sonido de clic manualmente
    /// </summary>
    public void PlayClickSound()
    {
        if (MainMenuAudioManager.Instance != null)
        {
            if (customClickSound != null)
            {
                MainMenuAudioManager.Instance.PlayCustomSound(customClickSound);
            }
            else
            {
                MainMenuAudioManager.Instance.PlayButtonClickSound();
            }
        }
    }
    
    /// <summary>
    /// Habilita o deshabilita el sonido de hover
    /// </summary>
    public void SetHoverSoundEnabled(bool enabled)
    {
        playHoverSound = enabled;
    }
    
    /// <summary>
    /// Habilita o deshabilita el sonido de clic
    /// </summary>
    public void SetClickSoundEnabled(bool enabled)
    {
        playClickSound = enabled;
    }
} 