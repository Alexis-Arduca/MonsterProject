using Unity.VisualScripting;
using UnityEngine;

public class Drink : EdibleHandler
{
    private PondController _pondController;
    public AudioClip[] clip;

    private void Start()
    {
        _pondController = GetComponentInParent<PondController>();
    }

    public override void InteractWith()
    {
        // Randomly select a sound clip to play
        int randomIndex = Random.Range(0, clip.Length);

        AudioSource.PlayClipAtPoint(clip[randomIndex], PlayerPosition);
        _pondController.LowerWaterLevel();
        GameEventsManager.instance.edibleEvents.OnDrink();
    }
}
