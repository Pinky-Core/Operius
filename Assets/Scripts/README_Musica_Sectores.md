# Sistema de Música por Sectores

Este sistema implementa música específica para cada sector del juego, con transiciones suaves, prevención de superposición de sonidos y gestión automática del audio.

## 🎵 Características Implementadas

### ✅ **Música por Sectores**
- Música específica para cada nivel de sector
- Loop automático si no se ha cambiado de sector
- Transiciones suaves entre sectores

### ✅ **Prevención de Superposición**
- Sistema de cola para efectos de sonido
- Tiempo mínimo entre sonidos
- Evita mezcla de efectos de audio

### ✅ **Gestión de Audio**
- Detiene música del juego al volver al menú
- Sistema independiente para menú y juego
- Transiciones con crossfade opcional

## 📁 Scripts del Sistema

### **Scripts Principales:**
- `SectorMusicManager.cs` - Núcleo del sistema de música por sectores
- `GameAudioManager.cs` - Audio manager del juego (actualizado)
- `SectorMusicSetup.cs` - Helper para configuración

### **Scripts Modificados:**
- `PlayerShooting.cs` - Cambia música automáticamente al subir de sector
- `MenuButton.cs` - Detiene música del juego al volver al menú

## 🚀 Configuración

### **1. Configurar Música por Sectores**

1. **Crear el SectorMusicManager:**
   ```
   GameObject "SectorMusicManager" → Agregar SectorMusicManager
   ```

2. **Configurar música para cada sector:**
   - **Sector 0:** Música inicial (ej: "A New Planet.wav")
   - **Sector 1:** Música de nivel 1 (ej: "Among Stars.wav")
   - **Sector 2:** Música de nivel 2 (ej: "Cold Space.wav")
   - **Sector 3:** Música de nivel 3 (ej: "Glitch Bot.wav")
   - **Sector 4:** Música de nivel 4 (ej: "Little Engine.wav")

3. **Configurar propiedades por sector:**
   - **Volume:** 0.7f (recomendado)
   - **Loop:** true (para que no se corte)
   - **Fade In Duration:** 1f
   - **Fade Out Duration:** 1f

### **2. Configuración Automática (Opcional)**

1. **Agregar SectorMusicSetup:**
   ```
   GameObject → Agregar SectorMusicSetup
   Marcar "Setup On Start" como true
   ```

2. **Configurar arrays:**
   - **Sector Music Clips:** Arrastrar archivos de música
   - **Sector Volumes:** [0.7, 0.7, 0.7, 0.7, 0.7]
   - **Sector Loops:** [true, true, true, true, true]

## 🎮 Funcionamiento

### **Cambio Automático de Música:**
```csharp
// En PlayerShooting.cs - Se ejecuta automáticamente
public void CollectPowerup()
{
    if (powerupLevel < 3)
    {
        powerupLevel++;
    }
    else
    {
        sectorLevel++;
        powerupLevel = 0;
        
        // Cambiar música al nuevo sector
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.ChangeToSector(sectorLevel);
        }
    }
}
```

### **Detener Música al Volver al Menú:**
```csharp
// En MenuButton.cs - Se ejecuta automáticamente
case MenuAction.MainMenu:
    // Detener música del juego antes de volver al menú
    if (GameAudioManager.Instance != null)
    {
        GameAudioManager.Instance.StopGameMusic();
    }
    SceneManager.LoadScene("MainMenu");
    break;
```

## 🎛️ Configuración Avanzada

### **Transiciones de Música:**

#### **Crossfade (Recomendado):**
- **Use Crossfade:** true
- **Crossfade Duration:** 2f segundos
- **Ventaja:** Transición suave sin cortes

#### **Fade Simple:**
- **Use Crossfade:** false
- **Fade In Duration:** 1f segundo
- **Fade Out Duration:** 1f segundo
- **Ventaja:** Más control sobre la transición

### **Prevención de Superposición:**

#### **Configuración:**
- **Prevent Sound Overlap:** true
- **Min Time Between Sounds:** 0.1f segundos
- **Ventaja:** Evita mezcla de efectos de sonido

#### **Sistema de Cola:**
- Los sonidos se encolan si se reproducen muy rápido
- Se procesan en orden con el tiempo mínimo
- Se puede limpiar la cola manualmente

## 🎮 Uso en el Código

### **Control de Música por Sectores:**
```csharp
// Cambiar al sector especificado
GameAudioManager.Instance.ChangeToSector(2);

// Obtener sector actual
int currentSector = GameAudioManager.Instance.GetCurrentSector();

// Obtener nombre de música actual
string musicName = GameAudioManager.Instance.GetCurrentMusicName();
```

### **Control de Efectos de Sonido:**
```csharp
// Reproducir efectos (con prevención de superposición)
GameAudioManager.Instance.PlayShootSound();
GameAudioManager.Instance.PlayPowerupCollectSound();

// Control de superposición
GameAudioManager.Instance.SetPreventSoundOverlap(false);
```

### **Control de Música:**
```csharp
// Detener música del juego
GameAudioManager.Instance.StopGameMusic();

// Pausar/Reanudar música
GameAudioManager.Instance.PauseGameMusic();
GameAudioManager.Instance.ResumeGameMusic();
```

## 🧪 Pruebas

### **Probar Música por Sectores:**
```csharp
// En SectorMusicSetup
sectorMusicSetup.TestSectorMusic();
```

### **Probar Efectos de Sonido:**
```csharp
// En GameAudioSetup
gameAudioSetup.TestAllSounds();
gameAudioSetup.TestMultipleShots();
```

### **Probar Prevención de Superposición:**
```csharp
// Reproducir muchos disparos rápidamente
for (int i = 0; i < 10; i++)
{
    GameAudioManager.Instance.PlayShootSound();
    yield return new WaitForSeconds(0.05f);
}
```

## 🎵 Música Recomendada por Sector

### **Sector 0 (Inicio):**
- **A New Planet.wav** - Música suave y atmosférica
- **Volumen:** 0.6f
- **Loop:** true

### **Sector 1 (Primer Nivel):**
- **Among Stars.wav** - Música épica espacial
- **Volumen:** 0.7f
- **Loop:** true

### **Sector 2 (Segundo Nivel):**
- **Cold Space.wav** - Música fría y misteriosa
- **Volumen:** 0.7f
- **Loop:** true

### **Sector 3 (Tercer Nivel):**
- **Glitch Bot.wav** - Música con efectos glitch
- **Volumen:** 0.8f
- **Loop:** true

### **Sector 4 (Nivel Final):**
- **Little Engine.wav** - Música rítmica e intensa
- **Volumen:** 0.8f
- **Loop:** true

## 🔧 Ventajas del Sistema

### **Experiencia de Usuario:**
- ✅ Música que evoluciona con el progreso
- ✅ Transiciones suaves sin cortes
- ✅ Efectos de sonido limpios sin mezcla
- ✅ Audio optimizado para cada contexto

### **Desarrollo:**
- ✅ Fácil configuración por sector
- ✅ Sistema modular y extensible
- ✅ Control granular del audio
- ✅ Debugging simplificado

### **Rendimiento:**
- ✅ Prevención de superposición de audio
- ✅ Gestión eficiente de recursos
- ✅ Transiciones optimizadas
- ✅ Cola de sonidos inteligente

## 🐛 Solución de Problemas

### **La música no cambia de sector:**
1. Verificar que `SectorMusicManager` esté configurado
2. Comprobar que los clips de música estén asignados
3. Verificar que `PlayerShooting` llame a `ChangeToSector`

### **Los efectos de sonido se mezclan:**
1. Verificar que "Prevent Sound Overlap" esté habilitado
2. Ajustar "Min Time Between Sounds" si es necesario
3. Usar `ClearSoundQueue()` para limpiar la cola

### **La música del juego sigue en el menú:**
1. Verificar que `MenuButton` llame a `StopGameMusic`
2. Comprobar que el `SectorMusicManager` se detenga correctamente
3. Verificar que el `MenuAudioManager` esté funcionando

## 📝 Notas Importantes

- **SectorMusicManager** es un Singleton que persiste entre escenas
- **GameAudioManager** maneja los efectos de sonido del juego
- **MenuAudioManager** maneja el audio del menú principal
- Los sistemas son completamente independientes
- La música se cambia automáticamente al subir de sector
- La música se detiene automáticamente al volver al menú

## 🎯 Próximas Mejoras

- [ ] Música dinámica basada en eventos del juego
- [ ] Sistema de playlist personalizable por sector
- [ ] Efectos de sonido 3D para enemigos
- [ ] Integración con Audio Mixer
- [ ] Sistema de equalización dinámica
- [ ] Efectos de sonido ambientales

¡Ahora tienes un sistema de música por sectores completamente funcional y profesional! 🎵 