using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public float timeElapsed = 0f;
    public float difficultyMultiplier = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        difficultyMultiplier = 1f + (timeElapsed / 60f); // Aumenta dificultad cada minuto
    }
}
