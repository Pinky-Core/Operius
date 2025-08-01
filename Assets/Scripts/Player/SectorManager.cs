using UnityEngine;
using TMPro;
using System.Collections;

public class SectorManager : MonoBehaviour
{
    public Color[] sectorColors;               // Colores para cada sector
    public TextMeshProUGUI sectorText;         // Texto para mostrar solo el n�mero de sector
    public float transitionDuration = 2f;      // Tiempo de transici�n

    private int currentSector = 0;
    private Coroutine colorTransitionCoroutine;

    void OnEnable()
    {
        PlayerShooting.SectorLevelUpEvent += OnSectorLevelUp;
    }

    void OnDisable()
    {
        PlayerShooting.SectorLevelUpEvent -= OnSectorLevelUp;
    }

    void Start()
    {
        // Iniciar con sector 0 (nivel inicial)
        currentSector = 0;
        UpdateSkyboxTint(currentSector);
        UpdateSectorText(currentSector);
    }

    void OnSectorLevelUp(int newSector)
    {
        currentSector = newSector;
        UpdateSectorText(currentSector);

        if (colorTransitionCoroutine != null)
            StopCoroutine(colorTransitionCoroutine);

        colorTransitionCoroutine = StartCoroutine(TransitionSkyboxTint(sectorColors[currentSector]));
        
        // Cambiar música al nuevo sector
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.ChangeToSector(currentSector);
        }
    }

    void UpdateSkyboxTint(int sectorIndex)
    {
        if (sectorIndex >= 0 && sectorIndex < sectorColors.Length)
        {
            RenderSettings.skybox.SetColor("_Tint", sectorColors[sectorIndex]);
            DynamicGI.UpdateEnvironment();
        }
    }

    void UpdateSectorText(int sectorIndex)
    {
        if (sectorText != null)
        {
            sectorText.text = (sectorIndex + 1).ToString("D3");
        }
    }

    IEnumerator TransitionSkyboxTint(Color targetColor)
    {
        Color startColor = RenderSettings.skybox.GetColor("_Tint");
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            Color lerpedColor = Color.Lerp(startColor, targetColor, elapsedTime / transitionDuration);
            RenderSettings.skybox.SetColor("_Tint", lerpedColor);
            DynamicGI.UpdateEnvironment();
            yield return null;
        }

        // Asegura el color final exacto
        RenderSettings.skybox.SetColor("_Tint", targetColor);
        DynamicGI.UpdateEnvironment();
    }
}
