using UnityEngine;

public class Lick : EdibleHandler
{
    public AudioClip clip;

    public override void InteractWith()
    {
        // AudioSource.PlayClipAtPoint(clip, transform.position);
        GameEventsManager.instance.edibleEvents.OnLick();
    }
}
