# Solución de Problemas de Audio

Este documento explica cómo solucionar los problemas de audio entre escenas y la gestión de música por sectores.

## 🐛 **Problemas Solucionados:**

### ✅ **1. Música no inicia en sector 1:**
- **Problema:** El sistema iniciaba con sector 0
- **Solución:** Cambiado para iniciar con sector 1 (nivel inicial)

### ✅ **2. Música de muerte sigue al reiniciar:**
- **Problema:** La música no se detenía correctamente
- **Solución:** Detención inmediata en `PlayerController.Die()`

### ✅ **3. Música del juego sigue en el menú:**
- **Problema:** Los managers persistían entre escenas
- **Solución:** Removido `DontDestroyOnLoad` y agregado `AudioSceneManager`

### ✅ **4. MainMenuAudioManager no se detiene:**
- **Problema:** Persistía entre escenas
- **Solución:** Removido `DontDestroyOnLoad`

## 🚀 **Configuración Requerida:**

### **1. Agregar AudioSceneManager:**

```
1. Crear GameObject "AudioSceneManager"
2. Agregar componente AudioSceneManager.cs
3. Configurar:
   - Destroy Game Audio On Menu: true
   - Destroy Menu Audio On Game: true
   - Destroy All Audio On Scene Change: false
```

### **2. Configurar SectorMusicManager:**

```
1. Crear GameObject "SectorMusicManager"
2. Agregar componente SectorMusicManager.cs
3. Configurar música por sector:
   - Sector 0: (no usado - inicia en sector 1)
   - Sector 1: A New Planet.wav (nivel inicial)
   - Sector 2: Among Stars.wav
   - Sector 3: Cold Space.wav
   - Sector 4: Glitch Bot.wav
   - Sector 5: Little Engine.wav
```

### **3. Configurar GameAudioManager:**

```
1. Crear GameObject "GameAudioManager"
2. Agregar componente GameAudioManager.cs
3. Configurar efectos de sonido
```

### **4. Configurar MainMenuAudioManager:**

```
1. Crear GameObject "MainMenuAudioManager"
2. Agregar componente MainMenuAudioManager.cs
3. Configurar música de fondo del menú
```

## 🎮 **Flujo de Audio Corregido:**

### **Al iniciar el juego:**
```
1. SectorManager inicia con sector 1
2. SectorMusicManager reproduce música del sector 1
3. GameAudioManager maneja efectos de sonido
```

### **Al cambiar de sector:**
```
1. PlayerShooting dispara evento
2. SectorManager cambia color y texto
3. SectorMusicManager cambia música
4. Todo sincronizado
```

### **Al morir el jugador:**
```
1. Se reproduce sonido de muerte
2. Se detiene música del juego inmediatamente
3. Se reproduce sonido de game over
4. Se muestra panel de game over
```

### **Al cambiar de escena:**
```
1. AudioSceneManager detecta cambio de escena
2. Destruye audio de la escena anterior
3. Crea audio para la nueva escena
4. No hay superposición de audio
```

## 🔧 **Cambios Realizados:**

### **SectorMusicManager.cs:**
```csharp
// Cambio 1: Iniciar con sector 1
private void Start()
{
    PlaySectorMusic(1); // Antes era 0
}

// Cambio 2: Detención inmediata
public void StopMusic()
{
    if (musicSource != null)
    {
        musicSource.Stop();
        musicSource.volume = 0f;
    }
    isTransitioning = false;
    currentMusic = null;
}
```

### **SectorManager.cs:**
```csharp
// Cambio: Iniciar con sector 1
void Start()
{
    currentSector = 1; // Antes era 0
    UpdateSkyboxTint(currentSector);
    UpdateSectorText(currentSector);
}
```

### **PlayerController.cs:**
```csharp
// Cambio: Detener música antes de game over
IEnumerator Die()
{
    // ... código existente ...
    
    // Detener música del juego antes de mostrar game over
    if (GameAudioManager.Instance != null)
    {
        GameAudioManager.Instance.StopGameMusic();
    }
    
    // Mostrar panel de Game Over
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);
}
```

### **AudioSceneManager.cs (NUEVO):**
```csharp
// Maneja automáticamente la limpieza de audio entre escenas
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    string sceneName = scene.name.ToLower();
    
    if (sceneName.Contains("menu") || sceneName.Contains("main"))
    {
        HandleMenuScene(); // Destruye audio del juego
    }
    else if (sceneName.Contains("game") || sceneName.Contains("sample"))
    {
        HandleGameScene(); // Destruye audio del menú
    }
}
```

## 🎵 **Configuración de Música por Sectores:**

### **Sector 1 (Nivel Inicial):**
- **Música:** A New Planet.wav
- **Color:** Azul
- **Texto:** "001"

### **Sector 2:**
- **Música:** Among Stars.wav
- **Color:** Verde
- **Texto:** "002"

### **Sector 3:**
- **Música:** Cold Space.wav
- **Color:** Amarillo
- **Texto:** "003"

### **Sector 4:**
- **Música:** Glitch Bot.wav
- **Color:** Rojo
- **Texto:** "004"

### **Sector 5:**
- **Música:** Little Engine.wav
- **Color:** Púrpura
- **Texto:** "005"

## 🧪 **Pruebas:**

### **Probar inicio del juego:**
1. Iniciar juego desde el menú
2. Verificar que la música del sector 1 se reproduzca
3. Verificar que el color del skybox sea el correcto
4. Verificar que el texto muestre "001"

### **Probar cambio de sector:**
1. Recolectar powerups hasta subir de sector
2. Verificar que la música cambie
3. Verificar que el color cambie
4. Verificar que el texto se actualice

### **Probar muerte del jugador:**
1. Chocar con un enemigo
2. Verificar que se reproduzca sonido de muerte
3. Verificar que la música se detenga
4. Verificar que se reproduzca sonido de game over

### **Probar cambio de escena:**
1. Ir del menú al juego
2. Verificar que la música del menú se detenga
3. Verificar que la música del juego inicie
4. Volver al menú
5. Verificar que la música del juego se detenga
6. Verificar que la música del menú inicie

## 🐛 **Solución de Problemas:**

### **La música no inicia:**
1. Verificar que `AudioSceneManager` esté en la escena
2. Verificar que `SectorMusicManager` esté configurado
3. Verificar que los clips de música estén asignados

### **La música no se detiene:**
1. Verificar que `AudioSceneManager` esté funcionando
2. Verificar que los managers no tengan `DontDestroyOnLoad`
3. Verificar que `StopMusic()` se llame correctamente

### **Hay superposición de audio:**
1. Verificar que `AudioSceneManager` destruya el audio anterior
2. Verificar que no haya múltiples managers
3. Verificar que los eventos de escena se disparen correctamente

## 📝 **Notas Importantes:**

- **AudioSceneManager** debe estar en cada escena o en un GameObject que persista
- **Los managers de audio** ya no usan `DontDestroyOnLoad`
- **El sistema inicia en sector 1**, no en sector 0
- **La música se detiene inmediatamente** al morir o cambiar de escena
- **No hay superposición** entre audio de diferentes escenas

¡Ahora el sistema de audio debería funcionar perfectamente! 🎵 