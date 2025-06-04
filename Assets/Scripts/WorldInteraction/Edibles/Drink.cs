using Unity.VisualScripting;
using UnityEngine;

public class Drink : EdibleHandler
{
    public AudioClip[] clip;

    public override void InteractWith()
    {
        // AudioSource.PlayClipAtPoint(clip, transform.position);
        Debug.Log("Drinking from the pond");
        GameEventsManager.instance.edibleEvents.OnDrink();
    }
}
