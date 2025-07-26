using UnityEngine;

public class TouchRotator : MonoBehaviour
{
    public RadialMenu radialMenu;
    public float mouseSensitivity = 1f;
    public float touchSensitivity = 1f;
    public bool enableMouse = true;
    public bool enableTouch = true;

    private Vector2 startInput;
    private bool isDragging = false;
    private float accumulatedDelta = 0f;
    private bool hasTriggered = false; // Evitar múltiples activaciones

    void Update()
    {
        // Control con mouse
        if (enableMouse && Input.GetMouseButtonDown(0))
        {
            startInput = Input.mousePosition;
            isDragging = true;
            accumulatedDelta = 0f;
            hasTriggered = false;
            Debug.Log("Mouse: Empezó arrastre");
        }
        else if (enableMouse && Input.GetMouseButton(0) && isDragging)
        {
            float deltaY = Input.mousePosition.y - startInput.y;
            
            // Solo acumular si el movimiento es significativo
            if (Mathf.Abs(deltaY) > 1f)
            {
                accumulatedDelta += deltaY;
                Debug.Log("Mouse: Delta Y = " + deltaY + ", Acumulado = " + accumulatedDelta);
                
                // Solo activar una vez por arrastre
                if (Mathf.Abs(accumulatedDelta) > 25f && !hasTriggered)
                {
                    int direction = accumulatedDelta > 0 ? -1 : 1; // Invertido el signo
                    Debug.Log("Mouse: Activando dirección " + (direction > 0 ? "ARRIBA" : "ABAJO"));
                    radialMenu.RotateMenu(direction * 50f);
                    hasTriggered = true;
                }
            }
            
            startInput = Input.mousePosition;
        }
        else if (enableMouse && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            accumulatedDelta = 0f;
            hasTriggered = false;
            Debug.Log("Mouse: Terminó arrastre");
        }

        // Control con touch
        if (enableTouch && Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            
            if (t.phase == TouchPhase.Began)
            {
                startInput = t.position;
                isDragging = true;
                accumulatedDelta = 0f;
                hasTriggered = false;
                Debug.Log("Touch: Empezó arrastre");
            }
            else if (t.phase == TouchPhase.Moved && isDragging)
            {
                float deltaY = t.position.y - startInput.y;
                
                // Solo acumular si el movimiento es significativo
                if (Mathf.Abs(deltaY) > 1f)
                {
                    accumulatedDelta += deltaY;
                    Debug.Log("Touch: Delta Y = " + deltaY + ", Acumulado = " + accumulatedDelta);
                    
                    // Solo activar una vez por arrastre
                    if (Mathf.Abs(accumulatedDelta) > 25f && !hasTriggered)
                    {
                        int direction = accumulatedDelta > 0 ? -1 : 1; // Invertido el signo
                        Debug.Log("Touch: Activando dirección " + (direction > 0 ? "ARRIBA" : "ABAJO"));
                        radialMenu.RotateMenu(direction * 50f);
                        hasTriggered = true;
                    }
                }
                
                startInput = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                isDragging = false;
                accumulatedDelta = 0f;
                hasTriggered = false;
                Debug.Log("Touch: Terminó arrastre");
            }
        }
    }
}
