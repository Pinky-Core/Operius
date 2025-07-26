using UnityEngine;
using System.Collections;

public class GameOverPanelAnimator : MonoBehaviour
{
    public enum MainAnimation { MoveDownFadeIn, None }
    public enum SecondaryAnimation { Latido, Temblor, Glitch, None }

    public MainAnimation mainAnimation = MainAnimation.MoveDownFadeIn;
    public SecondaryAnimation secondaryAnimation = SecondaryAnimation.None;

    public float moveDuration = 1f;
    public float fadeDuration = 1f;
    public float latidoScale = 1.1f;
    public float latidoSpeed = 2f;
    public float temblorAmount = 10f;
    public float temblorSpeed = 30f;
    public int glitchTimes = 10;
    public float glitchInterval = 0.05f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 targetPos;
    private Vector2 startPos;
    private Vector3 originalScale;
    private bool animating = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        targetPos = rectTransform.anchoredPosition;
        startPos = targetPos + new Vector2(0, 400); // Empieza arriba
        originalScale = rectTransform.localScale;
    }

    void OnEnable()
    {
        StartCoroutine(AnimatePanel());
    }

    IEnumerator AnimatePanel()
    {
        animating = true;
        // Inicializa
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;
        float t = 0f;
        // Movimiento y fade in
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(moveDuration, 0.01f);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / Mathf.Max(fadeDuration, 0.01f));
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;
        canvasGroup.alpha = 1f;
        // Animación secundaria
        switch (secondaryAnimation)
        {
            case SecondaryAnimation.Latido:
                StartCoroutine(LatidoLoop());
                break;
            case SecondaryAnimation.Temblor:
                StartCoroutine(TemblorLoop());
                break;
            case SecondaryAnimation.Glitch:
                StartCoroutine(GlitchLoop());
                break;
        }
        animating = false;
    }

    IEnumerator LatidoLoop()
    {
        while (true)
        {
            float t = 0f;
            while (t < Mathf.PI)
            {
                t += Time.deltaTime * latidoSpeed;
                float scale = Mathf.Lerp(1f, latidoScale, Mathf.Sin(t));
                rectTransform.localScale = originalScale * scale;
                yield return null;
            }
            rectTransform.localScale = originalScale;
        }
    }

    IEnumerator TemblorLoop()
    {
        while (true)
        {
            float offsetX = Random.Range(-temblorAmount, temblorAmount);
            float offsetY = Random.Range(-temblorAmount, temblorAmount);
            rectTransform.anchoredPosition = targetPos + new Vector2(offsetX, offsetY);
            yield return new WaitForSeconds(1f / temblorSpeed);
            rectTransform.anchoredPosition = targetPos;
        }
    }

    IEnumerator GlitchLoop()
    {
        for (int i = 0; i < glitchTimes; i++)
        {
            float offsetX = Random.Range(-10f, 10f);
            float offsetY = Random.Range(-10f, 10f);
            rectTransform.anchoredPosition = targetPos + new Vector2(offsetX, offsetY);
            rectTransform.localScale = originalScale * Random.Range(0.95f, 1.05f);
            yield return new WaitForSeconds(glitchInterval);
        }
        rectTransform.anchoredPosition = targetPos;
        rectTransform.localScale = originalScale;
    }
} 