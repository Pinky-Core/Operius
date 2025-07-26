using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class RadialMenu : MonoBehaviour
{
    [System.Serializable]
    public class MenuItem
    {
        public RectTransform rectTransform;
        public string itemName;
        public UnityEvent onSelect;
        public Image backgroundImage; // Para cambiar color cuando está seleccionado
        public Color normalColor = Color.white;
        public Color selectedColor = Color.yellow;
        public Button button; // Botón para presionar
    }

    public List<MenuItem> menuItems = new List<MenuItem>();
    public float radius = 300f;
    public float rotateSpeed = 5f;
    
    [Header("Giro por pasos")]
    public float snapSpeed = 10f; // Velocidad de movimiento a la posición
    public float inputThreshold = 10f; // Sensibilidad del input (bajado de 50 a 10)

    private float angleStep;
    private int currentSelection = 0; // Opción actual seleccionada
    private float targetRotation = 0f;
    private bool isMoving = false;

    void Start()
    {
        // Calcular cuántos grados hay entre cada elemento
        angleStep = 360f / menuItems.Count;
        
        // Posicionar los elementos en círculo
        PositionMenuItems();
        
        // Configurar botones
        SetupButtons();
        
        // Asegurar que los elementos estén derechos desde el inicio
        KeepElementsUpright();
        
        // Seleccionar la primera opción
        UpdateSelection(0);
        
        Debug.Log("RadialMenu iniciado con " + menuItems.Count + " elementos");
    }

    void PositionMenuItems()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            // Calcular la posición en el círculo
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            
            // Mover el elemento a esa posición
            menuItems[i].rectTransform.anchoredPosition = pos;
            
            // Asegurar que el elemento esté derecho
            menuItems[i].rectTransform.rotation = Quaternion.identity;
        }
    }

    // Método para girar el menú por pasos
    public void RotateMenu(float delta)
    {
        Debug.Log("RotateMenu llamado con delta: " + delta);
        
        // Solo mover si el delta es suficientemente grande
        if (Mathf.Abs(delta) > inputThreshold)
        {
            Debug.Log("Delta supera el threshold, cambiando opción");
            
            // Determinar dirección (1 = siguiente, -1 = anterior)
            int direction = delta > 0 ? 1 : -1;
            
            // Calcular nueva selección
            int newSelection = (currentSelection + direction + menuItems.Count) % menuItems.Count;
            
            // Mover a la nueva posición
            MoveToSelection(newSelection);
        }
        else
        {
            Debug.Log("Delta no supera el threshold: " + inputThreshold);
        }
    }

    void MoveToSelection(int selection)
    {
        Debug.Log("MoveToSelection: " + selection);
        
        // Calcular la rotación necesaria para que la opción esté en la posición fija (derecha)
        float targetAngle = selection * angleStep;
        targetRotation = -targetAngle; // Sin offset, la posición fija está a 0 grados
        
        Debug.Log("Target Angle: " + targetAngle + ", Target Rotation: " + targetRotation);
        
        isMoving = true;
        
        Debug.Log("Moviendo a: " + menuItems[selection].itemName);
    }

    void Update()
    {
        // Mover suavemente hacia la posición objetivo
        if (isMoving)
        {
            float currentRotation = transform.eulerAngles.z;
            float angleDifference = Mathf.DeltaAngle(currentRotation, targetRotation);
            
            if (Mathf.Abs(angleDifference) > 1f)
            {
                // Mover hacia la posición objetivo
                float newRotation = Mathf.MoveTowardsAngle(currentRotation, targetRotation, snapSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
            }
            else
            {
                // Llegó a la posición exacta
                transform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
                isMoving = false;
            }
        }
        
        // Mantener los elementos siempre derechos
        KeepElementsUpright();
        
        // Actualizar la selección basada en qué opción está en la posición fija
        UpdateSelectionBasedOnPosition();
    }
    
    void UpdateSelectionBasedOnPosition()
    {
        // Calcular qué opción está en la posición fija (derecha)
        float menuRotation = transform.eulerAngles.z;
        float normalizedRotation = (360f - menuRotation) % 360f;
        
        int newSelection = Mathf.RoundToInt(normalizedRotation / angleStep) % menuItems.Count;
        newSelection = (newSelection + menuItems.Count) % menuItems.Count;
        
        // Solo actualizar si cambió la selección
        if (newSelection != currentSelection)
        {
            UpdateSelection(newSelection);
        }
    }
    
    void KeepElementsUpright()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i].rectTransform != null)
            {
                // Mantener los elementos siempre en su orientación original (0 grados)
                menuItems[i].rectTransform.rotation = Quaternion.identity;
            }
        }
    }

    void SetupButtons()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            int index = i; // Capturar el índice para el lambda
            if (menuItems[i].button != null)
            {
                menuItems[i].button.onClick.RemoveAllListeners();
                menuItems[i].button.onClick.AddListener(() => SelectItem(index));
            }
        }
    }

    void SelectItem(int index)
    {
        if (index >= 0 && index < menuItems.Count)
        {
            Debug.Log("Seleccionado: " + menuItems[index].itemName);
            menuItems[index].onSelect?.Invoke();
        }
    }

    void UpdateSelection(int newSelection)
    {
        // Deshabilitar y restaurar color de TODOS los elementos primero
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i].backgroundImage != null)
            {
                menuItems[i].backgroundImage.color = menuItems[i].normalColor;
            }
            if (menuItems[i].button != null)
            {
                menuItems[i].button.interactable = true;
            }
        }

        currentSelection = newSelection;

        // Solo habilitar y cambiar color del elemento seleccionado
        if (currentSelection >= 0 && currentSelection < menuItems.Count)
        {
            if (menuItems[currentSelection].backgroundImage != null)
            {
                menuItems[currentSelection].backgroundImage.color = menuItems[currentSelection].selectedColor;
            }
            if (menuItems[currentSelection].button != null)
            {
                menuItems[currentSelection].button.interactable = true;
            }
        }
    }
}
