using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform center;
    public float rotationSmoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null || center == null) return;

        Vector3 toPlayer = (player.position - center.position).normalized;

        // Mirar hacia atrás (-Z) y mantener al jugador abajo
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.back, -toPlayer);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }

}
