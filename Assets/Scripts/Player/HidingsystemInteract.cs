using UnityEngine;

public class Hidingsystem : MonoBehaviour, IInteractable
{

    public bool CanInteract = true;

    public void Interact()
    {
        if (CanInteract == true)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5))
            {
                if (hit.collider.CompareTag("Hide")) 
                {

                }
            }
        }
    }




}
