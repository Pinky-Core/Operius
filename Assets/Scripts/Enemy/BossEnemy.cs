using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour, IEnemy
{
    public float maxHealth = 1000f;
    private float currentHealth;

    public Slider healthBar; // Asignar un slider en el prefab o instanciar desde canvas

    private Transform center;
    private float angularSpeed;
    private float forwardSpeed;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }

        UpdateHealthBar();
    }


    public void Initialize(Transform center, float radius, float angularSpeed, float forwardSpeed)
    {
        this.center = center;
        this.angularSpeed = angularSpeed;
        this.forwardSpeed = forwardSpeed;
    }

    void Update()
    {
        if (center == null) return;

        // Avance extremadamente lento
        transform.position += Vector3.forward * 0.3f * Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        Destroy(gameObject);
    }

}
