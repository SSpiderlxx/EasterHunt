using UnityEngine;

public class AbilityCollectable : MonoBehaviour, IInteractable
{
    public Ability ability;

    public void Interact()
    {
        PlayerController.instance.ApplyAbility(ability);
        Destroy(gameObject);
    }
}
