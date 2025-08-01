using UnityEngine;
using TMPro;
using System.Collections;

public class EnhancedSectorManager : MonoBehaviour
{
    [Header("Configuración Visual")]
    public Color[] sectorColors;               // Colores para cada sector
    public TextMeshProUGUI sectorText;         // Texto para mostrar solo el número de sector
    public float transitionDuration = 2f;      // Tiempo de transición visual
    
    [Header("Configuración de Audio")]
    [SerializeField] private bool enableSectorMusic = true;
    [SerializeField] private bool enableVisualTransitions = true;
    [SerializeField] private bool enableSectorText = true;
    
    [Header("Configuración de Efectos")]
    [SerializeField] private AudioClip sectorChangeSound;
    [SerializeField] private float sectorChangeVolume = 0.5f;
    [SerializeField] private bool playSectorChangeSound = true;
    
    private int currentSector = 0;
    private Coroutine colorTransitionCoroutine;
    private Coroutine musicTransitionCoroutine;
    
    // Eventos
    public System.Action<int> OnSectorChanged;
    public System.Action<int> OnSectorTransitionStarted;
    public System.Action<int> OnSectorTransitionCompleted;

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
        InitializeSector();
    }
    
    /// <summary>
    /// Inicializa el sector inicial
    /// </summary>
    void InitializeSector()
    {
        currentSector = 0;
        
        if (enableVisualTransitions)
        {
            UpdateSkyboxTint(currentSector);
        }
        
        if (enableSectorText)
        {
            UpdateSectorText(currentSector);
        }
        
        if (enableSectorMusic && GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.ChangeToSector(currentSector);
        }
        
        OnSectorChanged?.Invoke(currentSector);
    }

    /// <summary>
    /// Maneja el cambio de sector cuando el jugador sube de nivel
    /// </summary>
    void OnSectorLevelUp(int newSector)
    {
        if (newSector == currentSector) return;
        
        int previousSector = currentSector;
        currentSector = newSector;
        
        OnSectorTransitionStarted?.Invoke(currentSector);
        
        // Reproducir sonido de cambio de sector
        if (playSectorChangeSound && sectorChangeSound != null)
        {
            if (GameAudioManager.Instance != null)
            {
                GameAudioManager.Instance.PlayCustomSound(sectorChangeSound);
            }
        }
        
        // Iniciar transiciones
        StartSectorTransitions(previousSector, currentSector);
        
        Debug.Log($"Sector cambiado de {previousSector} a {currentSector}");
    }
    
    /// <summary>
    /// Inicia todas las transiciones del sector
    /// </summary>
    void StartSectorTransitions(int previousSector, int newSector)
    {
        // Transición visual
        if (enableVisualTransitions)
        {
            if (colorTransitionCoroutine != null)
                StopCoroutine(colorTransitionCoroutine);
            
            colorTransitionCoroutine = StartCoroutine(TransitionSkyboxTint(sectorColors[newSector]));
        }
        
        // Transición de música
        if (enableSectorMusic)
        {
            if (musicTransitionCoroutine != null)
                StopCoroutine(musicTransitionCoroutine);
            
            musicTransitionCoroutine = StartCoroutine(TransitionMusic(newSector));
        }
        
        // Actualizar texto inmediatamente
        if (enableSectorText)
        {
            UpdateSectorText(newSector);
        }
        
        OnSectorChanged?.Invoke(newSector);
    }

    /// <summary>
    /// Actualiza el color del skybox para el sector especificado
    /// </summary>
    void UpdateSkyboxTint(int sectorIndex)
    {
        if (sectorIndex >= 0 && sectorIndex < sectorColors.Length)
        {
            RenderSettings.skybox.SetColor("_Tint", sectorColors[sectorIndex]);
            DynamicGI.UpdateEnvironment();
        }
    }

    /// <summary>
    /// Actualiza el texto del sector
    /// </summary>
    void UpdateSectorText(int sectorIndex)
    {
        if (sectorText != null)
        {
            sectorText.text = (sectorIndex + 1).ToString("D3");
        }
    }

    /// <summary>
    /// Transición suave del color del skybox
    /// </summary>
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
    
    /// <summary>
    /// Transición de música con delay para sincronizar con la transición visual
    /// </summary>
    IEnumerator TransitionMusic(int newSector)
    {
        // Pequeño delay para que la transición visual y de música no sean simultáneas
        yield return new WaitForSeconds(0.2f);
        
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.ChangeToSector(newSector);
        }
        
        OnSectorTransitionCompleted?.Invoke(newSector);
    }
    
    /// <summary>
    /// Cambia manualmente al sector especificado
    /// </summary>
    public void ChangeToSector(int sectorIndex)
    {
        if (sectorIndex < 0 || sectorIndex >= sectorColors.Length) return;
        
        OnSectorLevelUp(sectorIndex);
    }
    
    /// <summary>
    /// Obtiene el sector actual
    /// </summary>
    public int GetCurrentSector()
    {
        return currentSector;
    }
    
    /// <summary>
    /// Obtiene el color del sector actual
    /// </summary>
    public Color GetCurrentSectorColor()
    {
        if (currentSector >= 0 && currentSector < sectorColors.Length)
        {
            return sectorColors[currentSector];
        }
        return Color.white;
    }
    
    /// <summary>
    /// Obtiene el nombre del sector actual
    /// </summary>
    public string GetCurrentSectorName()
    {
        return $"Sector {(currentSector + 1).ToString("D3")}";
    }
    
    /// <summary>
    /// Habilita o deshabilita las transiciones visuales
    /// </summary>
    public void SetVisualTransitions(bool enabled)
    {
        enableVisualTransitions = enabled;
    }
    
    /// <summary>
    /// Habilita o deshabilita la música por sectores
    /// </summary>
    public void SetSectorMusic(bool enabled)
    {
        enableSectorMusic = enabled;
    }
    
    /// <summary>
    /// Habilita o deshabilita el texto del sector
    /// </summary>
    public void SetSectorText(bool enabled)
    {
        enableSectorText = enabled;
        if (!enabled && sectorText != null)
        {
            sectorText.text = "";
        }
    }
    
    /// <summary>
    /// Habilita o deshabilita el sonido de cambio de sector
    /// </summary>
    public void SetSectorChangeSound(bool enabled)
    {
        playSectorChangeSound = enabled;
    }
    
    /// <summary>
    /// Prueba el cambio de sector (para debugging)
    /// </summary>
    [ContextMenu("Test Sector Change")]
    public void TestSectorChange()
    {
        int testSector = (currentSector + 1) % sectorColors.Length;
        ChangeToSector(testSector);
    }
    
    /// <summary>
    /// Prueba todos los sectores en secuencia
    /// </summary>
    [ContextMenu("Test All Sectors")]
    public void TestAllSectors()
    {
        StartCoroutine(TestAllSectorsCoroutine());
    }
    
    IEnumerator TestAllSectorsCoroutine()
    {
        for (int i = 0; i < sectorColors.Length; i++)
        {
            ChangeToSector(i);
            yield return new WaitForSeconds(3f);
        }
    }
} 