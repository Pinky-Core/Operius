using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform center;                // Centro del tubo
    public float radius = 4f;               // Radio del t�nel
    public float maxRotationSpeed = 200f;   // M�xima velocidad de giro (grados/segundo)
    public float gyroSensitivity = 1.5f;    // Sensibilidad al giro
    public float gyroThreshold = 0.05f;     // Umbral m�nimo para ignorar ruido
    public bool useGyro = true;
    public bool invertGyro = false;         // Puedes invertir manualmente si lo deseas

    private float angle;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
    }

    void Update()
    {
        float input = 0f;

        if (useGyro && SystemInfo.supportsGyroscope)
        {
            // CORRECCI�N: Invertido manualmente para que el giro sea natural como volante
            float gyroInput = -Input.gyro.rotationRateUnbiased.z;

            if (invertGyro) gyroInput *= -1f;

            float scaledInput = gyroInput * gyroSensitivity;

            if (Mathf.Abs(scaledInput) > gyroThreshold)
            {
                input = Mathf.Clamp(scaledInput, -1f, 1f);
            }
            else
            {
                input = 0f;
            }
        }
        else
        {
            input = Input.GetAxis("Horizontal");
        }

        // Aplica rotaci�n proporcional al input
        angle += input * maxRotationSpeed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        transform.position = center.position + offset;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            GetComponent<PlayerShooting>().CollectPowerup();
            Destroy(other.gameObject);
        }
    }

}
