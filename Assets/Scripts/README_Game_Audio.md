# Sistema de Audio para el Juego

Este sistema maneja todos los efectos de sonido del juego, incluyendo disparos, muerte del jugador y recolección de powerups.

## 🎮 Efectos de Sonido Implementados

### ✅ **Disparo** (`PlayerShooting.cs`)
- Se reproduce cada vez que el jugador dispara
- Sonido generado programáticamente con frecuencia ajustable
- Incluye variación de pitch para disparos múltiples

### ✅ **Muerte del Jugador** (`PlayerController.cs`)
- Se reproduce cuando el jugador muere
- Sonido de "muerte" con frecuencia descendente
- Sonido de "game over" al final

### ✅ **Recolección de Powerup** (`PowerUp.cs`)
- Se reproduce cuando el jugador recoge un powerup
- Sonido ascendente que simula "powerup"

## 🚀 Configuración Rápida

### 1. Crear el Audio Manager del Juego

1. En tu escena del juego, crea un GameObject vacío llamado "GameAudioManager"
2. Agrega el componente `GameAudioManager` al GameObject
3. Configura los siguientes campos:
   - **Use Generated Sounds**: true (recomendado)
   - **SFX Volume**: 0.8 (recomendado)
   - **Shoot Frequency**: 800
   - **Death Frequency**: 200
   - **Powerup Frequency**: 1200

### 2. Configuración Automática (Opcional)

1. Agrega el componente `GameAudioSetup` a cualquier GameObject
2. Marca "Setup On Start" como true
3. El sistema se configurará automáticamente

## 🎛️ Configuración Avanzada

### Efectos de Sonido Generados

Los efectos se generan programáticamente con las siguientes características:

#### **Disparo**
- **Frecuencia**: 800 Hz (ajustable)
- **Duración**: 0.1 segundos
- **Envelope**: Muy rápido (efecto de "pistola")

#### **Muerte**
- **Frecuencia**: 200 Hz (ajustable)
- **Duración**: 0.2 segundos
- **Envelope**: Descendente (efecto de "muerte")

#### **Powerup**
- **Frecuencia**: 1200 Hz (ajustable)
- **Duración**: 0.1 segundos
- **Envelope**: Ascendente (efecto de "powerup")

#### **Game Over**
- **Duración**: 0.3 segundos
- **Secuencia**: Tonos descendentes
- **Envelope**: Con pausas

### Efectos de Sonido Personalizados

Si prefieres usar archivos de audio externos:

1. Crea o descarga efectos de sonido (.wav o .ogg)
2. Asigna los archivos en el `GameAudioManager`:
   - **Shoot Sound**
   - **Death Sound**
   - **Powerup Collect Sound**
   - **Enemy Death Sound**
   - **Game Over Sound**

## 🔧 Scripts Incluidos

### GameAudioManager.cs
- Maneja todos los efectos de sonido del juego
- Patrón Singleton para acceso global
- Generación automática de efectos
- Control de volúmenes

### GameAudioSetup.cs
- Facilita la configuración inicial
- Métodos de prueba para todos los efectos
- Configuración automática

### Modificaciones Realizadas:
- **PlayerShooting.cs**: Agregado sonido de disparo
- **PlayerController.cs**: Agregado sonido de muerte y game over
- **PowerUp.cs**: Agregado sonido de recolección

## 🎮 Uso en el Código

### Reproducir Efectos Manualmente
```csharp
// Reproducir sonido de disparo
GameAudioManager.Instance.PlayShootSound();

// Reproducir sonido de muerte
GameAudioManager.Instance.PlayDeathSound();

// Reproducir sonido de powerup
GameAudioManager.Instance.PlayPowerupCollectSound();

// Reproducir sonido de game over
GameAudioManager.Instance.PlayGameOverSound();
```

### Control de Volumen
```csharp
// Cambiar volumen de efectos
GameAudioManager.Instance.SetSFXVolume(0.5f);

// Obtener volumen actual
float currentVolume = GameAudioManager.Instance.GetSFXVolume();
```

### Efectos Avanzados
```csharp
// Disparo con variación de pitch
GameAudioManager.Instance.PlayShootSoundWithVariation(0.1f);

// Sonido con fade in
GameAudioManager.Instance.PlaySoundWithFadeIn(myAudioClip, 0.5f);
```

## 🧪 Pruebas

### Probar Todos los Efectos
```csharp
// En GameAudioSetup
gameAudioSetup.TestAllSounds();
```

### Probar Disparos Múltiples
```csharp
// En GameAudioSetup
gameAudioSetup.TestMultipleShots();
```

## 🎨 Personalización

### Modificar Frecuencias
```csharp
// En GameAudioManager
shootFrequency = 1000f;  // Disparo más agudo
deathFrequency = 150f;   // Muerte más grave
powerupFrequency = 1500f; // Powerup más agudo
```

### Crear Efectos Personalizados
```csharp
// En SimpleSoundEffects
AudioClip customShoot = CreateShootSound(1200f, 0.6f);
AudioClip customDeath = CreateDeathSound(300f, 0.8f);
```

### Agregar Nuevos Efectos
1. Crea un nuevo método en `SimpleSoundEffects`
2. Genera el AudioClip con la lógica deseada
3. Agrega el método correspondiente en `GameAudioManager`

## 🐛 Solución de Problemas

### No se escuchan los efectos
1. Verifica que `GameAudioManager` esté en la escena
2. Asegúrate de que "Use Generated Sounds" esté marcado
3. Comprueba que el SFX Volume no esté en 0

### Los efectos suenan mal
1. Ajusta las frecuencias en la configuración
2. Modifica la duración en `SimpleSoundEffects`
3. Considera usar archivos de audio externos

### Conflictos con el audio del menú
- El `GameAudioManager` y `MainMenuAudioManager` son independientes
- Pueden coexistir sin problemas
- Cada uno maneja su propio contexto

## 📝 Notas Importantes

- El sistema usa el patrón Singleton, por lo que solo debe haber una instancia del `GameAudioManager`
- Los efectos generados se crean en tiempo de ejecución
- El sistema es compatible con Unity 2020.3 y versiones posteriores
- Los efectos se optimizan para dispositivos móviles

## 🎯 Próximas Mejoras

- [ ] Efectos de sonido 3D para enemigos
- [ ] Sistema de reverb y echo
- [ ] Efectos de sonido ambientales
- [ ] Integración con Audio Mixer
- [ ] Sistema de equalización dinámica
- [ ] Efectos de sonido basados en eventos 