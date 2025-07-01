using UnityEngine;
using TMPro;

public class SectorManager : MonoBehaviour
{
    public Color[] sectorColors;          // Colores para cada sector
    public TextMeshProUGUI sectorText;   // Texto para mostrar solo el número de sector

    private int currentSector = 0;

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
        UpdateSkyboxColor(currentSector);
        UpdateSectorText(currentSector);
    }

    void OnSectorLevelUp(int newSector)
    {
        currentSector = newSector;
        UpdateSkyboxColor(currentSector);
        UpdateSectorText(currentSector);
    }

    void UpdateSkyboxColor(int sectorIndex)
    {
        if (sectorIndex >= 0 && sectorIndex < sectorColors.Length)
        {
            RenderSettings.skybox.SetColor("_SkyTint", sectorColors[sectorIndex]);
            DynamicGI.UpdateEnvironment();
        }
    }

    void UpdateSectorText(int sectorIndex)
    {
        if (sectorText != null)
        {
            // Muestra con ceros a la izquierda, 3 dígitos
            sectorText.text = (sectorIndex + 1).ToString("D3");
        }
    }
}
