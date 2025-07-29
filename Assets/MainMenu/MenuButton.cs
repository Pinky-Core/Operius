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

    // Método principal para usar en eventos del menú radial
    public void OnClick()
    {
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
