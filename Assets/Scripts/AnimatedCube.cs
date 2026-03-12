using UnityEngine;

public class AnimatedCube : MonoBehaviour, IInteractable
{
    [SerializeField] Animation anim;

    public void Interact()
    {
        anim.Play();
    }
}
