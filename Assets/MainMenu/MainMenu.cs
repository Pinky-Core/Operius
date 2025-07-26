using UnityEngine;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;
    public float countDuration = 2f; // Duración de la animación en segundos
    public bool animateOnStart = true;

    void Start()
    {
        if (animateOnStart)
        {
            StartCoroutine(AnimateHighscore());
        }
        else
        {
            ShowHighscore();
        }
    }

    IEnumerator AnimateHighscore()
    {
        int finalHighscore = PlayerPrefs.GetInt("HighScore", 0);
        int currentValue = 0;
        float elapsedTime = 0f;

        while (elapsedTime < countDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / countDuration;
            
            // Usar una curva suave para la animación
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            currentValue = Mathf.RoundToInt(smoothProgress * finalHighscore);
            
            highscoreText.text = "Highscore: " + currentValue.ToString();
            yield return null;
        }

        // Asegurar que muestre el valor final exacto
        highscoreText.text = "Highscore: " + finalHighscore.ToString();
    }

    void ShowHighscore()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highscoreText.text = "Highscore: " + highscore.ToString();
    }
} 