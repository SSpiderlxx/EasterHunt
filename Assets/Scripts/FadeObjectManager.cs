using UnityEngine;

// Very simple Fade Manager could be called by colliders ?
public class FadeObjectManager : MonoBehaviour
{
    [SerializeField] FadeObject[] fadeObjects;

    private bool objectsAreFaded;

    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other) || objectsAreFaded)
        {
            return;
        }

        FadeAllObjectsOut();
        objectsAreFaded = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other) || !objectsAreFaded)
        {
            return;
        }

        FadeAllObjectsIn();
        objectsAreFaded = false;
    }

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

    private bool IsPlayer(Collider other)
    {
        return other.GetComponentInParent<PlayerController>() != null;
    }
}
