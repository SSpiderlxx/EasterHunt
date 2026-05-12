using UnityEngine;

// Very simple Fade Manager could be called by colliders ?
public class FadeObjectManager : MonoBehaviour
{
    [SerializeField] FadeObject[] fadeObjects;

    public void FadeAllObjectsIn()
    {
        foreach (FadeObject obj in fadeObjects)
        {
            obj.FadeIn();
        }
    }

    public void FadeAllObjectsOut()
    {
        foreach (FadeObject obj in fadeObjects)
        {
            obj.FadeOut();
        }
    }
}
