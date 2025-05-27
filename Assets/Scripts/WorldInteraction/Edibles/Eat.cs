using UnityEngine;

public class Eat : EdibleHandler
{
    public AudioClip clip;

    public override void InteractWith()
    {
        // AudioSource.PlayClipAtPoint(clip, transform.position);
        GameEventsManager.instance.edibleEvents.OnEat();
    }
}
