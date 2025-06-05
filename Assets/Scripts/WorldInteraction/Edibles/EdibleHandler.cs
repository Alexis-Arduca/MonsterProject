using UnityEngine;

public class EdibleHandler : MonoBehaviour
{
    private bool canInteract;
    protected Vector3 PlayerPosition;

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
            PlayerPosition = collision.transform.position;
            canInteract = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPosition = Vector3.zero; // Reset player position
            canInteract = false;
        }
    }
}
