using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider gyroSensitivitySlider;
    [SerializeField] private Toggle vibrationToggle;
    
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI gyroSensitivityText;
    [SerializeField] private TextMeshProUGUI vibrationText;
    
    [Header("Default Values")]
    [SerializeField] private float defaultMusicVolume = 0.7f;
    [SerializeField] private float defaultSFXVolume = 0.8f;
    [SerializeField] private float defaultGyroSensitivity = 1.5f;
    [SerializeField] private bool defaultVibration = true;
    
    [Header("Slider Ranges")]
    [SerializeField] private float minGyroSensitivity = 0.1f;
    [SerializeField] private float maxGyroSensitivity = 5f;
    
    // PlayerPrefs Keys
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string GYRO_SENSITIVITY_KEY = "GyroSensitivity";
    private const string VIBRATION_KEY = "Vibration";
    
    // Singleton
    public static OptionsManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        LoadSettings();
        SetupUI();
    }
    
    /// <summary>
    /// Configura los listeners de la UI
    /// </summary>
    private void SetupUI()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        
        if (gyroSensitivitySlider != null)
        {
            gyroSensitivitySlider.onValueChanged.AddListener(OnGyroSensitivityChanged);
        }
        
        if (vibrationToggle != null)
        {
            vibrationToggle.onValueChanged.AddListener(OnVibrationChanged);
        }
    }
    
    /// <summary>
    /// Carga la configuración guardada
    /// </summary>
    private void LoadSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
        float gyroSensitivity = PlayerPrefs.GetFloat(GYRO_SENSITIVITY_KEY, defaultGyroSensitivity);
        bool vibration = PlayerPrefs.GetInt(VIBRATION_KEY, defaultVibration ? 1 : 0) == 1;
        
        // Aplicar configuración a todos los sistemas
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        SetGyroSensitivity(gyroSensitivity);
        SetVibration(vibration);
    }
    
    /// <summary>
    /// Guarda la configuración actual
    /// </summary>
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, GetMusicVolume());
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, GetSFXVolume());
        PlayerPrefs.SetFloat(GYRO_SENSITIVITY_KEY, GetGyroSensitivity());
        PlayerPrefs.SetInt(VIBRATION_KEY, GetVibration() ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Actualiza la UI con los valores actuales
    /// </summary>
    private void UpdateUI()
    {
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = GetMusicVolume();
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = GetSFXVolume();
        }
        
        if (gyroSensitivitySlider != null)
        {
            gyroSensitivitySlider.value = GetGyroSensitivity();
        }
        
        if (vibrationToggle != null)
        {
            vibrationToggle.isOn = GetVibration();
        }
        
        UpdateTexts();
    }
    
    /// <summary>
    /// Actualiza los textos de la UI
    /// </summary>
    private void UpdateTexts()
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = $"{GetMusicVolume():P0}";
        }
        
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = $"{GetSFXVolume():P0}";
        }
        
        if (gyroSensitivityText != null)
        {
            gyroSensitivityText.text = $"{GetGyroSensitivity():F1}";
        }
        
        if (vibrationText != null)
        {
            vibrationText.text = $"{(GetVibration() ? "ON" : "OFF")}";
        }
    }
    
    // Event Handlers
    private void OnMusicVolumeChanged(float value)
    {
        SetMusicVolume(value);
        UpdateTexts();
        SaveSettings();
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
        UpdateTexts();
        SaveSettings();
    }
    
    private void OnGyroSensitivityChanged(float value)
    {
        SetGyroSensitivity(value);
        UpdateTexts();
        SaveSettings();
    }
    
    private void OnVibrationChanged(bool value)
    {
        SetVibration(value);
        UpdateTexts();
        SaveSettings();
    }
    
    // Public Methods
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        
        // Aplicar al SectorMusicManager si existe (juego)
        SectorMusicManager sectorMusic = FindObjectOfType<SectorMusicManager>();
        if (sectorMusic != null)
        {
            sectorMusic.SetMusicVolume(volume);
        }
        
        // Aplicar al MainMenuAudioManager si existe (menú principal)
        MainMenuAudioManager mainMenuAudio = FindObjectOfType<MainMenuAudioManager>();
        if (mainMenuAudio != null)
        {
            mainMenuAudio.SetMusicVolume(volume);
        }
        
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }
    
    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
    }
    
    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        
        // Aplicar al GameAudioManager si existe (juego)
        GameAudioManager gameAudio = FindObjectOfType<GameAudioManager>();
        if (gameAudio != null)
        {
            gameAudio.SetSFXVolume(volume);
        }
        
        // Aplicar al MainMenuAudioManager si existe (menú principal)
        MainMenuAudioManager mainMenuAudio = FindObjectOfType<MainMenuAudioManager>();
        if (mainMenuAudio != null)
        {
            mainMenuAudio.SetSFXVolume(volume);
        }
        
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }
    
    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
    }
    
    public void SetGyroSensitivity(float sensitivity)
    {
        sensitivity = Mathf.Clamp(sensitivity, minGyroSensitivity, maxGyroSensitivity);
        
        // Aplicar al PlayerController si existe
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.gyroSensitivity = sensitivity;
        }
        
        PlayerPrefs.SetFloat(GYRO_SENSITIVITY_KEY, sensitivity);
    }
    
    public float GetGyroSensitivity()
    {
        return PlayerPrefs.GetFloat(GYRO_SENSITIVITY_KEY, defaultGyroSensitivity);
    }
    
    public void SetVibration(bool enabled)
    {
        // Aplicar vibración si está disponible
        if (SystemInfo.supportsVibration)
        {
            // La vibración se aplicará cuando sea necesaria
        }
        
        PlayerPrefs.SetInt(VIBRATION_KEY, enabled ? 1 : 0);
    }
    
    public bool GetVibration()
    {
        return PlayerPrefs.GetInt(VIBRATION_KEY, defaultVibration ? 1 : 0) == 1;
    }
    
    /// <summary>
    /// Reproduce vibración si está habilitada
    /// </summary>
    public void PlayVibration(float duration = 0.1f)
    {
        if (GetVibration() && SystemInfo.supportsVibration)
        {
            Handheld.Vibrate();
        }
    }
    
    /// <summary>
    /// Resetea todas las opciones a los valores por defecto
    /// </summary>
    public void ResetToDefaults()
    {
        SetMusicVolume(defaultMusicVolume);
        SetSFXVolume(defaultSFXVolume);
        SetGyroSensitivity(defaultGyroSensitivity);
        SetVibration(defaultVibration);
        
        UpdateUI();
        SaveSettings();
    }
    
    /// <summary>
    /// Abre el panel de opciones
    /// </summary>
    public void OpenOptions()
    {
        UpdateUI();
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Cierra el panel de opciones
    /// </summary>
    public void CloseOptions()
    {
        gameObject.SetActive(false);
    }
    
    // Context Menu methods for testing
    [ContextMenu("Test Vibration")]
    private void TestVibration()
    {
        PlayVibration(0.2f);
    }
    
    [ContextMenu("Reset All Settings")]
    private void ResetAllSettings()
    {
        ResetToDefaults();
    }
    
    [ContextMenu("Print Current Settings")]
    private void PrintCurrentSettings()
    {
        Debug.Log($"Music Volume: {GetMusicVolume():P0}");
        Debug.Log($"SFX Volume: {GetSFXVolume():P0}");
        Debug.Log($"Gyro Sensitivity: {GetGyroSensitivity():F1}");
        Debug.Log($"Vibration: {GetVibration()}");
    }
} 