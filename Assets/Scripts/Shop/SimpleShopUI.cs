using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI simple para la tienda con imágenes, botones y textos
/// </summary>
public class SimpleShopUI : MonoBehaviour
{
    [Header("Panel Principal")]
    [SerializeField] private GameObject shopPanel;
    
    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI titleText;
    
    [Header("Botones de Nave")]
    [SerializeField] private Button[] shipButtons = new Button[4];
    [SerializeField] private Image[] shipImages = new Image[4];
    [SerializeField] private TextMeshProUGUI[] shipNames = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI[] shipPrices = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI[] buttonTexts = new TextMeshProUGUI[4]; // Textos de los botones
    
    [Header("Botón Cerrar")]
    [SerializeField] private Button closeButton;
    
    [Header("Materiales de Nave")]
    [SerializeField] private Material[] shipMaterials;
    
    [Header("Colores")]
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private Color lockedColor = Color.red;
    [SerializeField] private Color selectedColor = Color.blue;
    
    private int currentShipStyle = 0;
    
    void Start()
    {
        // Configurar botón cerrar
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseShop);
        }
        
        // Configurar botones de nave
        SetupShipButtons();
        
        // Cargar datos guardados
        LoadShopData();
        
        // Actualizar UI
        UpdateUI();
        
        // Ocultar panel al inicio
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// Configura los botones de nave
    /// </summary>
    private void SetupShipButtons()
    {
        string[] shipNames = { "Basic", "Fast", "Defensive", "Elite" };
        int[] shipPrices = { 0, 10, 15, 30 };
        
        for (int i = 0; i < shipButtons.Length; i++)
        {
            if (shipButtons[i] != null)
            {
                int shipIndex = i; // Capturar índice para el lambda
                shipButtons[i].onClick.AddListener(() => HandleShipButton(shipIndex));
                
                // Configurar nombre
                if (this.shipNames[i] != null)
                {
                    this.shipNames[i].text = shipNames[i];
                }
                
                // Configurar precio
                if (this.shipPrices[i] != null)
                {
                    if (i == 0)
                    {
                        this.shipPrices[i].text = "FREE";
                    }
                    else
                    {
                        this.shipPrices[i].text = $"{shipPrices[i]}";
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Abre la tienda
    /// </summary>
    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            UpdateUI();
            Debug.Log("Tienda abierta");
        }
    }
    
    /// <summary>
    /// Cierra la tienda
    /// </summary>
    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("Tienda cerrada");
        }
    }
    
    /// <summary>
    /// Maneja el click del botón de nave (comprar o seleccionar)
    /// </summary>
    public void HandleShipButton(int shipIndex)
    {
        if (shipIndex == 0)
        {
            // Nave básica siempre disponible
            SelectShip(shipIndex);
        }
        else if (IsShipUnlocked(shipIndex))
        {
            // Nave desbloqueada - seleccionar
            SelectShip(shipIndex);
        }
        else
        {
            // Nave bloqueada - intentar comprar
            BuyShip(shipIndex);
        }
    }
    
    /// <summary>
    /// Selecciona una nave (solo si está desbloqueada)
    /// </summary>
    private void SelectShip(int shipIndex)
    {
        currentShipStyle = shipIndex;
        ApplyShipMaterial(shipIndex);
        SaveShopData();
        UpdateUI();
        Debug.Log($"Nave {shipIndex} seleccionada");
    }
    
    /// <summary>
    /// Compra una nave
    /// </summary>
    private void BuyShip(int shipIndex)
    {
        // Verificar si tiene suficientes monedas
        int requiredCoins = GetShipPrice(shipIndex);
        int currentCoins = GetPlayerCoins();
        
        if (currentCoins >= requiredCoins)
        {
            // Comprar nave
            SpendCoins(requiredCoins);
            Debug.Log($"Nave {shipIndex} comprada por {requiredCoins} monedas");
            UpdateUI();
        }
        else
        {
            Debug.Log($"No tienes suficientes monedas. Necesitas {requiredCoins}, tienes {currentCoins}");
        }
    }
    
    /// <summary>
    /// Obtiene el precio de una nave
    /// </summary>
    private int GetShipPrice(int shipIndex)
    {
        int[] prices = { 0, 10, 15, 30 };
        if (shipIndex >= 0 && shipIndex < prices.Length)
        {
            return prices[shipIndex];
        }
        return 999;
    }
    
    /// <summary>
    /// Aplica el material de la nave
    /// </summary>
    private void ApplyShipMaterial(int shipIndex)
    {
        if (shipMaterials != null && shipIndex < shipMaterials.Length)
        {
            // Buscar la nave del jugador
            GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
            if (playerShip == null)
            {
                playerShip = GameObject.Find("Player");
            }
            
            if (playerShip != null)
            {
                Renderer renderer = playerShip.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = shipMaterials[shipIndex];
                    Debug.Log($"Material aplicado: Nave {shipIndex}");
                }
            }
        }
    }
    
    /// <summary>
    /// Actualiza la UI
    /// </summary>
    private void UpdateUI()
    {
        // Actualizar puntos y monedas
        if (pointsText != null)
        {
            int points = GetPlayerPoints();
            int pointsPerCoin = 100;
            pointsText.text = $"Points: {points}/{pointsPerCoin}";
        }
        
        if (coinsText != null)
        {
            int coins = GetPlayerCoins();
            coinsText.text = $"Coins: {coins}";
        }
        
        // Actualizar botones de nave
        for (int i = 0; i < shipButtons.Length; i++)
        {
            if (shipButtons[i] != null)
            {
                // Determinar color del botón
                Color buttonColor;
                
                if (i == currentShipStyle)
                {
                    buttonColor = selectedColor; // Seleccionada
                }
                else if (i == 0 || IsShipUnlocked(i))
                {
                    buttonColor = unlockedColor; // Desbloqueada
                }
                else
                {
                    buttonColor = lockedColor; // Bloqueada
                }
                
                // Aplicar color al botón
                Image buttonImage = shipButtons[i].GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = buttonColor;
                }
                
                // Actualizar precio
                if (shipPrices[i] != null)
                {
                    if (i == currentShipStyle)
                    {
                        shipPrices[i].text = "SELECTED";
                        shipPrices[i].color = Color.white;
                    }
                    else if (i == 0 || IsShipUnlocked(i))
                    {
                        shipPrices[i].text = "AVAILABLE";
                        shipPrices[i].color = Color.white;
                    }
                    else
                    {
                        int price = GetShipPrice(i);
                        shipPrices[i].text = $"{price}";
                        shipPrices[i].color = Color.yellow;
                    }
                }
                
                // Actualizar texto del botón
                if (buttonTexts[i] != null)
                {
                    if (i == 0)
                    {
                        buttonTexts[i].text = "SELECT";
                        buttonTexts[i].color = Color.white;
                    }
                    else if (i == currentShipStyle)
                    {
                        buttonTexts[i].text = "SELECTED";
                        buttonTexts[i].color = Color.white;
                    }
                    else if (IsShipUnlocked(i))
                    {
                        buttonTexts[i].text = "SELECT";
                        buttonTexts[i].color = Color.white;
                    }
                    else
                    {
                        int price = GetShipPrice(i);
                        buttonTexts[i].text = $"BUY ({price})";
                        buttonTexts[i].color = Color.yellow;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Verifica si una nave está desbloqueada
    /// </summary>
    private bool IsShipUnlocked(int shipIndex)
    {
        if (shipIndex == 0) return true; // Nave básica siempre disponible
        
        // Verificar si el jugador tiene suficientes monedas para comprarla
        int requiredCoins = GetShipPrice(shipIndex);
        int currentCoins = GetPlayerCoins();
        
        return currentCoins >= requiredCoins;
    }
    
    /// <summary>
    /// Gasta monedas
    /// </summary>
    private void SpendCoins(int amount)
    {
        int currentCoins = GetPlayerCoins();
        int newCoins = currentCoins - amount;
        PlayerPrefs.SetInt("PlayerCoins", newCoins);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Obtiene las monedas del jugador
    /// </summary>
    private int GetPlayerCoins()
    {
        return PlayerPrefs.GetInt("PlayerCoins", 0);
    }
    
    /// <summary>
    /// Obtiene los puntos del jugador
    /// </summary>
    private int GetPlayerPoints()
    {
        return PlayerPrefs.GetInt("PlayerPoints", 0);
    }
    
    /// <summary>
    /// Guarda los datos de la tienda
    /// </summary>
    private void SaveShopData()
    {
        PlayerPrefs.SetInt("CurrentShipStyle", currentShipStyle);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Carga los datos de la tienda
    /// </summary>
    private void LoadShopData()
    {
        currentShipStyle = PlayerPrefs.GetInt("CurrentShipStyle", 0);
    }
    
    /// <summary>
    /// Agrega monedas (para testing)
    /// </summary>
    [ContextMenu("Agregar 10 Monedas")]
    public void AddCoins()
    {
        int currentCoins = GetPlayerCoins();
        PlayerPrefs.SetInt("PlayerCoins", currentCoins + 500);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log("10 monedas agregadas");
    }
    
    /// <summary>
    /// Resetea los datos (para testing)
    /// </summary>
    [ContextMenu("Resetear Datos")]
    public void ResetData()
    {
        PlayerPrefs.DeleteKey("PlayerCoins");
        PlayerPrefs.DeleteKey("PlayerPoints");
        PlayerPrefs.DeleteKey("CurrentShipStyle");
        PlayerPrefs.Save();
        currentShipStyle = 0;
        UpdateUI();
        Debug.Log("Datos reseteados");
    }
} 