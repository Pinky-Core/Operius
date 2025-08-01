using UnityEngine;
using System.Collections;

public class MusicStartupHelper : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float delayBeforeStart = 0.1f;
    [SerializeField] private bool autoStart = true;
    
    private void Start()
    {
        if (autoStart)
        {
            StartCoroutine(StartMusicDelayed());
        }
    }
    
    /// <summary>
    /// Crea automáticamente un MusicStartupHelper si no existe en la escena
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CreateMusicStartupHelper()
    {
        // Solo crear en escenas de juego, no en menú
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.ToLower();
        if (sceneName.Contains("sample") || sceneName.Contains("game") || sceneName.Contains("level"))
        {
            if (FindObjectOfType<MusicStartupHelper>() == null)
            {
                GameObject helperObj = new GameObject("MusicStartupHelper");
                helperObj.AddComponent<MusicStartupHelper>();
                Debug.Log("MusicStartupHelper creado automáticamente");
            }
        }
    }
    
    /// <summary>
    /// Inicia la música con un pequeño delay para asegurar que todo esté configurado
    /// </summary>
    private IEnumerator StartMusicDelayed()
    {
        Debug.Log("MusicStartupHelper: Esperando para iniciar música...");
        yield return new WaitForSeconds(delayBeforeStart);
        
        // Intentar restaurar música a través de AudioSceneManager
        AudioSceneManager audioSceneManager = FindObjectOfType<AudioSceneManager>();
        if (audioSceneManager != null)
        {
            Debug.Log("MusicStartupHelper: Restaurando música a través de AudioSceneManager");
            audioSceneManager.RestoreSectorMusic();
        }
        else
        {
            Debug.LogWarning("MusicStartupHelper: AudioSceneManager no encontrado");
            
            // Fallback: intentar directamente con SectorMusicManager
            SectorMusicManager sectorMusicManager = FindObjectOfType<SectorMusicManager>();
            if (sectorMusicManager != null)
            {
                Debug.Log("MusicStartupHelper: Iniciando música directamente con SectorMusicManager");
                sectorMusicManager.ForceStartMusic();
            }
            else
            {
                Debug.LogWarning("MusicStartupHelper: SectorMusicManager no encontrado");
            }
        }
    }
    
    /// <summary>
    /// Método público para forzar el inicio de música
    /// </summary>
    public void ForceStartMusic()
    {
        StartCoroutine(StartMusicDelayed());
    }
} 