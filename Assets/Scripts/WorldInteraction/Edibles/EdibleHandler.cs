using UnityEngine;

public class EdibleHandler : MonoBehaviour
{
    private bool canInteract;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
