using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelatePostProcess : MonoBehaviour
{
    public Material pixelateMaterial;  // Material con shader pixelate
    [Range(1, 64)]
    public int pixelSize = 8;          // Tamaño base del pixel en px
    public bool autoAdjustPixelSize = true;  // Ajustar pixelSize para que divida la resolución
    public bool adjustCameraFOV = true;       // Compensar FOV para evitar zoom visual
    public float baseFOV = 60f;        // FOV estándar de la cámara
    public float fovCompensationFactor = 0.5f; // Factor para compensar zoom, ajustable

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
            baseFOV = cam.fieldOfView;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelateMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        int finalPixelSize = pixelSize;

        if (autoAdjustPixelSize)
        {
            // Ajusta pixelSize para que divida la resolución vertical exactamente
            int screenHeight = Screen.height;
            finalPixelSize = Mathf.Max(1, Mathf.RoundToInt((float)screenHeight / Mathf.Round(screenHeight / (float)pixelSize)));
        }

        pixelateMaterial.SetFloat("_PixelSize", finalPixelSize);

        // Ajuste opcional del FOV para compensar zoom
        if (adjustCameraFOV && cam != null)
        {
            // Reduce el FOV proporcionalmente para “alejar” la cámara y compensar pixelado
            cam.fieldOfView = baseFOV + fovCompensationFactor * (pixelSize - finalPixelSize);
        }

        Graphics.Blit(src, dest, pixelateMaterial);
    }

    void OnDisable()
    {
        if (cam != null)
        {
            cam.fieldOfView = baseFOV; // Restaurar FOV al desactivar
        }
    }
}
