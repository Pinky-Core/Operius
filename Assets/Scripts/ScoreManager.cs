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
}
