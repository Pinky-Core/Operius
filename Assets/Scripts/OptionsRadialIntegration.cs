using UnityEngine;
using UnityEngine.UI;

public class OptionsRadialIntegration : MonoBehaviour
{
    [Header("Radial Menu Integration")]
    [SerializeField] private RadialMenu radialMenu;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button openOptionsButton;
    [SerializeField] private Button closeOptionsButton;
    [SerializeField] private Button resetDefaultsButton;
    
    [Header("Options Manager")]
    [SerializeField] private OptionsManager optionsManager;
    
    private void Start()
    {
        // Buscar el OptionsManager si no está asignado
        if (optionsManager == null)
        {
            optionsManager = FindObjectOfType<OptionsManager>();
        }
        
        // Buscar el RadialMenu si no está asignado
        if (radialMenu == null)
        {
            radialMenu = FindObjectOfType<RadialMenu>();
        }
        
        SetupButtons();
        
        // Ocultar el panel de opciones al inicio
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// Configura los botones de opciones
    /// </summary>
    private void SetupButtons()
    {
        if (openOptionsButton != null)
        {
            openOptionsButton.onClick.AddListener(OpenOptionsPanel);
        }
        
        if (closeOptionsButton != null)
        {
            closeOptionsButton.onClick.AddListener(CloseOptionsPanel);
        }
        
        if (resetDefaultsButton != null)
        {
            resetDefaultsButton.onClick.AddListener(ResetToDefaults);
        }
    }
    
    /// <summary>
    /// Abre el panel de opciones
    /// </summary>
    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            
            // Actualizar la UI del OptionsManager
            if (optionsManager != null)
            {
                optionsManager.OpenOptions();
            }
            
            Debug.Log("Panel de opciones abierto");
        }
    }
    
    /// <summary>
    /// Cierra el panel de opciones
    /// </summary>
    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            
            // Cerrar el OptionsManager
            if (optionsManager != null)
            {
                optionsManager.CloseOptions();
            }
            
            Debug.Log("Panel de opciones cerrado");
        }
    }
    
    /// <summary>
    /// Resetea las opciones a los valores por defecto
    /// </summary>
    public void ResetToDefaults()
    {
        if (optionsManager != null)
        {
            optionsManager.ResetToDefaults();
            Debug.Log("Opciones reseteadas a valores por defecto");
        }
    }
    
    /// <summary>
    /// Método para ser llamado desde el RadialMenu
    /// </summary>
    public void OnOptionsSelected()
    {
        OpenOptionsPanel();
    }
    
    /// <summary>
    /// Método para cerrar opciones desde el RadialMenu
    /// </summary>
    public void OnOptionsClosed()
    {
        CloseOptionsPanel();
    }
    
    // Context Menu methods for testing
    [ContextMenu("Open Options")]
    private void TestOpenOptions()
    {
        OpenOptionsPanel();
    }
    
    [ContextMenu("Close Options")]
    private void TestCloseOptions()
    {
        CloseOptionsPanel();
    }
    
    [ContextMenu("Reset Options")]
    private void TestResetOptions()
    {
        ResetToDefaults();
    }
} 