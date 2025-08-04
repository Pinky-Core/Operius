# 🎮 **Guía de Configuración - Controles Automáticos**

## 📋 **Descripción**
El `PlayerController` ahora detecta automáticamente si el dispositivo tiene giroscopio y usa controles de botones como fallback. Esto mejora la compatibilidad con diferentes dispositivos móviles.

## 🎯 **Funcionamiento Automático**

### **Detección Automática:**
- ✅ **Con Giroscopio**: Usa giroscopio para control
- ✅ **Sin Giroscopio**: Usa controles de botones automáticamente
- ✅ **Controles Táctiles**: Funciona en dispositivos móviles
- ✅ **Controles de Teclado**: Funciona en PC/Editor

## 🛠️ **Configuración en el Inspector**

### **Configuración de Giroscopio:**
```
Gyro Sensitivity: 1.5 (ajustable)
Gyro Threshold: 0.05 (ajustable)
Use Gyro: true (se auto-ajusta)
Invert Gyro: false (opcional)
```

### **Configuración de Botones:**
```
Enable Button Controls: ✓ true
Button Sensitivity: 1.0 (ajustable)
Left Button: A (teclado)
Right Button: D (teclado)
Left Touch Button: LeftArrow (teclado)
Right Touch Button: RightArrow (teclado)
```

## 🎮 **Tipos de Controles**

### **1. Giroscopio (Automático):**
- **Dispositivos**: iPhone, Android con giroscopio
- **Control**: Inclinar el dispositivo
- **Configuración**: Se ajusta automáticamente

### **2. Controles Táctiles (Automático):**
- **Dispositivos**: Cualquier dispositivo táctil
- **Control**: Tocar lado izquierdo/derecho de la pantalla
- **Funcionamiento**: 
  - Lado izquierdo = Mover izquierda
  - Lado derecho = Mover derecha

### **3. Controles de Teclado (PC/Editor):**
- **Plataforma**: PC, Unity Editor
- **Teclas**: A/D o Flechas izquierda/derecha
- **Configuración**: Personalizable en el inspector

## 🔧 **Métodos de Context Menu**

### **Forzar Uso de Giroscopio:**
```
PlayerController → Forzar Uso de Giroscopio
```
- Fuerza el uso del giroscopio para testing

### **Forzar Uso de Botones:**
```
PlayerController → Forzar Uso de Botones
```
- Fuerza el uso de controles de botones para testing

### **Mostrar Info de Controles:**
```
PlayerController → Mostrar Info de Controles
```
- Muestra información detallada del estado de los controles

## 📱 **Controles Táctiles**

### **Funcionamiento:**
- **Pantalla dividida en dos mitades**
- **Lado izquierdo**: Mover nave hacia la izquierda
- **Lado derecho**: Mover nave hacia la derecha
- **Múltiples toques**: Funciona con varios dedos

### **Configuración:**
- **Sensibilidad**: Ajustable con `buttonSensitivity`
- **Área de toque**: Automática (mitad izquierda/derecha)
- **Respuesta**: Inmediata al tocar

## 🎯 **Testing y Debug**

### **En Unity Editor:**
1. **Teclado**: Usar A/D o flechas
2. **Forzar giroscopio**: Context menu → "Forzar Uso de Giroscopio"
3. **Forzar botones**: Context menu → "Forzar Uso de Botones"

### **En Dispositivo Móvil:**
1. **Con giroscopio**: Inclinar dispositivo
2. **Sin giroscopio**: Tocar lados de la pantalla
3. **Verificar logs**: "Giroscopio detectado" o "usando controles de botones"

### **Logs de Debug:**
```
PlayerController: Giroscopio detectado y habilitado
PlayerController: No se detectó giroscopio - usando controles de botones
PlayerController: Uso de giroscopio forzado
PlayerController: Uso de botones forzado
```

## ⚙️ **Configuración Avanzada**

### **Sensibilidad del Giroscopio:**
- **Valores típicos**: 1.0 - 3.0
- **Más alto**: Más sensible
- **Más bajo**: Menos sensible

### **Sensibilidad de Botones:**
- **Valores típicos**: 0.5 - 2.0
- **Más alto**: Movimiento más rápido
- **Más bajo**: Movimiento más suave

### **Threshold del Giroscopio:**
- **Valores típicos**: 0.01 - 0.1
- **Más alto**: Menos sensible a movimientos pequeños
- **Más bajo**: Más sensible a movimientos pequeños

## 🔍 **Información del Sistema**

El método `ShowControlInfo()` muestra:
```
=== INFORMACIÓN DE CONTROLES ===
Giroscopio disponible: true/false
Giroscopio detectado: true/false
Usando giroscopio: true/false
Controles de botones habilitados: true/false
Sensibilidad giroscopio: 1.5
Sensibilidad botones: 1.0
Botón izquierdo: A
Botón derecho: D
```

## 🚀 **Compatibilidad**

### **Dispositivos Soportados:**
- ✅ **iPhone**: Giroscopio + táctil
- ✅ **Android con giroscopio**: Giroscopio + táctil
- ✅ **Android sin giroscopio**: Solo táctil
- ✅ **PC/Editor**: Teclado
- ✅ **Tablets**: Táctil + giroscopio (si disponible)

### **Plataformas:**
- ✅ **iOS**: Completo
- ✅ **Android**: Completo
- ✅ **PC**: Completo
- ✅ **Editor**: Completo

## 🎮 **Testing Recomendado**

### **1. En Editor:**
- Probar teclas A/D
- Probar flechas
- Forzar diferentes modos

### **2. En Dispositivo con Giroscopio:**
- Verificar que usa giroscopio
- Probar inclinación
- Verificar sensibilidad

### **3. En Dispositivo sin Giroscopio:**
- Verificar que usa táctil
- Probar toques en pantalla
- Verificar respuesta

### **4. Transiciones:**
- Cambiar entre modos
- Verificar que funciona correctamente
- Probar sensibilidad

## 📝 **Notas Importantes**

- **Detección automática**: Se ejecuta al iniciar
- **Fallback inteligente**: Siempre hay un método de control disponible
- **Configuración persistente**: Se mantiene durante la sesión
- **Testing fácil**: Métodos de contexto para forzar modos
- **Logs detallados**: Información completa en consola

¡Con esta configuración, el juego funcionará perfectamente en cualquier dispositivo! 🎮✨ 