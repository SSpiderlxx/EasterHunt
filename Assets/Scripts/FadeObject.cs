using UnityEngine;
using System.Collections;

public class FadeObject : MonoBehaviour
{

    [SerializeField] float fadeDuration = 1f;

    Renderer renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void FadeOut() => StartCoroutine(Fade(0f));
    public void FadeIn() => StartCoroutine(Fade(1f));

    IEnumerator Fade(float targetAlpha)
    {
        Material mat = renderer.material;
        Color col = mat.color;
        float startAlpha = col.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            col.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            mat.color = col;
            yield return null;
        }

        col.a = targetAlpha;
        mat.color = col;
    }
}
