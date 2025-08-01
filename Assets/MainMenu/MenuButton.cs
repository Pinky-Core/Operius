using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public enum MenuAction
    {
        Play,
        Options,
        Shop,
        Quit,
        Credits,
        Settings,
        MainMenu,
        Restart
    }

    public MenuAction action;
    public Animator screenAnimator;
    public float delayBeforeAction = 1f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;

    // Método principal para usar en eventos del menú radial
    public void OnClick()
    {
        // Reproducir sonido de clic de botón
        PlayButtonClickSound();
        
        // Si hay animación, ejecutarla primero
        if (screenAnimator != null)
        {
            screenAnimator.SetTrigger("Close");
            Invoke(nameof(ExecuteAction), delayBeforeAction);
        }
        else
        {
            // Si no hay animación, ejecutar inmediatamente
            ExecuteAction();
        }
    }
    
    /// <summary>
    /// Reproduce el sonido de clic de botón según la escena actual
    /// </summary>
    private void PlayButtonClickSound()
    {
        // Verificar si estamos en la escena del menú principal
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToLower();
        
        if (currentScene.Contains("menu") || currentScene.Contains("main"))
        {
            // Estamos en el menú principal, usar MainMenuAudioManager
            if (MainMenuAudioManager.Instance != null)
            {
                MainMenuAudioManager.Instance.PlayButtonClickSound();
            }
        }
        else
        {
            // Estamos en el juego, verificar si hay DeathMenuAudio
            DeathMenuAudio deathMenuAudio = FindObjectOfType<DeathMenuAudio>();
            if (deathMenuAudio != null)
            {
                // Usar DeathMenuAudio si está disponible
                deathMenuAudio.PlayButtonClickSound();
            }
            else if (GameAudioManager.Instance != null)
            {
                // Usar GameAudioManager como fallback
                GameAudioManager.Instance.PlayCustomSound(GetButtonClickSound());
            }
        }
    }
    
    /// <summary>
    /// Obtiene el sonido de clic de botón
    /// </summary>
    private AudioClip GetButtonClickSound()
    {
        // Si hay un sonido personalizado asignado, usarlo
        if (buttonClickSound != null)
        {
            return buttonClickSound;
        }
        
        // Si no hay sonido personalizado, usar el sonido generado del GameAudioManager
        return null;
    }

    // Método alternativo sin parámetros para eventos
    public void OnButtonClick()
    {
        OnClick();
    }

    // Método para jugar
    public void PlayGame()
    {
        action = MenuAction.Play;
        OnClick();
    }

    // Método para opciones
    public void OpenOptions()
    {
        action = MenuAction.Options;
        OnClick();
    }

    // Método para tienda
    public void OpenShop()
    {
        action = MenuAction.Shop;
        OnClick();
    }

    // Método para salir
    public void QuitGame()
    {
        action = MenuAction.Quit;
        OnClick();
    }

    // Método para créditos
    public void OpenCredits()
    {
        action = MenuAction.Credits;
        OnClick();
    }

    // Método para configuración
    public void OpenSettings()
    {
        action = MenuAction.Settings;
        OnClick();
    }

    // Método para menú principal
    public void GoToMainMenu()
    {
        action = MenuAction.MainMenu;
        OnClick();
    }

    // Método para reiniciar
    public void RestartLevel()
    {
        action = MenuAction.Restart;
        OnClick();
    }
    


    // Método para ejecutar acción inmediatamente sin delay
    public void ExecuteImmediately()
    {
        ExecuteAction();
    }

    // Método para ejecutar con delay personalizado
    public void ExecuteWithDelay(float customDelay)
    {
        if (screenAnimator != null)
    {
        screenAnimator.SetTrigger("Close");
            Invoke(nameof(ExecuteAction), customDelay);
        }
        else
        {
            Invoke(nameof(ExecuteAction), customDelay);
        }
    }

    void ExecuteAction()
    {
        Debug.Log("Ejecutando acción: " + action);
        
        // Detener música según la acción
        switch (action)
        {
            case MenuAction.Play:
                // Detener música del menú antes de ir al juego
                if (MainMenuAudioManager.Instance != null)
                {
                    Debug.Log("Deteniendo música del menú");
                    MainMenuAudioManager.Instance.StopMusic();
                }
                break;
                
            case MenuAction.MainMenu:
            case MenuAction.Restart:
                // Detener todo el audio del juego inmediatamente antes de volver al menú o reiniciar
                if (GameAudioManager.Instance != null)
                {
                    Debug.Log("Deteniendo todo el audio del juego");
                    GameAudioManager.Instance.StopAllAudio();
                }
                break;
        }
        
        // Ejecutar la acción
        switch (action)
        {
            case MenuAction.Play:
                SceneManager.LoadScene("SampleScene");
                break;
            case MenuAction.Options:
                SceneManager.LoadScene("Opciones");
                break;
            case MenuAction.Shop:
                SceneManager.LoadScene("Tienda");
                break;
            case MenuAction.Quit:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
            case MenuAction.Credits:
                SceneManager.LoadScene("Creditos");
                break;
            case MenuAction.Settings:
                SceneManager.LoadScene("Configuracion");
                break;
            case MenuAction.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case MenuAction.Restart:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }
    

}
