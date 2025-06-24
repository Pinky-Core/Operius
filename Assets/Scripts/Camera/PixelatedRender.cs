using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelatedRender : MonoBehaviour
{
    [Header("Pixelated Render Settings")]
    public int pixelWidth = 320;
    public int pixelHeight = 180;
    public FilterMode filterMode = FilterMode.Point;

    [HideInInspector]
    public RenderTexture pixelRenderTexture;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        SetupRenderTexture();
    }

    void OnValidate()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        SetupRenderTexture();
    }

    void SetupRenderTexture()
    {
        if (pixelRenderTexture != null)
            pixelRenderTexture.Release();

        pixelRenderTexture = new RenderTexture(pixelWidth, pixelHeight, 24);
        pixelRenderTexture.filterMode = filterMode;
        pixelRenderTexture.useMipMap = false;
        pixelRenderTexture.autoGenerateMips = false;
        pixelRenderTexture.Create();

        cam.targetTexture = pixelRenderTexture;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Escala el resultado al tamaño de pantalla actual con filtro Point (pixelado)
        Graphics.Blit(pixelRenderTexture, destination);
    }

    void OnDisable()
    {
        if (pixelRenderTexture != null)
            pixelRenderTexture.Release();

        if (cam != null)
            cam.targetTexture = null;
    }
}
