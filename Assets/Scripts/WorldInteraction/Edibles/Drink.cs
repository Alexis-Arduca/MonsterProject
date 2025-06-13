using UnityEngine;
using Random = UnityEngine.Random;

public class Drink : EdibleHandler
{
    private PondController _pondController;
    public AudioClip[] clip;
    private static Vector3 PlayerPosition => GameObject.FindGameObjectWithTag("Player").transform.position;

    private void Start()
    {
        _pondController = GetComponentInParent<PondController>();
    }

    public override void InteractWith()
    {
        if (_pondController.IsWaterLevelEmpty())
        {
            Debug.LogWarning("Water level is empty. Cannot drink.");
        }
        else
        {
            // Randomly select a sound clip to play
            int randomIndex = Random.Range(0, clip.Length);

            AudioSource.PlayClipAtPoint(clip[randomIndex], PlayerPosition);
            _pondController.LowerWaterLevel();
            GameEventsManager.instance.edibleEvents.OnDrink();
        }
    }
}
