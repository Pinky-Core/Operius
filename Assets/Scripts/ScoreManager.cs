using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [System.Serializable]
    public class ScoringTag
    {
        public string tag;
        public int points;
    }

    [Header("Tags que otorgan puntos")]
    public List<ScoringTag> scoringTags;

    [Header("Texto UI (TextMeshPro)")]
    public TextMeshProUGUI scoreText;

    [Header("Puntaje actual")]
    public int score = 0;

    [Header("Configuración de Conversión a Monedas")]
    [SerializeField] private int pointsPerCoin = 100; // Configurable: 100 puntos = 1 moneda
    [SerializeField] private bool autoConvertToCoins = true; // Convertir automáticamente a monedas

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI();
    }

    // Suma puntos SOLO según el tag del objeto recibido
    public void AddPointsFrom(GameObject obj)
    {
        // Por seguridad, ignorar balas o tags no deseados
        if (obj.CompareTag("bullet"))
            return;

        foreach (var item in scoringTags)
        {
            if (obj.CompareTag(item.tag))
            {
                score += item.points;
                UpdateScoreUI();
                
                // Convertir puntos a monedas automáticamente si está habilitado
                if (autoConvertToCoins)
                {
                    ConvertPointsToCoins(item.points);
                }
                break;
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    
    /// <summary>
    /// Convierte puntos ganados a monedas de la tienda
    /// </summary>
    private void ConvertPointsToCoins(int pointsGained)
    {
        // Convertir puntos a monedas directamente usando PlayerPrefs
        int currentPoints = PlayerPrefs.GetInt("PlayerPoints", 0);
        int newPoints = currentPoints + pointsGained;
        
        // Convertir a monedas (100 puntos = 1 moneda)
        int newCoins = newPoints / pointsPerCoin;
        int remainingPoints = newPoints % pointsPerCoin;
        
        // Guardar monedas y puntos restantes
        int currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        PlayerPrefs.SetInt("PlayerCoins", currentCoins + newCoins);
        PlayerPrefs.SetInt("PlayerPoints", remainingPoints);
        PlayerPrefs.Save();
        
        Debug.Log($"Convertidos {pointsGained} puntos a monedas de la tienda. Monedas totales: {currentCoins + newCoins}, Puntos restantes: {remainingPoints}");
    }

    /// <summary>
    /// Obtiene la configuración de puntos por moneda
    /// </summary>
    public int GetPointsPerCoin()
    {
        return pointsPerCoin;
    }

    /// <summary>
    /// Configura la conversión de puntos a monedas
    /// </summary>
    public void SetPointsPerCoin(int newPointsPerCoin)
    {
        pointsPerCoin = newPointsPerCoin;
        Debug.Log($"Configuración actualizada: {pointsPerCoin} puntos = 1 moneda");
    }

    /// <summary>
    /// Habilita o deshabilita la conversión automática
    /// </summary>
    public void SetAutoConvertToCoins(bool enabled)
    {
        autoConvertToCoins = enabled;
        Debug.Log($"Conversión automática a monedas: {(enabled ? "HABILITADA" : "DESHABILITADA")}");
    }

    /// <summary>
    /// Obtiene el puntaje actual
    /// </summary>
    public int GetCurrentScore()
    {
        return score;
    }

    /// <summary>
    /// Resetea el puntaje (útil para nuevas partidas)
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
        Debug.Log("Puntaje reseteado");
    }
}
