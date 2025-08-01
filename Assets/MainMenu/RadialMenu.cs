using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using TMPro; // Added for TextMeshProUGUI

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
        public GameObject panel; // Panel que se abre para esta opción
    }

    [System.Serializable]
    public class PanelAnimation
    {
        public GameObject panel;
        public RectTransform panelRect;
        public CanvasGroup panelCanvasGroup;
        public float openDuration = 0.5f;
        public float closeDuration = 0.3f;
        public Vector2 openScale = Vector2.one;
        public Vector2 closedScale = Vector2.zero;
        public AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve closeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        [Header("Tipo de Animación")]
        public PanelAnimationType animationType = PanelAnimationType.Hilo;
    }

    [System.Serializable]
    public class TextAnimation
    {
        public TextMeshProUGUI text;
        public TextAnimationType animationType = TextAnimationType.GlitchTypewriter;
        public float duration = 1f;
        public float delay = 0f;
        public bool enableShake = true;
        public float shakeIntensity = 2f;
    }

    [System.Serializable]
    public class ButtonAnimation
    {
        public Button button;
        public ButtonAnimationType animationType = ButtonAnimationType.Rebote;
        public float duration = 0.3f;
        public float delay = 0f;
    }

    [System.Serializable]
    public class ImageAnimation
    {
        public Image image;
        public ImageAnimationType animationType = ImageAnimationType.FadeRotate;
        public float duration = 0.4f;
        public float delay = 0f;
    }

    public enum PanelAnimationType
    {
        Hilo,           // Se estira como hilo
        Fade,           // Solo fade
        Scale,          // Solo escala
        SlideFromTop,   // Se desliza desde arriba
        SlideFromBottom, // Se desliza desde abajo
        SlideFromLeft,  // Se desliza desde la izquierda
        SlideFromRight, // Se desliza desde la derecha
        Pop,            // Aparece con pop
        None            // Sin animación
    }

    public enum TextAnimationType
    {
        GlitchTypewriter,   // Escritura con glitch
        Typewriter,         // Escritura normal
        FadeIn,            // Solo fade
        ScaleIn,           // Escala desde 0
        Shake,             // Solo shake
        Glitch,            // Solo glitch
        None               // Sin animación
    }

    public enum ButtonAnimationType
    {
        Rebote,            // Escala con rebote
        FadeIn,           // Solo fade
        ScaleIn,          // Escala desde 0
        SlideFromTop,     // Se desliza desde arriba
        SlideFromBottom,  // Se desliza desde abajo
        Rotate,           // Rotación
        None              // Sin animación
    }

    public enum ImageAnimationType
    {
        FadeRotate,       // Fade + rotación
        FadeIn,          // Solo fade
        ScaleIn,         // Escala desde 0
        SlideFromTop,    // Se desliza desde arriba
        SlideFromBottom, // Se desliza desde abajo
        Rotate,          // Solo rotación
        None             // Sin animación
    }

    [Header("Animaciones de Elementos")]
    public List<TextAnimation> textAnimations = new List<TextAnimation>();
    public List<ButtonAnimation> buttonAnimations = new List<ButtonAnimation>();
    public List<ImageAnimation> imageAnimations = new List<ImageAnimation>();

    public List<MenuItem> menuItems = new List<MenuItem>();
    public float radius = 300f;
    public float rotateSpeed = 5f;
    
    [Header("Giro por pasos")]
    public float snapSpeed = 10f; // Velocidad de movimiento a la posición
    public float inputThreshold = 10f; // Sensibilidad del input (bajado de 50 a 10)
    
    [Header("Sistema de Paneles")]
    public List<PanelAnimation> panelAnimations = new List<PanelAnimation>();
    public bool enablePanelAnimations = true;
    public float panelSwitchDelay = 0.1f; // Delay más corto entre cerrar y abrir panel

    [Header("Sistema de Skybox")]
    public Color[] panelSkyboxColors; // Colores del skybox para cada panel
    public float skyboxTransitionDuration = 2f; // Duración de transición del skybox
    public bool enableSkyboxTransition = true; // Habilitar transición de skybox

    private float angleStep;
    private int currentSelection = 0; // Opción actual seleccionada
    private float targetRotation = 0f;
    private bool isMoving = false;
    private GameObject currentOpenPanel = null;
    private bool isAnimatingPanel = false;
    private Coroutine skyboxTransitionCoroutine;

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
        
        // Inicializar paneles cerrados
        InitializePanels();
        
        // Ocultar elementos animables
        HideAllAnimatableElements();
        
        // Seleccionar la primera opción
        UpdateSelection(0);
        
        Debug.Log("RadialMenu iniciado con " + menuItems.Count + " elementos");
    }

    void HideAllAnimatableElements()
    {
        // Ocultar textos
        foreach (TextAnimation textAnim in textAnimations)
        {
            if (textAnim.text != null)
            {
                CanvasGroup textCanvasGroup = textAnim.text.GetComponent<CanvasGroup>();
                if (textCanvasGroup == null)
                {
                    textCanvasGroup = textAnim.text.gameObject.AddComponent<CanvasGroup>();
                }
                textCanvasGroup.alpha = 0f;
            }
        }

        // Ocultar botones
        foreach (ButtonAnimation buttonAnim in buttonAnimations)
        {
            if (buttonAnim.button != null)
            {
                CanvasGroup buttonCanvasGroup = buttonAnim.button.GetComponent<CanvasGroup>();
                if (buttonCanvasGroup == null)
                {
                    buttonCanvasGroup = buttonAnim.button.gameObject.AddComponent<CanvasGroup>();
                }
                buttonCanvasGroup.alpha = 0f;
            }
        }

        // Ocultar imágenes
        foreach (ImageAnimation imageAnim in imageAnimations)
        {
            if (imageAnim.image != null)
            {
                CanvasGroup imageCanvasGroup = imageAnim.image.GetComponent<CanvasGroup>();
                if (imageCanvasGroup == null)
                {
                    imageCanvasGroup = imageAnim.image.gameObject.AddComponent<CanvasGroup>();
                }
                imageCanvasGroup.alpha = 0f;
            }
        }
    }

    void InitializePanels()
    {
        // Cerrar todos los paneles al inicio
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i].panel != null)
            {
                menuItems[i].panel.SetActive(false);
            }
        }
        
        // Configurar animaciones de paneles
        for (int i = 0; i < panelAnimations.Count; i++)
        {
            if (panelAnimations[i].panel != null)
            {
                panelAnimations[i].panel.SetActive(false);
                if (panelAnimations[i].panelRect != null)
                {
                    panelAnimations[i].panelRect.localScale = panelAnimations[i].closedScale;
                }
                if (panelAnimations[i].panelCanvasGroup != null)
                {
                    panelAnimations[i].panelCanvasGroup.alpha = 0f;
                }
            }
        }
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
            
            // Reproducir sonido de rotación del menú
            if (MainMenuAudioManager.Instance != null)
            {
                MainMenuAudioManager.Instance.PlayMenuRotateSound();
            }
            
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
                menuItems[i].button.interactable = false;
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

        // Manejar animación de paneles
        if (enablePanelAnimations)
        {
            StartCoroutine(SwitchPanel(newSelection));
        }

        // Cambiar color del skybox si está habilitado
        if (enableSkyboxTransition)
        {
            ChangeSkyboxColor(newSelection);
        }
    }

    IEnumerator SwitchPanel(int newSelection)
    {
        if (isAnimatingPanel) yield break;
        isAnimatingPanel = true;

        // Cerrar panel actual si existe
        if (currentOpenPanel != null)
        {
            yield return StartCoroutine(ClosePanel(currentOpenPanel));
        }

        // Delay entre cerrar y abrir
        yield return new WaitForSeconds(panelSwitchDelay);

        // Abrir nuevo panel si existe
        if (newSelection >= 0 && newSelection < menuItems.Count)
        {
            if (menuItems[newSelection].panel != null)
            {
                currentOpenPanel = menuItems[newSelection].panel;
                yield return StartCoroutine(OpenPanel(currentOpenPanel));
            }
        }

        isAnimatingPanel = false;
    }

    IEnumerator OpenPanel(GameObject panel)
    {
        if (panel == null) yield break;

        // Ocultar todos los elementos del panel ANTES de abrirlo
        HidePanelElements(panel);

        panel.SetActive(true);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();

        if (panelRect != null)
        {
            // Buscar la animación del panel
            PanelAnimation panelAnim = panelAnimations.Find(p => p.panel == panel);
            if (panelAnim != null)
            {
                yield return StartCoroutine(AnimatePanel(panelAnim, true));
            }
            else
            {
                // Animación por defecto
                yield return StartCoroutine(AnimatePanelDefault(panelRect, canvasGroup, true));
            }

            // Esperar un poco antes de animar los elementos
            yield return new WaitForSeconds(0.1f);

            // Animar elementos dentro del panel DESPUÉS de que el panel esté abierto
            StartCoroutine(AnimatePanelElements(panel));
        }
    }

    void HidePanelElements(GameObject panel)
    {
        // Ocultar solo los elementos configurados en las listas de animación
        foreach (TextAnimation textAnim in textAnimations)
        {
            if (textAnim.text != null && textAnim.text.transform.IsChildOf(panel.transform))
            {
                CanvasGroup textCanvasGroup = textAnim.text.GetComponent<CanvasGroup>();
                if (textCanvasGroup == null)
                {
                    textCanvasGroup = textAnim.text.gameObject.AddComponent<CanvasGroup>();
                }
                textCanvasGroup.alpha = 0f;
            }
        }

        foreach (ButtonAnimation buttonAnim in buttonAnimations)
        {
            if (buttonAnim.button != null && buttonAnim.button.transform.IsChildOf(panel.transform))
            {
                CanvasGroup buttonCanvasGroup = buttonAnim.button.GetComponent<CanvasGroup>();
                if (buttonCanvasGroup == null)
                {
                    buttonCanvasGroup = buttonAnim.button.gameObject.AddComponent<CanvasGroup>();
                }
                buttonCanvasGroup.alpha = 0f;
            }
        }

        foreach (ImageAnimation imageAnim in imageAnimations)
        {
            if (imageAnim.image != null && imageAnim.image.transform.IsChildOf(panel.transform))
            {
                CanvasGroup imageCanvasGroup = imageAnim.image.GetComponent<CanvasGroup>();
                if (imageCanvasGroup == null)
                {
                    imageCanvasGroup = imageAnim.image.gameObject.AddComponent<CanvasGroup>();
                }
                imageCanvasGroup.alpha = 0f;
            }
        }
    }

    IEnumerator AnimatePanel(PanelAnimation panelAnim, bool isOpening)
    {
        if (panelAnim.panelRect == null) yield break;

        float duration = isOpening ? panelAnim.openDuration : panelAnim.closeDuration;
        Vector2 startScale = isOpening ? panelAnim.closedScale : panelAnim.openScale;
        Vector2 targetScale = isOpening ? panelAnim.openScale : panelAnim.closedScale;
        AnimationCurve curve = isOpening ? panelAnim.openCurve : panelAnim.closeCurve;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            float curveProgress = curve.Evaluate(progress);

            switch (panelAnim.animationType)
            {
                case PanelAnimationType.Hilo:
                    // Efecto hilo: escala desde el centro
                    float scaleProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
                    panelAnim.panelRect.localScale = Vector2.Lerp(startScale, targetScale, scaleProgress);
                    break;

                case PanelAnimationType.Fade:
                    // Solo fade
                    if (panelAnim.panelCanvasGroup != null)
                    {
                        panelAnim.panelCanvasGroup.alpha = isOpening ? progress : 1f - progress;
                    }
                    break;

                case PanelAnimationType.Scale:
                    // Solo escala
                    panelAnim.panelRect.localScale = Vector2.Lerp(startScale, targetScale, curveProgress);
                    break;

                case PanelAnimationType.SlideFromTop:
                    // Deslizar desde arriba
                    Vector2 startPos = new Vector2(0, 1000);
                    Vector2 endPos = Vector2.zero;
                    panelAnim.panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, curveProgress);
                    break;

                case PanelAnimationType.SlideFromBottom:
                    // Deslizar desde abajo
                    Vector2 startPosBottom = new Vector2(0, -1000);
                    Vector2 endPosBottom = Vector2.zero;
                    panelAnim.panelRect.anchoredPosition = Vector2.Lerp(startPosBottom, endPosBottom, curveProgress);
                    break;

                case PanelAnimationType.SlideFromLeft:
                    // Deslizar desde la izquierda
                    Vector2 startPosLeft = new Vector2(-1000, 0);
                    Vector2 endPosLeft = Vector2.zero;
                    panelAnim.panelRect.anchoredPosition = Vector2.Lerp(startPosLeft, endPosLeft, curveProgress);
                    break;

                case PanelAnimationType.SlideFromRight:
                    // Deslizar desde la derecha
                    Vector2 startPosRight = new Vector2(1000, 0);
                    Vector2 endPosRight = Vector2.zero;
                    panelAnim.panelRect.anchoredPosition = Vector2.Lerp(startPosRight, endPosRight, curveProgress);
                    break;

                case PanelAnimationType.Pop:
                    // Efecto pop con rebote
                    float popProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
                    panelAnim.panelRect.localScale = Vector2.Lerp(startScale, targetScale, popProgress);
                    break;

                case PanelAnimationType.None:
                    // Sin animación
                    panelAnim.panelRect.localScale = targetScale;
                    if (panelAnim.panelCanvasGroup != null)
                    {
                        panelAnim.panelCanvasGroup.alpha = isOpening ? 1f : 0f;
                    }
                    break;
            }

            yield return null;
        }

        // Asegurar valores finales
        panelAnim.panelRect.localScale = targetScale;
        if (panelAnim.panelCanvasGroup != null)
        {
            panelAnim.panelCanvasGroup.alpha = isOpening ? 1f : 0f;
        }
    }

    IEnumerator AnimatePanelDefault(RectTransform panelRect, CanvasGroup canvasGroup, bool isOpening)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector2 startScale = isOpening ? Vector2.zero : Vector2.one;
        Vector2 targetScale = isOpening ? Vector2.one : Vector2.zero;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            
            // Efecto hilo por defecto
            float scaleProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
            panelRect.localScale = Vector2.Lerp(startScale, targetScale, scaleProgress);
            
            if (canvasGroup != null)
            {
                canvasGroup.alpha = isOpening ? progress : 1f - progress;
            }
            
            yield return null;
        }

        panelRect.localScale = targetScale;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isOpening ? 1f : 0f;
        }
    }

    IEnumerator AnimatePanelElements(GameObject panel)
    {
        Debug.Log("AnimatePanelElements iniciado para panel: " + panel.name);

        // Animar textos configurados
        int textCount = 0;
        foreach (TextAnimation textAnim in textAnimations)
        {
            if (textAnim.text != null && textAnim.text.transform.IsChildOf(panel.transform))
            {
                Debug.Log("Animando texto: " + textAnim.text.name + " con tipo: " + textAnim.animationType);
                StartCoroutine(AnimateTextElement(textAnim));
                textCount++;
            }
        }
        Debug.Log("Textos animados: " + textCount);

        // Animar botones configurados
        int buttonCount = 0;
        foreach (ButtonAnimation buttonAnim in buttonAnimations)
        {
            if (buttonAnim.button != null && buttonAnim.button.transform.IsChildOf(panel.transform))
            {
                Debug.Log("Animando botón: " + buttonAnim.button.name + " con tipo: " + buttonAnim.animationType);
                StartCoroutine(AnimateButtonElement(buttonAnim));
                buttonCount++;
            }
        }
        Debug.Log("Botones animados: " + buttonCount);

        // Animar imágenes configuradas
        int imageCount = 0;
        foreach (ImageAnimation imageAnim in imageAnimations)
        {
            if (imageAnim.image != null && imageAnim.image.transform.IsChildOf(panel.transform))
            {
                Debug.Log("Animando imagen: " + imageAnim.image.name + " con tipo: " + imageAnim.animationType);
                StartCoroutine(AnimateImageElement(imageAnim));
                imageCount++;
            }
        }
        Debug.Log("Imágenes animadas: " + imageCount);

        yield return null;
    }

    IEnumerator AnimateTextElement(TextAnimation textAnim)
    {
        if (textAnim.text == null) yield break;

        Debug.Log("AnimateTextElement iniciado para: " + textAnim.text.name);

        yield return new WaitForSeconds(textAnim.delay);

        // MOSTRAR el elemento antes de animarlo
        CanvasGroup textCanvasGroup = textAnim.text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
        {
            textCanvasGroup = textAnim.text.gameObject.AddComponent<CanvasGroup>();
        }
        textCanvasGroup.alpha = 1f; // Mostrar el elemento
        Debug.Log("Elemento mostrado antes de animación: " + textAnim.text.name);

        string originalText = textAnim.text.text;
        Vector3 originalPos = textAnim.text.transform.localPosition;

        Debug.Log("Ejecutando animación de texto: " + textAnim.animationType);

        switch (textAnim.animationType)
        {
            case TextAnimationType.GlitchTypewriter:
                yield return StartCoroutine(GlitchTypewriterAnimation(textAnim, originalText, originalPos));
                break;

            case TextAnimationType.Typewriter:
                yield return StartCoroutine(TypewriterAnimation(textAnim, originalText));
                break;

            case TextAnimationType.FadeIn:
                yield return StartCoroutine(FadeInAnimation(textAnim));
                break;

            case TextAnimationType.ScaleIn:
                yield return StartCoroutine(ScaleInAnimation(textAnim));
                break;

            case TextAnimationType.Shake:
                yield return StartCoroutine(ShakeAnimation(textAnim, originalPos));
                break;

            case TextAnimationType.Glitch:
                yield return StartCoroutine(GlitchAnimation(textAnim, originalText));
                break;

            case TextAnimationType.None:
                Debug.Log("Sin animación para texto: " + textAnim.text.name);
                break;
        }

        Debug.Log("AnimateTextElement completado para: " + textAnim.text.name);
    }

    IEnumerator GlitchTypewriterAnimation(TextAnimation textAnim, string originalText, Vector3 originalPos)
    {
        textAnim.text.text = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            if (Random.Range(0f, 1f) < 0.1f)
            {
                textAnim.text.text += GetRandomGlitchChar();
                yield return new WaitForSeconds(0.05f);
                textAnim.text.text = textAnim.text.text.Substring(0, textAnim.text.text.Length - 1);
            }

            textAnim.text.text += originalText[i];
            yield return new WaitForSeconds(0.05f);
        }

        if (textAnim.enableShake)
        {
            for (int j = 0; j < 5; j++)
            {
                textAnim.text.transform.localPosition = originalPos + new Vector3(
                    Random.Range(-textAnim.shakeIntensity, textAnim.shakeIntensity),
                    Random.Range(-textAnim.shakeIntensity, textAnim.shakeIntensity), 0);
                yield return new WaitForSeconds(0.05f);
            }
            textAnim.text.transform.localPosition = originalPos;
        }
    }

    IEnumerator TypewriterAnimation(TextAnimation textAnim, string originalText)
    {
        textAnim.text.text = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            textAnim.text.text += originalText[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeInAnimation(TextAnimation textAnim)
    {
        Debug.Log("FadeInAnimation iniciado para: " + textAnim.text.name + " con duración: " + textAnim.duration);
        
        CanvasGroup textCanvasGroup = textAnim.text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null)
        {
            textCanvasGroup = textAnim.text.gameObject.AddComponent<CanvasGroup>();
        }

        // Ocultar inicialmente para la animación fade
        textCanvasGroup.alpha = 0f;
        Debug.Log("Alpha inicial: " + textCanvasGroup.alpha);
        
        float elapsedTime = 0f;

        while (elapsedTime < textAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / textAnim.duration;
            textCanvasGroup.alpha = progress;
            Debug.Log("Alpha progreso: " + textCanvasGroup.alpha + " (" + progress * 100 + "%)");
            yield return null;
        }

        textCanvasGroup.alpha = 1f;
        Debug.Log("Alpha final: " + textCanvasGroup.alpha);
        Debug.Log("FadeInAnimation completado para: " + textAnim.text.name);
    }

    IEnumerator ScaleInAnimation(TextAnimation textAnim)
    {
        Debug.Log("ScaleInAnimation iniciado para: " + textAnim.text.name + " con duración: " + textAnim.duration);
        
        Vector3 originalScale = textAnim.text.transform.localScale;
        // Ocultar inicialmente para la animación scale
        textAnim.text.transform.localScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < textAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / textAnim.duration;
            textAnim.text.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            Debug.Log("Scale progreso: " + progress * 100 + "%");
            yield return null;
        }

        textAnim.text.transform.localScale = originalScale;
        Debug.Log("ScaleInAnimation completado para: " + textAnim.text.name);
    }

    IEnumerator ShakeAnimation(TextAnimation textAnim, Vector3 originalPos)
    {
        for (int j = 0; j < 10; j++)
        {
            textAnim.text.transform.localPosition = originalPos + new Vector3(
                Random.Range(-textAnim.shakeIntensity, textAnim.shakeIntensity),
                Random.Range(-textAnim.shakeIntensity, textAnim.shakeIntensity), 0);
            yield return new WaitForSeconds(0.05f);
        }
        textAnim.text.transform.localPosition = originalPos;
    }

    IEnumerator GlitchAnimation(TextAnimation textAnim, string originalText)
    {
        for (int i = 0; i < 20; i++)
        {
            textAnim.text.text = originalText;
            yield return new WaitForSeconds(0.1f);
            textAnim.text.text = GetRandomGlitchString(originalText.Length);
            yield return new WaitForSeconds(0.05f);
        }
        textAnim.text.text = originalText;
    }

    IEnumerator AnimateButtonElement(ButtonAnimation buttonAnim)
    {
        if (buttonAnim.button == null) yield break;

        Debug.Log("AnimateButtonElement iniciado para: " + buttonAnim.button.name);

        yield return new WaitForSeconds(buttonAnim.delay);

        // MOSTRAR el elemento antes de animarlo
        CanvasGroup buttonCanvasGroup = buttonAnim.button.GetComponent<CanvasGroup>();
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = buttonAnim.button.gameObject.AddComponent<CanvasGroup>();
        }
        buttonCanvasGroup.alpha = 1f; // Mostrar el elemento
        Debug.Log("Elemento mostrado antes de animación: " + buttonAnim.button.name);

        RectTransform buttonRect = buttonAnim.button.GetComponent<RectTransform>();
        if (buttonRect == null) yield break;

        Vector3 originalScale = buttonRect.localScale;
        Vector3 originalPos = buttonRect.localPosition;

        Debug.Log("Ejecutando animación de botón: " + buttonAnim.animationType);

        switch (buttonAnim.animationType)
        {
            case ButtonAnimationType.Rebote:
                yield return StartCoroutine(ReboteAnimation(buttonRect, originalScale));
                break;

            case ButtonAnimationType.FadeIn:
                yield return StartCoroutine(ButtonFadeInAnimation(buttonAnim));
                break;

            case ButtonAnimationType.ScaleIn:
                yield return StartCoroutine(ButtonScaleInAnimation(buttonRect, originalScale));
                break;

            case ButtonAnimationType.SlideFromTop:
                yield return StartCoroutine(ButtonSlideAnimation(buttonRect, originalPos, new Vector3(0, 200, 0)));
                break;

            case ButtonAnimationType.SlideFromBottom:
                yield return StartCoroutine(ButtonSlideAnimation(buttonRect, originalPos, new Vector3(0, -200, 0)));
                break;

            case ButtonAnimationType.Rotate:
                yield return StartCoroutine(ButtonRotateAnimation(buttonRect));
                break;

            case ButtonAnimationType.None:
                Debug.Log("Sin animación para botón: " + buttonAnim.button.name);
                break;
        }

        Debug.Log("AnimateButtonElement completado para: " + buttonAnim.button.name);
    }

    IEnumerator ReboteAnimation(RectTransform buttonRect, Vector3 originalScale)
    {
        buttonRect.localScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 0.3f;
            float bounceProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
            buttonRect.localScale = Vector3.Lerp(Vector3.zero, originalScale, bounceProgress);
            yield return null;
        }

        buttonRect.localScale = originalScale;
    }

    IEnumerator ButtonFadeInAnimation(ButtonAnimation buttonAnim)
    {
        Debug.Log("ButtonFadeInAnimation iniciado para: " + buttonAnim.button.name + " con duración: " + buttonAnim.duration);
        
        CanvasGroup buttonCanvasGroup = buttonAnim.button.GetComponent<CanvasGroup>();
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = buttonAnim.button.gameObject.AddComponent<CanvasGroup>();
        }

        // Ocultar inicialmente para la animación fade
        buttonCanvasGroup.alpha = 0f;
        Debug.Log("Alpha inicial botón: " + buttonCanvasGroup.alpha);
        
        float elapsedTime = 0f;

        while (elapsedTime < buttonAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / buttonAnim.duration;
            buttonCanvasGroup.alpha = progress;
            Debug.Log("Alpha progreso botón: " + buttonCanvasGroup.alpha + " (" + progress * 100 + "%)");
            yield return null;
        }

        buttonCanvasGroup.alpha = 1f;
        Debug.Log("Alpha final botón: " + buttonCanvasGroup.alpha);
        Debug.Log("ButtonFadeInAnimation completado para: " + buttonAnim.button.name);
    }

    IEnumerator ButtonScaleInAnimation(RectTransform buttonRect, Vector3 originalScale)
    {
        Debug.Log("ButtonScaleInAnimation iniciado con duración: 0.3f");
        
        // Ocultar inicialmente para la animación scale
        buttonRect.localScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 0.3f;
            buttonRect.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            Debug.Log("Button scale progreso: " + progress * 100 + "%");
            yield return null;
        }

        buttonRect.localScale = originalScale;
        Debug.Log("ButtonScaleInAnimation completado");
    }

    IEnumerator ButtonSlideAnimation(RectTransform buttonRect, Vector3 originalPos, Vector3 startOffset)
    {
        Debug.Log("ButtonSlideAnimation iniciado con duración: 0.3f");
        
        // Ocultar inicialmente (fuera de pantalla)
        buttonRect.localPosition = originalPos + startOffset;
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 0.3f;
            buttonRect.localPosition = Vector3.Lerp(originalPos + startOffset, originalPos, progress);
            Debug.Log("Button slide progreso: " + progress * 100 + "%");
            yield return null;
        }

        buttonRect.localPosition = originalPos;
        Debug.Log("ButtonSlideAnimation completado");
    }

    IEnumerator ButtonRotateAnimation(RectTransform buttonRect)
    {
        Vector3 originalRotation = buttonRect.localRotation.eulerAngles;
        buttonRect.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + 360f);
        float elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / 0.5f;
            buttonRect.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y,
                Mathf.Lerp(originalRotation.z + 360f, originalRotation.z, progress));
            yield return null;
        }

        buttonRect.localRotation = Quaternion.Euler(originalRotation);
    }

    IEnumerator AnimateImageElement(ImageAnimation imageAnim)
    {
        if (imageAnim.image == null) yield break;

        Debug.Log("AnimateImageElement iniciado para: " + imageAnim.image.name);

        yield return new WaitForSeconds(imageAnim.delay);

        // MOSTRAR el elemento antes de animarlo
        CanvasGroup imageCanvasGroup = imageAnim.image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
        {
            imageCanvasGroup = imageAnim.image.gameObject.AddComponent<CanvasGroup>();
        }
        imageCanvasGroup.alpha = 1f; // Mostrar el elemento
        Debug.Log("Elemento mostrado antes de animación: " + imageAnim.image.name);

        Vector3 originalScale = imageAnim.image.transform.localScale;
        Vector3 originalPos = imageAnim.image.transform.localPosition;
        Vector3 originalRotation = imageAnim.image.transform.localRotation.eulerAngles;

        Debug.Log("Ejecutando animación de imagen: " + imageAnim.animationType);

        switch (imageAnim.animationType)
        {
            case ImageAnimationType.FadeRotate:
                yield return StartCoroutine(ImageFadeRotateAnimation(imageAnim, originalRotation));
                break;

            case ImageAnimationType.FadeIn:
                yield return StartCoroutine(ImageFadeInAnimation(imageAnim));
                break;

            case ImageAnimationType.ScaleIn:
                yield return StartCoroutine(ImageScaleInAnimation(imageAnim, originalScale));
                break;

            case ImageAnimationType.SlideFromTop:
                yield return StartCoroutine(ImageSlideAnimation(imageAnim, originalPos, new Vector3(0, 200, 0)));
                break;

            case ImageAnimationType.SlideFromBottom:
                yield return StartCoroutine(ImageSlideAnimation(imageAnim, originalPos, new Vector3(0, -200, 0)));
                break;

            case ImageAnimationType.Rotate:
                yield return StartCoroutine(ImageRotateAnimation(imageAnim, originalRotation));
                break;

            case ImageAnimationType.None:
                Debug.Log("Sin animación para imagen: " + imageAnim.image.name);
                break;
        }

        Debug.Log("AnimateImageElement completado para: " + imageAnim.image.name);
    }

    IEnumerator ImageFadeRotateAnimation(ImageAnimation imageAnim, Vector3 originalRotation)
    {
        CanvasGroup imageCanvasGroup = imageAnim.image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
        {
            imageCanvasGroup = imageAnim.image.gameObject.AddComponent<CanvasGroup>();
        }

        // Ocultar inicialmente
        imageCanvasGroup.alpha = 0f;
        imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + 180f);

        float elapsedTime = 0f;
        while (elapsedTime < imageAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / imageAnim.duration;
            
            imageCanvasGroup.alpha = progress;
            imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y,
                Mathf.Lerp(originalRotation.z + 180f, originalRotation.z, progress));
            
            yield return null;
        }

        imageCanvasGroup.alpha = 1f;
        imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation);
    }

    IEnumerator ImageFadeInAnimation(ImageAnimation imageAnim)
    {
        Debug.Log("ImageFadeInAnimation iniciado para: " + imageAnim.image.name + " con duración: " + imageAnim.duration);
        
        CanvasGroup imageCanvasGroup = imageAnim.image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null)
        {
            imageCanvasGroup = imageAnim.image.gameObject.AddComponent<CanvasGroup>();
        }

        // Ocultar inicialmente para la animación fade
        imageCanvasGroup.alpha = 0f;
        Debug.Log("Alpha inicial imagen: " + imageCanvasGroup.alpha);
        
        float elapsedTime = 0f;

        while (elapsedTime < imageAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / imageAnim.duration;
            imageCanvasGroup.alpha = progress;
            Debug.Log("Alpha progreso imagen: " + imageCanvasGroup.alpha + " (" + progress * 100 + "%)");
            yield return null;
        }

        imageCanvasGroup.alpha = 1f;
        Debug.Log("Alpha final imagen: " + imageCanvasGroup.alpha);
        Debug.Log("ImageFadeInAnimation completado para: " + imageAnim.image.name);
    }

    IEnumerator ImageScaleInAnimation(ImageAnimation imageAnim, Vector3 originalScale)
    {
        Debug.Log("ImageScaleInAnimation iniciado para: " + imageAnim.image.name + " con duración: " + imageAnim.duration);
        
        // Ocultar inicialmente para la animación scale
        imageAnim.image.transform.localScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < imageAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / imageAnim.duration;
            imageAnim.image.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            Debug.Log("Image scale progreso: " + progress * 100 + "%");
            yield return null;
        }

        imageAnim.image.transform.localScale = originalScale;
        Debug.Log("ImageScaleInAnimation completado para: " + imageAnim.image.name);
    }

    IEnumerator ImageSlideAnimation(ImageAnimation imageAnim, Vector3 originalPos, Vector3 startOffset)
    {
        Debug.Log("ImageSlideAnimation iniciado para: " + imageAnim.image.name + " con duración: " + imageAnim.duration);
        
        // Ocultar inicialmente (fuera de pantalla)
        imageAnim.image.transform.localPosition = originalPos + startOffset;
        float elapsedTime = 0f;

        while (elapsedTime < imageAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / imageAnim.duration;
            imageAnim.image.transform.localPosition = Vector3.Lerp(originalPos + startOffset, originalPos, progress);
            Debug.Log("Image slide progreso: " + progress * 100 + "%");
            yield return null;
        }

        imageAnim.image.transform.localPosition = originalPos;
        Debug.Log("ImageSlideAnimation completado para: " + imageAnim.image.name);
    }

    IEnumerator ImageRotateAnimation(ImageAnimation imageAnim, Vector3 originalRotation)
    {
        imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + 360f);
        float elapsedTime = 0f;

        while (elapsedTime < imageAnim.duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / imageAnim.duration;
            imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation.x, originalRotation.y,
                Mathf.Lerp(originalRotation.z + 360f, originalRotation.z, progress));
            yield return null;
        }

        imageAnim.image.transform.localRotation = Quaternion.Euler(originalRotation);
    }

    string GetRandomGlitchChar()
    {
        string glitchChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        return glitchChars[Random.Range(0, glitchChars.Length)].ToString();
    }

    string GetRandomGlitchString(int length)
    {
        string glitchChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        string result = "";
        for (int i = 0; i < length; i++)
        {
            result += glitchChars[Random.Range(0, glitchChars.Length)];
        }
        return result;
    }

    IEnumerator ClosePanel(GameObject panel)
    {
        if (panel == null) yield break;

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();

        if (panelRect != null)
        {
            float elapsedTime = 0f;
            Vector2 startScale = panelRect.localScale;
            Vector2 targetScale = Vector2.zero;

            while (elapsedTime < 0.2f) // Más rápido
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / 0.2f;
                
                // Animación simple: escala lineal
                panelRect.localScale = Vector2.Lerp(startScale, targetScale, progress);
                
                // Fade simple
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f - progress;
                }
                
                yield return null;
            }

            panel.SetActive(false);
            panelRect.localScale = targetScale;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }
    }

    void ChangeSkyboxColor(int panelIndex)
    {
        // Verificar que hay colores configurados y el índice es válido
        if (panelSkyboxColors != null && panelSkyboxColors.Length > 0 && 
            panelIndex >= 0 && panelIndex < panelSkyboxColors.Length)
        {
            // Detener transición anterior si existe
            if (skyboxTransitionCoroutine != null)
            {
                StopCoroutine(skyboxTransitionCoroutine);
            }

            // Iniciar nueva transición
            skyboxTransitionCoroutine = StartCoroutine(TransitionSkyboxColor(panelSkyboxColors[panelIndex]));
            
            Debug.Log("Cambiando skybox a color del panel " + panelIndex + ": " + panelSkyboxColors[panelIndex]);
        }
        else
        {
            Debug.LogWarning("No hay colores de skybox configurados para el panel " + panelIndex);
        }
    }

    IEnumerator TransitionSkyboxColor(Color targetColor)
    {
        // Obtener el color actual del skybox
        Color startColor = RenderSettings.skybox.GetColor("_Tint");
        float elapsedTime = 0f;

        Debug.Log("Iniciando transición de skybox de " + startColor + " a " + targetColor);

        while (elapsedTime < skyboxTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / skyboxTransitionDuration;
            
            // Interpolación suave del color
            Color lerpedColor = Color.Lerp(startColor, targetColor, progress);
            RenderSettings.skybox.SetColor("_Tint", lerpedColor);
            DynamicGI.UpdateEnvironment();
            
            yield return null;
        }

        // Asegurar el color final exacto
        RenderSettings.skybox.SetColor("_Tint", targetColor);
        DynamicGI.UpdateEnvironment();
        
        Debug.Log("Transición de skybox completada: " + targetColor);
    }
}
