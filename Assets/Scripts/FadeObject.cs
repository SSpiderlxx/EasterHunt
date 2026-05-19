using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{

    [SerializeField] float fadeDuration = 1f;

    List<Renderer> renderers = new List<Renderer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Renderer parentRenderer = GetComponent<Renderer>();

        if (parentRenderer != null)
        {
            renderers.Add(parentRenderer);
            return;
        }

        renderers.AddRange(GetComponentsInChildren<Renderer>(true));
    }

    public void FadeOut() => StartCoroutine(Fade(0f));
    public void FadeIn() => StartCoroutine(Fade(1f));

    IEnumerator Fade(float targetAlpha)
    {
        float elapsed = 0f;

        if (renderers.Count == 0)
        {
            yield break;
        }

        SetTransparentRendering(true);

        List<Material> materials = new List<Material>();
        List<Color> colors = new List<Color>();

        foreach (Renderer currentRenderer in renderers)
        {
            Material mat = currentRenderer.material;
            materials.Add(mat);
            colors.Add(mat.color);
        }

        float startAlpha = colors[0].a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            for (int i = 0; i < materials.Count; i++)
            {
                Color col = colors[i];
                col.a = Mathf.Lerp(startAlpha, targetAlpha, t);
                materials[i].color = col;
            }

            yield return null;
        }

        for (int i = 0; i < materials.Count; i++)
        {
            Color col = colors[i];
            col.a = targetAlpha;
            materials[i].color = col;
            materials[i].SetColor("_BaseColor", col);
            materials[i].SetColor("_Color", col);
        }

        SetTransparentRendering(targetAlpha < 1f);
    }

    void SetTransparentRendering(bool isTransparent)
    {
        foreach (Renderer currentRenderer in renderers)
        {
            foreach (Material material in currentRenderer.materials)
            {
                if (isTransparent)
                {
                    material.SetFloat("_Surface", 1f);
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetFloat("_ZWrite", 0f);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
                else
                {
                    material.SetFloat("_Surface", 0f);
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetFloat("_ZWrite", 1f);
                    material.renderQueue = -1;
                }
            }
        }
    }
}
