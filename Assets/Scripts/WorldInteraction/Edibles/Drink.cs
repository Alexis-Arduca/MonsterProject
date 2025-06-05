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
        // AudioSource.PlayClipAtPoint(clip, transform.position);
        if (_pondController.IsWaterLevelEmpty())
        {
            Debug.Log("The pond is empty, you can't drink anymore.");
        }
        else
        {
            _pondController.LowerWaterLevel();
        }
        GameEventsManager.instance.edibleEvents.OnDrink();
    }
}
