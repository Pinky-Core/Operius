using UnityEngine;

/// <summary>
/// Helper para aplicar vibración en eventos del juego
/// </summary>
public static class VibrationHelper
{
    /// <summary>
    /// Vibración para disparo
    /// </summary>
    public static void PlayShootVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.05f);
        }
    }
    
    /// <summary>
    /// Vibración para recolección de powerup
    /// </summary>
    public static void PlayPowerupVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.1f);
        }
    }
    
    /// <summary>
    /// Vibración para muerte del jugador
    /// </summary>
    public static void PlayDeathVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.3f);
        }
    }
    
    /// <summary>
    /// Vibración para muerte de enemigo
    /// </summary>
    public static void PlayEnemyDeathVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.08f);
        }
    }
    
    /// <summary>
    /// Vibración para game over
    /// </summary>
    public static void PlayGameOverVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.5f);
        }
    }
    
    /// <summary>
    /// Vibración para selección en menú
    /// </summary>
    public static void PlayMenuSelectVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.03f);
        }
    }
    
    /// <summary>
    /// Vibración para confirmación en menú
    /// </summary>
    public static void PlayMenuConfirmVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.06f);
        }
    }
    
    /// <summary>
    /// Vibración para error o cancelación
    /// </summary>
    public static void PlayErrorVibration()
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(0.15f);
        }
    }
    
    /// <summary>
    /// Vibración personalizada
    /// </summary>
    public static void PlayCustomVibration(float duration)
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.PlayVibration(duration);
        }
    }
    
    /// <summary>
    /// Vibración de patrón (múltiples vibraciones)
    /// </summary>
    public static void PlayPatternVibration(float[] durations, float[] delays)
    {
        if (OptionsManager.Instance != null && OptionsManager.Instance.GetVibration())
        {
            // Crear un GameObject temporal para manejar el patrón
            GameObject tempVibration = new GameObject("TempVibration");
            VibrationPattern pattern = tempVibration.AddComponent<VibrationPattern>();
            pattern.PlayPattern(durations, delays);
        }
    }
}

/// <summary>
/// Componente temporal para manejar patrones de vibración
/// </summary>
public class VibrationPattern : MonoBehaviour
{
    public void PlayPattern(float[] durations, float[] delays)
    {
        StartCoroutine(PlayPatternCoroutine(durations, delays));
    }
    
    private System.Collections.IEnumerator PlayPatternCoroutine(float[] durations, float[] delays)
    {
        for (int i = 0; i < durations.Length; i++)
        {
            if (OptionsManager.Instance != null)
            {
                OptionsManager.Instance.PlayVibration(durations[i]);
            }
            
            if (i < delays.Length)
            {
                yield return new WaitForSeconds(delays[i]);
            }
        }
        
        // Destruir el GameObject temporal
        Destroy(gameObject);
    }
} 