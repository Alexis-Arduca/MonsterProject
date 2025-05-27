using UnityEngine;

public class Drink : EdibleHandler
{
    public AudioClip clip;

    public override void InteractWith()
    {
        // AudioSource.PlayClipAtPoint(clip, transform.position);
        GameEventsManager.instance.edibleEvents.OnDrink();
    }
}
