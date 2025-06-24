using UnityEngine;
using UnityEngine.UI;

public class PixelatedScreenOutput : MonoBehaviour
{
    public RawImage screenImage;
    public PixelatedRender pixelatedRenderScript;

    void Start()
    {
        if (screenImage != null && pixelatedRenderScript != null)
        {
            screenImage.texture = pixelatedRenderScript.pixelRenderTexture;
        }
    }
}
