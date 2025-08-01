# 🎛️ Guía de Configuración - Sistema de Opciones

## 📋 Descripción
Sistema completo de opciones que se integra con el RadialMenu para controlar:
- **Volumen de Música** (0-100%)
- **Volumen de SFX** (0-100%)
- **Sensibilidad del Giroscopio** (0.1-5.0)
- **Vibración** (ON/OFF)

## 🏗️ Estructura de Archivos

### Scripts Principales:
- `OptionsManager.cs` - Maneja toda la lógica de opciones
- `OptionsRadialIntegration.cs` - Integración con RadialMenu

## 🎮 Configuración Paso a Paso

### 1. Crear el Panel de Opciones

#### **Crear el GameObject Principal:**
```
Options Panel
├── Canvas Group
├── Rect Transform
├── Image (fondo)
└── OptionsManager (script)
```

#### **Estructura del Panel:**
```
Options Panel
├── Background Image
├── Title Text ("OPTIONS")
├── Music Volume Section
│   ├── Label ("Music Volume")
│   ├── Slider (0-1)
│   └── Value Text ("70%")
├── SFX Volume Section
│   ├── Label ("SFX Volume")
│   ├── Slider (0-1)
│   └── Value Text ("80%")
├── Gyro Sensitivity Section
│   ├── Label ("Gyro Sensitivity")
│   ├── Slider (0.1-5.0)
│   └── Value Text ("1.5")
├── Vibration Section
│   ├── Label ("Vibration")
│   └── Toggle (ON/OFF)
├── Reset Button ("Reset to Defaults")
└── Close Button ("Close")
```

### 2. Configurar OptionsManager

#### **Asignar Referencias:**
- `Music Volume Slider` → Slider de música
- `SFX Volume Slider` → Slider de SFX
- `Gyro Sensitivity Slider` → Slider de sensibilidad
- `Vibration Toggle` → Toggle de vibración

#### **Asignar Textos:**
- `Music Volume Text` → TextMeshPro del valor de música
- `SFX Volume Text` → TextMeshPro del valor de SFX
- `Gyro Sensitivity Text` → TextMeshPro del valor de sensibilidad
- `Vibration Text` → TextMeshPro del estado de vibración

#### **Configurar Valores por Defecto:**
- `Default Music Volume`: 0.7 (70%)
- `Default SFX Volume`: 0.8 (80%)
- `Default Gyro Sensitivity`: 1.5
- `Default Vibration`: true

### 3. Integrar con RadialMenu

#### **Agregar Opción al RadialMenu:**
1. En el RadialMenu, agregar un nuevo `MenuItem`:
   - `Item Name`: "Options"
   - `Panel`: Asignar el Options Panel
   - `On Select`: Asignar `OptionsRadialIntegration.OnOptionsSelected()`

#### **Configurar OptionsRadialIntegration:**
- `Radial Menu` → Referencia al RadialMenu
- `Options Panel` → Referencia al panel de opciones
- `Options Manager` → Referencia al OptionsManager

### 4. Configurar Sliders

#### **Music Volume Slider:**
- `Min Value`: 0
- `Max Value`: 1
- `Value`: 0.7
- `Whole Numbers`: false

#### **SFX Volume Slider:**
- `Min Value`: 0
- `Max Value`: 1
- `Value`: 0.8
- `Whole Numbers`: false

#### **Gyro Sensitivity Slider:**
- `Min Value`: 0.1
- `Max Value`: 5.0
- `Value`: 1.5
- `Whole Numbers`: false

### 5. Configurar Toggle de Vibración

#### **Vibration Toggle:**
- `Is On`: true (por defecto)
- `Transition`: Color Tint
- `Navigation`: None

## 🔧 Funcionalidades

### **Volumen de Música:**
- Controla el volumen del `SectorMusicManager` (juego)
- Controla el volumen del `MainMenuAudioManager` (menú principal)
- Se guarda automáticamente en PlayerPrefs
- Se aplica en tiempo real

### **Volumen de SFX:**
- Controla el volumen del `GameAudioManager` (juego)
- Controla el volumen del `MainMenuAudioManager` (menú principal)
- Se guarda automáticamente en PlayerPrefs
- Se aplica en tiempo real

### **Sensibilidad del Giroscopio:**
- Controla la sensibilidad del `PlayerController`
- Rango: 0.1 (muy lento) a 5.0 (muy rápido)
- Se aplica inmediatamente al jugador

### **Vibración:**
- Habilita/deshabilita la vibración del dispositivo
- Se aplica cuando se llama `OptionsManager.PlayVibration()`
- Solo funciona en dispositivos que soporten vibración

## 🎨 Estilo Visual

### **Colores Sugeridos:**
- **Fondo del Panel**: Color oscuro semi-transparente
- **Textos**: Blanco o color claro
- **Sliders**: Color principal del juego
- **Botones**: Color de acento

### **Layout:**
- **Vertical Layout Group** para organizar elementos
- **Content Size Fitter** para ajustar tamaño automáticamente
- **Padding** de 20-30 píxeles entre elementos

## 🧪 Testing

### **Context Menu Methods:**
- `Test Vibration` - Prueba la vibración
- `Reset All Settings` - Resetea a valores por defecto
- `Print Current Settings` - Muestra configuración actual en consola

### **Métodos de Testing:**
```csharp
// Probar vibración
OptionsManager.Instance.PlayVibration(0.2f);

// Obtener configuración actual
float musicVol = OptionsManager.Instance.GetMusicVolume();
float sfxVol = OptionsManager.Instance.GetSFXVolume();
float gyroSens = OptionsManager.Instance.GetGyroSensitivity();
bool vibration = OptionsManager.Instance.GetVibration();
```

## 🔗 Integración con Otros Sistemas

### **PlayerController:**
El `PlayerController` debe leer la sensibilidad del giroscopio:
```csharp
// En PlayerController.Start()
if (OptionsManager.Instance != null)
{
    gyroSensitivity = OptionsManager.Instance.GetGyroSensitivity();
}
```

### **GameAudioManager:**
El `GameAudioManager` debe aplicar el volumen de SFX:
```csharp
// En GameAudioManager.Start()
if (OptionsManager.Instance != null)
{
    sfxVolume = OptionsManager.Instance.GetSFXVolume();
}
```

### **SectorMusicManager:**
El `SectorMusicManager` debe aplicar el volumen de música:
```csharp
// En SectorMusicManager.Start()
if (OptionsManager.Instance != null)
{
    SetMusicVolume(OptionsManager.Instance.GetMusicVolume());
}
```

### **MainMenuAudioManager:**
El `MainMenuAudioManager` debe aplicar el volumen de música y SFX:
```csharp
// En MainMenuAudioManager.Start()
if (OptionsManager.Instance != null)
{
    SetMusicVolume(OptionsManager.Instance.GetMusicVolume());
    SetSFXVolume(OptionsManager.Instance.GetSFXVolume());
}
```

## 🚀 Uso en el Juego

### **Abrir Opciones:**
1. Navegar al RadialMenu
2. Seleccionar "Options"
3. El panel se abre con animación

### **Cambiar Configuración:**
1. Mover sliders para ajustar volúmenes y sensibilidad
2. Toggle para activar/desactivar vibración
3. Los cambios se aplican inmediatamente

### **Cerrar Opciones:**
1. Presionar "Close" o "Reset to Defaults"
2. El panel se cierra con animación
3. La configuración se guarda automáticamente

## 💾 Persistencia de Datos

### **PlayerPrefs Keys:**
- `"MusicVolume"` - Volumen de música (float)
- `"SFXVolume"` - Volumen de SFX (float)
- `"GyroSensitivity"` - Sensibilidad del giroscopio (float)
- `"Vibration"` - Estado de vibración (int: 0/1)

### **Carga Automática:**
- Los valores se cargan al iniciar el juego
- Se aplican a todos los sistemas correspondientes
- Si no hay datos guardados, se usan valores por defecto

## 🎯 Características Avanzadas

### **Vibración Inteligente:**
- Solo vibra si el dispositivo lo soporta
- Se puede habilitar/deshabilitar desde opciones
- Duración configurable

### **Aplicación en Tiempo Real:**
- Los cambios se aplican inmediatamente
- No requiere reiniciar el juego
- Feedback visual y auditivo instantáneo

### **Valores por Defecto:**
- Configurables desde el inspector
- Se pueden resetear fácilmente
- Balanceados para una buena experiencia de juego 