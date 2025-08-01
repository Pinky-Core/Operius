using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathMenu : MonoBehaviour
{
    [Header("Textos para mostrar puntuaciones")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highscoreText;

    [Header("Configuración de animación")]
    public float countDuration = 1.5f; // Duración de la animación en segundos
    public bool animateScore = true;

    // Start is called before the first frame update
    void Start()
    {
        if (animateScore)
        {
            StartCoroutine(AnimateScores());
        }
        else
        {
            ShowScores();
        }
    }

    IEnumerator AnimateScores()
    {
        // Obtener valores finales
        int finalCurrentScore = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
        int finalHighscore = PlayerPrefs.GetInt("HighScore", 0);

        // Animar score actual
        int currentValue = 0;
        float elapsedTime = 0f;

        while (elapsedTime < countDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / countDuration;
            
            // Usar una curva suave para la animación
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            currentValue = Mathf.RoundToInt(smoothProgress * finalCurrentScore);
            
            if (currentScoreText != null)
            {
                currentScoreText.text = "Score: " + currentValue.ToString();
            }
            yield return null;
        }

        // Asegurar que muestre el valor final exacto
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + finalCurrentScore.ToString();
        }

        // Mostrar highscore (sin animación para que sea más rápido)
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + finalHighscore.ToString();
        }
    }

    void ShowScores()
    {
        // Mostrar score actual
        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.score : 0;
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + currentScore.ToString();
            Debug.Log("Score actual: " + currentScore);
        }

        // Mostrar highscore
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore.ToString();
            Debug.Log("Highscore: " + highscore);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Llama este método para volver al menú principal
    public void LoadMainMenu()
    {
        // Detener todo el audio del juego inmediatamente antes de volver al menú
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.StopAllAudio();
        }
        
        // Cargar escena inmediatamente
        SceneManager.LoadScene("MainMenu");
    }

    // Llama este método para reiniciar el nivel actual
    public void RestartLevel()
    {
        // Detener todo el audio del juego inmediatamente antes de reiniciar
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.StopAllAudio();
        }
        
        // Reiniciar escena inmediatamente
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    

    

}
