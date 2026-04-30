using UnityEngine;

public class AbilityCollectable : MonoBehaviour, IInteractable
{
    [SerializeField] private Ability ability;

    public void Interact()
    {
        if(ability == null)
        {
            Debug.LogWarning("No ability assigned to this collectable.");
            return;
        }
        if(PlayerController.instance == null)
        {
            Debug.LogError("PlayerController instance not found.");
            return;
        }
        PlayerController.instance.ApplyAbility(ability);
        Destroy(gameObject);
    }
}
