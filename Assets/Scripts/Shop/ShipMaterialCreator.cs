using UnityEngine;

/// <summary>
/// Script de utilidad para crear materiales básicos para las naves
/// Se puede usar en el editor para generar materiales automáticamente
/// </summary>
public class ShipMaterialCreator : MonoBehaviour
{
    [Header("Configuración de Materiales")]
    [SerializeField] private string materialNamePrefix = "ShipMaterial_";
    [SerializeField] private Color[] shipColors = new Color[]
    {
        Color.white,      // Básico
        Color.blue,       // Veloz
        Color.green,      // Defensiva
        Color.red,        // Elite
        Color.yellow,     // Premium
        Color.cyan,       // Especial
        Color.magenta,    // Último
        new Color(1f, 0.5f, 0f, 1f)  // Naranja (Bonus)
    };
    
    [Header("Propiedades del Material")]
    [SerializeField] private float metallic = 0.8f;
    [SerializeField] private float smoothness = 0.6f;
    [SerializeField] private bool enableEmission = true;
    [SerializeField] private float emissionIntensity = 0.2f;
    
    [Header("Generación")]
    [SerializeField] private bool generateOnStart = false;
    [SerializeField] private string savePath = "Assets/Materials/ShipMaterials/";
    
    void Start()
    {
        if (generateOnStart)
        {
            CreateShipMaterials();
        }
    }
    
    /// <summary>
    /// Crea materiales básicos para las naves
    /// </summary>
    [ContextMenu("Crear Materiales de Nave")]
    public void CreateShipMaterials()
    {
        Debug.Log("Creando materiales de nave...");
        
        // Crear directorio si no existe
        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }
        
        Material[] createdMaterials = new Material[shipColors.Length];
        
        for (int i = 0; i < shipColors.Length; i++)
        {
            Material material = CreateShipMaterial(i, shipColors[i]);
            createdMaterials[i] = material;
            
            // Guardar material como asset
            string materialPath = savePath + materialNamePrefix + i + ".mat";
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(material, materialPath);
            #endif
            
            Debug.Log($"Material creado: {materialPath}");
        }
        
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
        #endif
        
        Debug.Log($"Se crearon {shipColors.Length} materiales de nave en {savePath}");
        
        // Mostrar cómo usar los materiales
        ShowUsageInstructions(createdMaterials);
    }
    
    /// <summary>
    /// Crea un material individual para una nave
    /// </summary>
    private Material CreateShipMaterial(int index, Color color)
    {
        Material material = new Material(Shader.Find("Standard"));
        
        // Configurar propiedades básicas
        material.name = materialNamePrefix + index;
        material.color = color;
        material.SetFloat("_Metallic", metallic);
        material.SetFloat("_Glossiness", smoothness);
        
        // Configurar emisión si está habilitada
        if (enableEmission)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * emissionIntensity);
        }
        
        return material;
    }
    
    /// <summary>
    /// Muestra instrucciones de uso
    /// </summary>
    private void ShowUsageInstructions(Material[] materials)
    {
        Debug.Log("=== INSTRUCCIONES DE USO ===");
        Debug.Log("1. En ShopConfiguration, asignar los materiales:");
        
        for (int i = 0; i < materials.Length; i++)
        {
            Debug.Log($"   - Material {i}: {materials[i].name}");
        }
        
        Debug.Log("2. En ShipMaterialApplier (en la nave del jugador), asignar los mismos materiales");
        Debug.Log("3. Los materiales se aplicarán automáticamente según la selección en la tienda");
        Debug.Log("=============================");
    }
    
    /// <summary>
    /// Crea un material personalizado con propiedades específicas
    /// </summary>
    public Material CreateCustomShipMaterial(string name, Color color, float metallic = 0.8f, float smoothness = 0.6f, bool emission = true)
    {
        Material material = new Material(Shader.Find("Standard"));
        
        material.name = name;
        material.color = color;
        material.SetFloat("_Metallic", metallic);
        material.SetFloat("_Glossiness", smoothness);
        
        if (emission)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * emissionIntensity);
        }
        
        return material;
    }
    
    /// <summary>
    /// Obtiene los materiales creados
    /// </summary>
    public Material[] GetShipMaterials()
    {
        Material[] materials = new Material[shipColors.Length];
        
        for (int i = 0; i < shipColors.Length; i++)
        {
            string materialPath = savePath + materialNamePrefix + i + ".mat";
            Material material = Resources.Load<Material>(materialPath);
            
            if (material == null)
            {
                // Si no existe, crear uno temporal
                material = CreateShipMaterial(i, shipColors[i]);
            }
            
            materials[i] = material;
        }
        
        return materials;
    }
    
    /// <summary>
    /// Configura automáticamente el SimpleShopUI con los materiales creados
    /// </summary>
    [ContextMenu("Configurar SimpleShopUI")]
    public void ConfigureSimpleShopUI()
    {
        SimpleShopUI shopUI = FindObjectOfType<SimpleShopUI>();
        
        if (shopUI == null)
        {
            Debug.LogWarning("SimpleShopUI no encontrado en la escena");
            return;
        }
        
        Material[] materials = GetShipMaterials();
        
        // Usar reflexión para configurar los materiales
        var uiType = typeof(SimpleShopUI);
        var materialsField = uiType.GetField("shipMaterials", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (materialsField != null)
        {
            materialsField.SetValue(shopUI, materials);
            Debug.Log($"SimpleShopUI configurado con {materials.Length} materiales");
        }
        else
        {
            Debug.LogError("No se pudo configurar SimpleShopUI");
        }
    }
} 