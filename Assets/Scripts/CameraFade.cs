using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CameraFade : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDuration = 1f;

    public void StartFadeOutAndIn(Action onFullyFaded)
    {
        StartCoroutine(FadeOutAndIn(onFullyFaded));
    }

    private IEnumerator FadeOutAndIn(Action onFullyFaded)
    {
        fadeOverlay.gameObject.SetActive(true);
        yield return StartCoroutine(FadeTo(1));  // Fade to black
        onFullyFaded?.Invoke();  // Call the callback to change the area when fully black
        yield return new WaitForSeconds(0.5f);  // Wait at fully black for half a second
        yield return StartCoroutine(FadeTo(0));  // Fade back to scene
        fadeOverlay.gameObject.SetActive(false);
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float alpha = fadeOverlay.color.a;
        
        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, targetAlpha, t));
            fadeOverlay.color = newColor;
            yield return null;
        }

        fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
    }
}
