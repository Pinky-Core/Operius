using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public ScoreManager scoreManager;

    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();

            if (scoreManager == null)
            {
                Debug.LogWarning("No se encontró un ScoreManager en la escena.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo sumar puntos si choca con el Player
        if (other.CompareTag("Player"))
        {
            // Reproducir sonido de recolección de powerup
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayPowerupCollectSound();
            }
            
            if (scoreManager != null)
            {
                // Pasamos el PowerUp para que ScoreManager sume según su tag
                scoreManager.AddPointsFrom(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
