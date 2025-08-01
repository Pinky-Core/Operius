# Sistema de Audio para el Menú Principal

Este sistema te permite agregar música de fondo y efectos de sonido a tu menú principal de Unity.

## 🎵 Características

- **Música de fondo** que cambia automáticamente cuando termina una canción
- **Efectos de sonido** para el giro del menú radial
- **Efectos de sonido** para clics de botones
- **Efectos de sonido** para hover de botones
- **Transiciones suaves** entre canciones
- **Efectos de sonido generados programáticamente** (opcional)

## 🚀 Configuración Rápida

### 1. Crear el Audio Manager

1. En tu escena del menú principal, crea un GameObject vacío llamado "AudioManager"
2. Agrega el componente `MainMenuAudioManager` al GameObject
3. Configura los siguientes campos:
   - **Background Music Clips**: Arrastra los archivos de música desde `Assets/Importado/Sc-Fi Music/Music/WAVs/`
   - **Music Volume**: 0.7 (recomendado)
   - **SFX Volume**: 0.8 (recomendado)
   - **Loop Music**: false (para que cambie automáticamente)

### 2. Configurar Efectos de Sonido

#### Opción A: Efectos Generados Automáticamente
1. Agrega el componente `AudioSetupHelper` al mismo GameObject
2. Marca "Use Generated Sounds" como true
3. Ajusta las frecuencias según prefieras:
   - Click Frequency: 1200
   - Hover Frequency: 600
   - Rotate Start Freq: 400
   - Rotate End Freq: 800

#### Opción B: Efectos Personalizados
1. Crea o descarga efectos de sonido (.wav o .ogg)
2. Asigna los archivos en el `MainMenuAudioManager`:
   - Menu Rotate Sound
   - Button Click Sound
   - Button Hover Sound

### 3. Agregar Audio a los Botones

#### Opción A: Automático
1. En el `AudioSetupHelper`, llama al método `AddAudioHandlersToButtons()`
2. Esto agregará automáticamente el componente `ButtonAudioHandler` a todos los botones

#### Opción B: Manual
1. Para cada botón, agrega el componente `ButtonAudioHandler`
2. Configura si quieres sonido de hover y clic

## 📁 Archivos de Música Disponibles

En la carpeta `Assets/Importado/Sc-Fi Music/Music/WAVs/` tienes disponibles:

- **A New Planet.wav** - Música suave y atmosférica
- **Among Stars.wav** - Música épica espacial
- **Cold Space.wav** - Música fría y misteriosa
- **Glitch Bot.wav** - Música con efectos glitch
- **Little Engine.wav** - Música rítmica
- **Space Voyager.wav** - Música de exploración espacial

## 🎛️ Configuración Avanzada

### Volúmenes
- **Music Volume**: 0.0 - 1.0 (0.7 recomendado)
- **SFX Volume**: 0.0 - 1.0 (0.8 recomendado)

### Transiciones
- **Fade In Duration**: 1.0 segundos
- **Fade Out Duration**: 1.0 segundos
- **Crossfade Duration**: 2.0 segundos

### Efectos Generados
- **Sample Rate**: 44100 Hz
- **Duration**: 0.1 segundos
- **Frecuencias personalizables** para cada tipo de efecto

## 🔧 Scripts Incluidos

### MainMenuAudioManager.cs
- Maneja toda la lógica de audio
- Patrón Singleton para acceso global
- Transiciones suaves entre canciones
- Control de volúmenes

### ButtonAudioHandler.cs
- Maneja efectos de sonido de botones
- Detecta hover y clic automáticamente
- Permite efectos personalizados

### SimpleSoundEffects.cs
- Genera efectos de sonido programáticamente
- Incluye: beep, click, hover, rotate, pop, whoosh
- Completamente personalizable

### AudioSetupHelper.cs
- Facilita la configuración inicial
- Genera efectos automáticamente
- Agrega handlers a botones automáticamente

## 🎮 Uso en el Código

### Reproducir Efectos Manualmente
```csharp
// Reproducir sonido de clic
MainMenuAudioManager.Instance.PlayButtonClickSound();

// Reproducir sonido de hover
MainMenuAudioManager.Instance.PlayButtonHoverSound();

// Reproducir sonido de rotación
MainMenuAudioManager.Instance.PlayMenuRotateSound();
```

### Control de Música
```csharp
// Siguiente canción
MainMenuAudioManager.Instance.PlayNextMusic();

// Canción anterior
MainMenuAudioManager.Instance.PlayPreviousMusic();

// Pausar/Reanudar
MainMenuAudioManager.Instance.PauseMusic();
MainMenuAudioManager.Instance.ResumeMusic();

// Cambiar volumen
MainMenuAudioManager.Instance.SetMusicVolume(0.5f);
MainMenuAudioManager.Instance.SetSFXVolume(0.6f);
```

### Información de Música
```csharp
// Obtener información
string currentSong = MainMenuAudioManager.Instance.GetCurrentMusicName();
int currentIndex = MainMenuAudioManager.Instance.GetCurrentMusicIndex();
int totalSongs = MainMenuAudioManager.Instance.GetTotalMusicCount();
```

## 🐛 Solución de Problemas

### No se reproduce música
1. Verifica que los AudioClips estén asignados en el MainMenuAudioManager
2. Asegúrate de que el volumen no esté en 0
3. Verifica que "Play Music On Start" esté marcado

### No se escuchan efectos de sonido
1. Verifica que los efectos estén asignados o que "Use Generated Sounds" esté marcado
2. Asegúrate de que el SFX Volume no esté en 0
3. Verifica que los botones tengan el componente ButtonAudioHandler

### La música no cambia automáticamente
1. Asegúrate de que "Loop Music" esté desmarcado
2. Verifica que haya más de una canción en la lista
3. Comprueba que las canciones no estén en loop individual

## 🎨 Personalización

### Crear Efectos Personalizados
```csharp
// En SimpleSoundEffects
AudioClip customSound = CreateCustomSound(dataArray, "MiEfecto");
```

### Modificar Transiciones
```csharp
// En MainMenuAudioManager
fadeInDuration = 2f;  // Transición más lenta
fadeOutDuration = 1.5f;
```

### Agregar Nuevos Tipos de Efectos
1. Crea un nuevo método en `SimpleSoundEffects`
2. Genera el AudioClip con la lógica deseada
3. Asigna el efecto en el `MainMenuAudioManager`

## 📝 Notas Importantes

- El sistema usa el patrón Singleton, por lo que solo debe haber una instancia del `MainMenuAudioManager`
- Los efectos generados se crean en tiempo de ejecución y no se guardan en disco
- Para efectos más complejos, considera usar archivos de audio externos
- El sistema es compatible con Unity 2020.3 y versiones posteriores

## 🎯 Próximas Mejoras

- [ ] Sistema de playlist personalizable
- [ ] Efectos de sonido 3D
- [ ] Integración con Audio Mixer
- [ ] Sistema de equalización
- [ ] Efectos de reverb y delay
- [ ] Soporte para formatos de audio adicionales 