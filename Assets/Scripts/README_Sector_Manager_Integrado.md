# Sistema de Gestión de Sectores Integrado

Este sistema combina la gestión visual de sectores (colores del skybox, texto) con el sistema de música por sectores, creando una experiencia completa y sincronizada.

## 🎯 **Características del Sistema Integrado**

### ✅ **Gestión Visual:**
- **Colores del skybox** que cambian por sector
- **Texto del sector** que se actualiza automáticamente
- **Transiciones suaves** entre colores

### ✅ **Gestión de Audio:**
- **Música específica** para cada sector
- **Sonido de cambio de sector** opcional
- **Transiciones sincronizadas** entre audio y visual

### ✅ **Sistema Unificado:**
- **Un solo script** maneja todo el sistema
- **Eventos** para otros scripts
- **Configuración modular** (puedes deshabilitar partes)

## 📁 **Scripts del Sistema**

### **Scripts Principales:**
- `EnhancedSectorManager.cs` - Sistema integrado completo
- `SectorMusicManager.cs` - Núcleo del sistema de música
- `GameAudioManager.cs` - Audio manager del juego

### **Scripts Modificados:**
- `SectorManager.cs` - Tu script original (actualizado)
- `PlayerShooting.cs` - Solo dispara eventos, no maneja música

## 🚀 **Configuración Rápida**

### **Opción 1: Usar EnhancedSectorManager (Recomendado)**

1. **Reemplazar tu SectorManager actual:**
   ```
   Eliminar SectorManager.cs del GameObject
   Agregar EnhancedSectorManager.cs
   ```

2. **Configurar el EnhancedSectorManager:**
   ```
   Configuración Visual:
   - Sector Colors: [Color1, Color2, Color3, Color4, Color5]
   - Sector Text: Referencia al TextMeshProUGUI
   - Transition Duration: 2f
   
   Configuración de Audio:
   - Enable Sector Music: true
   - Enable Visual Transitions: true
   - Enable Sector Text: true
   
   Configuración de Efectos:
   - Sector Change Sound: (opcional)
   - Play Sector Change Sound: true
   ```

### **Opción 2: Mantener tu SectorManager + Integración**

1. **Tu SectorManager ya está actualizado** con la integración de música
2. **Configurar el SectorMusicManager** por separado
3. **Ambos sistemas funcionarán juntos**

## 🎮 **Funcionamiento del Sistema**

### **Flujo de Cambio de Sector:**

```csharp
// 1. Jugador recolecta powerup
PlayerShooting.CollectPowerup()

// 2. Se dispara el evento
PlayerShooting.SectorLevelUpEvent?.Invoke(sectorLevel)

// 3. SectorManager recibe el evento
SectorManager.OnSectorLevelUp(newSector)

// 4. Se ejecutan todas las transiciones:
//    - Cambio de color del skybox
//    - Actualización del texto
//    - Cambio de música
//    - Sonido de cambio de sector (opcional)
```

### **Sincronización de Transiciones:**

```csharp
// Transición visual (inmediata)
colorTransitionCoroutine = StartCoroutine(TransitionSkyboxTint(sectorColors[newSector]));

// Transición de música (con delay de 0.2s)
musicTransitionCoroutine = StartCoroutine(TransitionMusic(newSector));
```

## 🎛️ **Configuración Detallada**

### **Configuración Visual:**

#### **Colores por Sector:**
```csharp
sectorColors[0] = Color.blue;      // Sector 1 - Azul
sectorColors[1] = Color.green;     // Sector 2 - Verde
sectorColors[2] = Color.yellow;    // Sector 3 - Amarillo
sectorColors[3] = Color.red;       // Sector 4 - Rojo
sectorColors[4] = Color.purple;    // Sector 5 - Púrpura
```

#### **Texto del Sector:**
```csharp
// Formato: "001", "002", "003", etc.
sectorText.text = (sectorIndex + 1).ToString("D3");
```

### **Configuración de Audio:**

#### **Música por Sector:**
```csharp
// En SectorMusicManager
Sector 0: "A New Planet.wav" - Música suave
Sector 1: "Among Stars.wav" - Música épica
Sector 2: "Cold Space.wav" - Música fría
Sector 3: "Glitch Bot.wav" - Música glitch
Sector 4: "Little Engine.wav" - Música intensa
```

#### **Sonido de Cambio de Sector:**
```csharp
// Opcional - se reproduce al cambiar de sector
sectorChangeSound = "sector_change.wav";
```

## 🎮 **Uso en el Código**

### **Control Básico:**
```csharp
// Obtener sector actual
int currentSector = sectorManager.GetCurrentSector();

// Cambiar manualmente a un sector
sectorManager.ChangeToSector(2);

// Obtener información del sector
Color sectorColor = sectorManager.GetCurrentSectorColor();
string sectorName = sectorManager.GetCurrentSectorName();
```

### **Control de Funcionalidades:**
```csharp
// Habilitar/deshabilitar partes del sistema
sectorManager.SetVisualTransitions(false);
sectorManager.SetSectorMusic(true);
sectorManager.SetSectorText(true);
sectorManager.SetSectorChangeSound(false);
```

### **Eventos del Sistema:**
```csharp
// Suscribirse a eventos
sectorManager.OnSectorChanged += (sector) => {
    Debug.Log($"Sector cambiado a: {sector}");
};

sectorManager.OnSectorTransitionStarted += (sector) => {
    Debug.Log($"Iniciando transición al sector: {sector}");
};

sectorManager.OnSectorTransitionCompleted += (sector) => {
    Debug.Log($"Transición completada al sector: {sector}");
};
```

## 🧪 **Pruebas y Debugging**

### **Pruebas en el Inspector:**
```csharp
// Click derecho en EnhancedSectorManager → Context Menu
[ContextMenu("Test Sector Change")]     // Prueba un sector
[ContextMenu("Test All Sectors")]       // Prueba todos los sectores
```

### **Pruebas en Código:**
```csharp
// Probar cambio de sector
sectorManager.TestSectorChange();

// Probar todos los sectores en secuencia
sectorManager.TestAllSectors();
```

### **Debugging:**
```csharp
// Verificar estado del sistema
Debug.Log($"Sector actual: {sectorManager.GetCurrentSector()}");
Debug.Log($"Color actual: {sectorManager.GetCurrentSectorColor()}");
Debug.Log($"Nombre del sector: {sectorManager.GetCurrentSectorName()}");
```

## 🎵 **Configuración de Música por Sectores**

### **Configurar SectorMusicManager:**

1. **Crear GameObject:**
   ```
   GameObject "SectorMusicManager" → Agregar SectorMusicManager
   ```

2. **Configurar música por sector:**
   ```
   Sector Music Clips:
   - Element 0: A New Planet.wav
   - Element 1: Among Stars.wav
   - Element 2: Cold Space.wav
   - Element 3: Glitch Bot.wav
   - Element 4: Little Engine.wav
   ```

3. **Configurar propiedades:**
   ```
   Sector Volumes: [0.6, 0.7, 0.7, 0.8, 0.8]
   Sector Loops: [true, true, true, true, true]
   Use Crossfade: true
   Crossfade Duration: 2f
   ```

## 🔧 **Ventajas del Sistema Integrado**

### **Experiencia de Usuario:**
- ✅ **Transiciones sincronizadas** entre audio y visual
- ✅ **Experiencia cohesiva** al cambiar de sector
- ✅ **Feedback inmediato** con texto y colores
- ✅ **Inmersión total** con música temática

### **Desarrollo:**
- ✅ **Un solo punto de control** para todo el sistema
- ✅ **Configuración modular** (puedes deshabilitar partes)
- ✅ **Eventos para extensibilidad**
- ✅ **Debugging simplificado**

### **Rendimiento:**
- ✅ **Transiciones optimizadas**
- ✅ **Gestión eficiente de recursos**
- ✅ **Sincronización precisa**
- ✅ **Prevención de conflictos**

## 🐛 **Solución de Problemas**

### **Los colores no cambian:**
1. Verificar que `sectorColors` esté configurado
2. Comprobar que el skybox tenga la propiedad `_Tint`
3. Verificar que `enableVisualTransitions` esté habilitado

### **La música no cambia:**
1. Verificar que `SectorMusicManager` esté configurado
2. Comprobar que `enableSectorMusic` esté habilitado
3. Verificar que los clips de música estén asignados

### **El texto no se actualiza:**
1. Verificar que `sectorText` esté asignado
2. Comprobar que `enableSectorText` esté habilitado
3. Verificar que el TextMeshProUGUI esté configurado

### **Las transiciones no están sincronizadas:**
1. Ajustar el delay en `TransitionMusic()` (actualmente 0.2s)
2. Verificar que `transitionDuration` sea apropiado
3. Comprobar que no haya conflictos entre corrutinas

## 📝 **Notas Importantes**

- **EnhancedSectorManager** es una versión mejorada que incluye todo
- **SectorManager** original sigue funcionando con la integración de música
- **Los eventos** permiten que otros scripts reaccionen a los cambios
- **La configuración modular** permite deshabilitar partes específicas
- **Las transiciones** están sincronizadas para una experiencia fluida

## 🎯 **Próximas Mejoras**

- [ ] Efectos de partículas por sector
- [ ] Animaciones de UI por sector
- [ ] Dificultad dinámica por sector
- [ ] Logros específicos por sector
- [ ] Estadísticas por sector
- [ ] Guardado de progreso por sector

¡Ahora tienes un sistema de gestión de sectores completamente integrado y profesional! 🎵🎨 