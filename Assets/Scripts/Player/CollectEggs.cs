using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class CollectEggs : MonoBehaviour, IInteractable
{
    public GameObject collectText;

    public AudioSource collectSound;

    private GameObject egg;

    private bool inReach;

    void Start()
    {
        collectText.SetActive(false);

        inReach = false;

        egg = this.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            collectText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            collectText.SetActive(false);
        }
    }

    public void Interact()
    {
        collectSound.Play();
        collectText.SetActive(false);
        egg.SetActive(false);
    }

    //void Update()
    //{
    //if(inReach && Input.GetButtonDown("Interactor"))
    //    {
    //        collectSound.Play();
    //    collectText.SetActive(false);
    //      egg.SetActive(false);
    //   }

    //}

}
