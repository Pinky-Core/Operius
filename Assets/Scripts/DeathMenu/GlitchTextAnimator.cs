using UnityEngine;
using TMPro;
using System.Collections;

public class GlitchTextAnimator : MonoBehaviour
{
    public enum MainAnimation { GlitchTypewriter, Typewriter, Sello, FadeIn, None }
    public enum SecondaryAnimation { Latido, Temblor, Glitch, None }

    public MainAnimation mainAnimation = MainAnimation.GlitchTypewriter;
    public SecondaryAnimation secondaryAnimation = SecondaryAnimation.None;

    [TextArea]
    public string finalText = "GAME OVER";
    public float typeSpeed = 0.05f;
    public float glitchCharDuration = 0.03f;
    public string glitchChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
    public float selloStartScale = 2f;
    public float selloDuration = 0.5f;
    public float fadeDuration = 0.7f;
    public float latidoScale = 1.1f;
    public float latidoSpeed = 2f;
    public float temblorAmount = 2f;
    public float temblorSpeed = 30f;
    public int glitchTimes = 10;
    public float glitchInterval = 0.05f;

    private TextMeshProUGUI tmp;
    private Vector3 originalScale;
    private Color originalColor;
    private bool animating = false;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        originalScale = tmp.rectTransform.localScale;
        originalColor = tmp.color;
    }

    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        animating = true;
        tmp.text = "";
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        tmp.rectTransform.localScale = originalScale;
        switch (mainAnimation)
        {
            case MainAnimation.GlitchTypewriter:
                yield return StartCoroutine(GlitchTypewriter(finalText));
                break;
            case MainAnimation.Typewriter:
                yield return StartCoroutine(Typewriter(finalText));
                break;
            case MainAnimation.Sello:
                yield return StartCoroutine(Sello(finalText));
                break;
            case MainAnimation.FadeIn:
                yield return StartCoroutine(FadeIn(finalText));
                break;
            case MainAnimation.None:
                tmp.text = finalText;
                tmp.color = originalColor;
                break;
        }
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

    IEnumerator GlitchTypewriter(string text)
    {
        tmp.text = "";
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        for (int i = 0; i < text.Length; i++)
        {
            // Glitch mientras escribe
            for (float t = 0; t < typeSpeed; t += glitchCharDuration)
            {
                string glitched = text.Substring(0, i);
                glitched += glitchChars[Random.Range(0, glitchChars.Length)];
                if (i < text.Length - 1)
                    glitched += new string('_', text.Length - i - 1);
                tmp.text = glitched;
                yield return new WaitForSeconds(glitchCharDuration);
            }
            tmp.text = text.Substring(0, i + 1);
        }
        // Glitch final
        for (int j = 0; j < glitchTimes; j++)
        {
            string glitched = "";
            for (int k = 0; k < text.Length; k++)
            {
                if (Random.value > 0.7f)
                    glitched += glitchChars[Random.Range(0, glitchChars.Length)];
                else
                    glitched += text[k];
            }
            tmp.text = glitched;
            yield return new WaitForSeconds(glitchInterval);
        }
        tmp.text = text;
    }

    IEnumerator Typewriter(string text)
    {
        tmp.text = "";
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        for (int i = 0; i < text.Length; i++)
        {
            tmp.text = text.Substring(0, i + 1);
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator Sello(string text)
    {
        tmp.text = text;
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        tmp.rectTransform.localScale = originalScale * selloStartScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(selloDuration, 0.01f);
            float scale = Mathf.Lerp(selloStartScale, 1f, t);
            tmp.rectTransform.localScale = originalScale * scale;
            yield return null;
        }
        tmp.rectTransform.localScale = originalScale;
    }

    IEnumerator FadeIn(string text)
    {
        tmp.text = text;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(fadeDuration, 0.01f);
            float alpha = Mathf.Lerp(0f, 1f, t);
            tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        tmp.color = originalColor;
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
                tmp.rectTransform.localScale = originalScale * scale;
                yield return null;
            }
            tmp.rectTransform.localScale = originalScale;
        }
    }

    IEnumerator TemblorLoop()
    {
        while (true)
        {
            float offsetX = Random.Range(-temblorAmount, temblorAmount);
            float offsetY = Random.Range(-temblorAmount, temblorAmount);
            tmp.rectTransform.anchoredPosition += new Vector2(offsetX, offsetY);
            yield return new WaitForSeconds(1f / temblorSpeed);
            tmp.rectTransform.anchoredPosition -= new Vector2(offsetX, offsetY);
        }
    }

    IEnumerator GlitchLoop()
    {
        for (int i = 0; i < glitchTimes; i++)
        {
            string glitched = "";
            for (int k = 0; k < finalText.Length; k++)
            {
                if (Random.value > 0.7f)
                    glitched += glitchChars[Random.Range(0, glitchChars.Length)];
                else
                    glitched += finalText[k];
            }
            tmp.text = glitched;
            yield return new WaitForSeconds(glitchInterval);
        }
        tmp.text = finalText;
    }
} 