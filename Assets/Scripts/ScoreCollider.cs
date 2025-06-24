using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    public ScoreManager scoreManager;

    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (scoreManager != null)
        {
            scoreManager.AddPointsFrom(other.gameObject);
        }
    }
}
