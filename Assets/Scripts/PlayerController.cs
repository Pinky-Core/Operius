using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform center; // el centro del tubo
    public float radius = 4f; // radio del túnel
    public float rotationSpeed = 100f;
    public bool useGyro = true;

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
            input = -Input.gyro.rotationRateUnbiased.z;
        }
        else
        {
            input = Input.GetAxis("Horizontal");
        }

        angle += input * rotationSpeed * Time.deltaTime;

        // Orbitar alrededor del centro en círculo
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0f) * radius;

        transform.position = center.position + offset;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - center.position).normalized);
    }
}
