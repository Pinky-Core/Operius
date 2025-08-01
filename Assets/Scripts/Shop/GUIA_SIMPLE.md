# 🛒 GUÍA SIMPLE - TIENDA CON IMÁGENES, BOTONES Y TEXTOS

## 📋 Descripción
Versión simplificada de la tienda con UI básica: imágenes, botones y textos.

## 🚀 Configuración en 3 Pasos

### **PASO 1: Crear Materiales**
1. **Crear GameObject** llamado "MaterialCreator"
2. **Agregar componente** `ShipMaterialCreator`
3. **Click derecho** → **"Crear Materiales de Nave"**
4. **Verificar** que se crearon en `Assets/Materials/ShipMaterials/`

### **PASO 2: Crear UI de la Tienda**

#### **Estructura del Canvas:**
```
Canvas
├── Shop Panel (inicialmente oculto)
│   ├── Background (Image)
│   ├── Title: "TIENDA DE NAVES" (TextMeshPro)
│   ├── Points Text: "Puntos: 0/100" (TextMeshPro)
│   ├── Coins Text: "Monedas: 0 🪙" (TextMeshPro)
│   ├── Ship Buttons Container
│   │   ├── Ship Button 1 (Button + Image + TextMeshPro)
│   │   ├── Ship Button 2 (Button + Image + TextMeshPro)
│   │   ├── Ship Button 3 (Button + Image + TextMeshPro)
│   │   └── Ship Button 4 (Button + Image + TextMeshPro)
│   └── Close Button: "X" (Button)
```

#### **Configurar cada Botón de Nave:**
```
Ship Button X
├── Button (componente) - Para seleccionar/comprar nave
├── Image (para color de fondo)
├── Ship Image (imagen de la nave)
├── Ship Name (TextMeshPro: "Básica", "Veloz", etc.)
├── Ship Price (TextMeshPro: "GRATIS", "10 🪙", etc.)
└── Button Text (TextMeshPro: "SELECCIONAR", "COMPRAR", etc.)
```

### **PASO 3: Configurar SimpleShopUI**

1. **Crear GameObject** llamado "SimpleShopUI"
2. **Agregar componente** `SimpleShopUI`
3. **Configurar referencias**:

#### **Panel Principal:**
- `Shop Panel`: Panel principal de la tienda

#### **Textos:**
- `Points Text`: TextMeshPro para puntos
- `Coins Text`: TextMeshPro para monedas
- `Title Text`: TextMeshPro del título

#### **Botones de Nave (4 arrays):**
- `Ship Buttons`: Array de 4 botones
- `Ship Images`: Array de 4 imágenes de nave
- `Ship Names`: Array de 4 textos de nombre
- `Ship Prices`: Array de 4 textos de precio
- `Button Texts`: Array de 4 textos de botones

#### **Botón Cerrar:**
- `Close Button`: Botón para cerrar

#### **Materiales:**
- `Ship Materials`: Array de materiales (los que creaste)

#### **Colores:**
- `Unlocked Color`: Verde (desbloqueado)
- `Locked Color`: Rojo (bloqueado)
- `Selected Color`: Azul (seleccionado)

## 🎮 Integración con Menú Radial

1. **En el RadialMenu**, agregar opción "TIENDA"
2. **En "On Select"**, arrastrar SimpleShopUI
3. **Seleccionar método** `OpenShop()`

## 🧪 Testing

### **Teclas de Debug:**
- **Click derecho** en SimpleShopUI → **"Agregar 10 Monedas"**
- **Click derecho** en SimpleShopUI → **"Resetear Datos"**

### **Funcionalidad:**
- **Un botón por nave**: Maneja automáticamente comprar o seleccionar
- **Textos automáticos**: 
  - "SELECCIONAR" (nave básica o desbloqueada)
  - "SELECCIONADA" (nave actual)
  - "COMPRAR (X 🪙)" (nave bloqueada)
- **Colores automáticos**: Verde (disponible), Rojo (bloqueado), Azul (seleccionado)
- **Lógica inteligente**: Si está desbloqueada → selecciona, si está bloqueada → compra

## 💰 Sistema de Precios

- **Nave Básica**: GRATIS
- **Nave Veloz**: 10 monedas
- **Nave Defensiva**: 15 monedas
- **Nave Elite**: 30 monedas

## ✅ Checklist

- [ ] Materiales creados
- [ ] UI de tienda creada
- [ ] SimpleShopUI configurado
- [ ] Referencias asignadas
- [ ] Menú radial configurado
- [ ] Testing funcionando

## 🎯 Resultado

Una tienda simple con:
- ✅ 4 botones de nave con imágenes
- ✅ Textos de nombre y precio
- ✅ Colores según estado (verde/rojo/azul)
- ✅ Sistema de compra automático
- ✅ Aplicación de materiales
- ✅ Persistencia de datos

---

**¡Listo! Tienda simple y funcional 🚀** 