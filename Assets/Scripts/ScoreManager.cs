using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
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

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddPointsFrom(GameObject obj)
    {
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
