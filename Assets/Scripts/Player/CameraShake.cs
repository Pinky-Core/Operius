using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(_ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator _ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
