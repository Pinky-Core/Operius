# Sistema de Audio Separado - Menú y Juego

Este sistema ahora tiene scripts completamente separados para el menú principal y la escena del juego, evitando conflictos y mejorando la organización.

## 📁 Estructura de Archivos

### **Scripts Compartidos:**
- `Assets/Scripts/Audio/SimpleSoundEffects.cs` - Generador de efectos de sonido (compartido)

### **Scripts del Menú Principal:**
- `Assets/MainMenu/MenuAudioManager.cs` - Audio manager específico del menú
- `Assets/MainMenu/ButtonAudioHandler.cs` - Maneja efectos de botones del menú
- `Assets/MainMenu/AudioSetupHelper.cs` - Helper para configuración del menú

### **Scripts del Juego:**
- `Assets/Scripts/GameAudioManager.cs` - Audio manager específico del juego
- `Assets/Scripts/GameAudioSetup.cs` - Helper para configuración del juego

## 🎵 Características por Sistema

### **MenuAudioManager** (Menú Principal)
- ✅ Música de fondo con cambio automático
- ✅ Efectos de sonido para rotación del menú radial
- ✅ Sonidos de clic y hover de botones
- ✅ Transiciones suaves entre canciones
- ✅ Singleton específico para el menú

### **GameAudioManager** (Escena del Juego)
- ✅ Sonido de disparo automático
- ✅ Sonido de muerte del jugador
- ✅ Sonido de recolección de powerups
- ✅ Sonido de game over
- ✅ Singleton específico para el juego

## 🚀 Configuración

### **Para el Menú Principal:**

1. **Crear el Audio Manager del Menú:**
   ```
   GameObject "MenuAudioManager" → Agregar MenuAudioManager
   ```

2. **Configurar música:**
   - Arrastrar archivos .wav desde `Assets/Importado/Sc-Fi Music/Music/WAVs/`
   - Marcar "Use Generated Sounds" como true
   - Ajustar volúmenes según prefieras

3. **Configurar efectos (opcional):**
   ```
   GameObject → Agregar AudioSetupHelper
   Marcar "Setup On Start" como true
   ```

### **Para la Escena del Juego:**

1. **Crear el Audio Manager del Juego:**
   ```
   GameObject "GameAudioManager" → Agregar GameAudioManager
   ```

2. **Configurar efectos:**
   - Marcar "Use Generated Sounds" como true
   - Ajustar frecuencias según prefieras

3. **Configuración automática (opcional):**
   ```
   GameObject → Agregar GameAudioSetup
   Marcar "Setup On Start" como true
   ```

## 🎮 Uso en el Código

### **Menú Principal:**
```csharp
// Reproducir efectos del menú
MenuAudioManager.Instance.PlayButtonClickSound();
MenuAudioManager.Instance.PlayMenuRotateSound();
MenuAudioManager.Instance.PlayButtonHoverSound();

// Control de música del menú
MenuAudioManager.Instance.PlayNextMusic();
MenuAudioManager.Instance.SetMusicVolume(0.5f);
```

### **Escena del Juego:**
```csharp
// Reproducir efectos del juego
GameAudioManager.Instance.PlayShootSound();
GameAudioManager.Instance.PlayDeathSound();
GameAudioManager.Instance.PlayPowerupCollectSound();

// Control de efectos del juego
GameAudioManager.Instance.SetSFXVolume(0.6f);
GameAudioManager.Instance.PlayShootSoundWithVariation(0.1f);
```

## 🔧 Ventajas de la Separación

### **Independencia Total:**
- Cada sistema maneja su propio contexto
- No hay conflictos entre menú y juego
- Volúmenes independientes para cada sistema

### **Mejor Organización:**
- Scripts específicos para cada contexto
- Fácil mantenimiento y debugging
- Código más limpio y modular

### **Flexibilidad:**
- Puedes tener diferentes configuraciones
- Fácil de extender para nuevas funcionalidades
- Control granular de cada sistema

## 🎛️ Configuración Avanzada

### **Efectos Generados Compartidos:**
El `SimpleSoundEffects.cs` es compartido entre ambos sistemas y incluye:

- **Para Menú:** Click, Hover, Rotate
- **Para Juego:** Shoot, Death, Powerup, EnemyDeath, GameOver

### **Configuración de Frecuencias:**

#### **Menú:**
- Click Frequency: 1200 Hz
- Hover Frequency: 600 Hz
- Rotate Start/End: 400-800 Hz

#### **Juego:**
- Shoot Frequency: 800 Hz
- Death Frequency: 200 Hz
- Powerup Frequency: 1200 Hz

## 🧪 Pruebas

### **Probar Menú:**
```csharp
// En AudioSetupHelper
audioSetupHelper.TestSoundEffects();
audioSetupHelper.NextMusic();
```

### **Probar Juego:**
```csharp
// En GameAudioSetup
gameAudioSetup.TestAllSounds();
gameAudioSetup.TestMultipleShots();
```

## 🐛 Solución de Problemas

### **No se escuchan efectos del menú:**
1. Verificar que `MenuAudioManager` esté en la escena
2. Comprobar que "Use Generated Sounds" esté marcado
3. Verificar que el volumen no esté en 0

### **No se escuchan efectos del juego:**
1. Verificar que `GameAudioManager` esté en la escena
2. Comprobar que "Use Generated Sounds" esté marcado
3. Verificar que el volumen no esté en 0

### **Conflictos entre sistemas:**
- Cada sistema es completamente independiente
- No deberían haber conflictos
- Verificar que cada manager esté en su escena correspondiente

## 📝 Notas Importantes

- **MenuAudioManager** y **GameAudioManager** son Singletons independientes
- Cada uno maneja su propio contexto y no interfieren entre sí
- Los efectos generados se crean en tiempo de ejecución
- El sistema es compatible con Unity 2020.3 y versiones posteriores

## 🎯 Beneficios de la Separación

1. **Mantenimiento:** Más fácil de mantener y debuggear
2. **Escalabilidad:** Fácil agregar nuevas funcionalidades
3. **Rendimiento:** Cada sistema optimizado para su contexto
4. **Flexibilidad:** Configuraciones independientes
5. **Organización:** Código más limpio y modular

¡Ahora tienes un sistema de audio completamente separado y profesional! 🎵 