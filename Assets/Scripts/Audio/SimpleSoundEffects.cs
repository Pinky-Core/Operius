using UnityEngine;

public class SimpleSoundEffects : MonoBehaviour
{
    [Header("Configuración de Efectos")]
    [SerializeField] private int sampleRate = 44100;
    [SerializeField] private float duration = 0.1f;
    
    /// <summary>
    /// Crea un efecto de sonido de "beep" simple
    /// </summary>
    public AudioClip CreateBeepSound(float frequency = 800f, float volume = 0.5f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Beep", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            data[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * volume;
            
            // Aplicar fade out
            float fadeOut = 1f - (float)i / samples;
            data[i] *= fadeOut;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de "click" suave
    /// </summary>
    public AudioClip CreateClickSound(float frequency = 1200f, float volume = 0.3f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Click", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
            
            // Aplicar envelope rápido
            float envelope = Mathf.Exp(-t * 50f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de "hover" suave
    /// </summary>
    public AudioClip CreateHoverSound(float frequency = 600f, float volume = 0.2f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Hover", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
            
            // Aplicar envelope suave
            float envelope = Mathf.Sin(t * Mathf.PI / duration) * Mathf.Exp(-t * 10f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de "rotate" con sweep de frecuencia
    /// </summary>
    public AudioClip CreateRotateSound(float startFreq = 400f, float endFreq = 800f, float volume = 0.4f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Rotate", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / duration;
            
            // Frecuencia que cambia con el tiempo
            float currentFreq = Mathf.Lerp(startFreq, endFreq, progress);
            float wave = Mathf.Sin(2f * Mathf.PI * currentFreq * t);
            
            // Aplicar envelope
            float envelope = Mathf.Sin(progress * Mathf.PI) * Mathf.Exp(-progress * 3f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de disparo
    /// </summary>
    public AudioClip CreateShootSound(float frequency = 800f, float volume = 0.4f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Shoot", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float wave = Mathf.Sin(2f * Mathf.PI * frequency * t);
            
            // Aplicar envelope muy rápido para sonido de disparo
            float envelope = Mathf.Exp(-t * 100f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de muerte
    /// </summary>
    public AudioClip CreateDeathSound(float frequency = 200f, float volume = 0.6f)
    {
        int samples = (int)(sampleRate * duration * 2f); // Más largo para muerte
        AudioClip clip = AudioClip.Create("Death", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / (duration * 2f);
            
            // Frecuencia que baja con el tiempo (efecto de "muerte")
            float currentFreq = Mathf.Lerp(frequency, frequency * 0.1f, progress);
            float wave = Mathf.Sin(2f * Mathf.PI * currentFreq * t);
            
            // Aplicar envelope largo y suave
            float envelope = Mathf.Sin(progress * Mathf.PI) * Mathf.Exp(-progress * 2f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de powerup
    /// </summary>
    public AudioClip CreatePowerupSound(float frequency = 1200f, float volume = 0.5f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Powerup", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / duration;
            
            // Frecuencia que sube (efecto de "powerup")
            float currentFreq = Mathf.Lerp(frequency, frequency * 2f, progress);
            float wave = Mathf.Sin(2f * Mathf.PI * currentFreq * t);
            
            // Aplicar envelope con "ping"
            float envelope = Mathf.Sin(progress * Mathf.PI * 2f) * Mathf.Exp(-progress * 3f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de muerte de enemigo
    /// </summary>
    public AudioClip CreateEnemyDeathSound(float volume = 0.3f)
    {
        int samples = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("EnemyDeath", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / duration;
            
            // Combinar ruido con tono descendente
            float noise = Random.Range(-1f, 1f);
            float tone = Mathf.Sin(2f * Mathf.PI * 300f * t * (1f - progress));
            
            // Aplicar envelope
            float envelope = Mathf.Exp(-progress * 5f);
            data[i] = (noise * 0.4f + tone * 0.6f) * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Crea un efecto de sonido de game over
    /// </summary>
    public AudioClip CreateGameOverSound(float volume = 0.7f)
    {
        int samples = (int)(sampleRate * duration * 3f); // Más largo para game over
        AudioClip clip = AudioClip.Create("GameOver", samples, 1, sampleRate, false);
        
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float progress = t / (duration * 3f);
            
            // Secuencia de tonos descendentes
            float baseFreq = 400f;
            float currentFreq = baseFreq * Mathf.Pow(0.5f, Mathf.Floor(progress * 4f));
            float wave = Mathf.Sin(2f * Mathf.PI * currentFreq * t);
            
            // Aplicar envelope con pausas
            float envelope = Mathf.Sin(progress * Mathf.PI * 8f) * Mathf.Exp(-progress * 1.5f);
            data[i] = wave * envelope * volume;
        }
        
        clip.SetData(data, 0);
        return clip;
    }
    
    /// <summary>
    /// Método de utilidad para crear un AudioClip desde un array de datos
    /// </summary>
    public AudioClip CreateCustomSound(float[] data, string name = "Custom")
    {
        AudioClip clip = AudioClip.Create(name, data.Length, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
} 