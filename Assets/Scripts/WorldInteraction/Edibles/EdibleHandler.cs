using System;
using UnityEngine;

public class EdibleHandler : MonoBehaviour
{
    private bool canInteract;
    public GameObject imageInteraction;

    void Start()
    {
        canInteract = false;
    }

    public virtual void InteractWith()
    {

    }

    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }

    public bool GetInteraction()
    {
        return canInteract;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            imageInteraction.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            imageInteraction.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
            imageInteraction.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            imageInteraction.SetActive(false);
        }
    }
}
