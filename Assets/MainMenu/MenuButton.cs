using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public enum MenuAction
    {
        Play,
        Options,
        Shop
    }

    public MenuAction action;
    public Animator screenAnimator;

    public void OnClick()
    {
        screenAnimator.SetTrigger("Close");
        Invoke(nameof(ExecuteAction), 1f);
    }

    void ExecuteAction()
    {
        switch (action)
        {
            case MenuAction.Play:
                SceneManager.LoadScene("Juego");
                break;
            case MenuAction.Options:
                SceneManager.LoadScene("Opciones");
                break;
            case MenuAction.Shop:
                SceneManager.LoadScene("Tienda");
                break;
        }
    }
}
