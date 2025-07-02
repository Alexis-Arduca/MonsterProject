using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the Pickable Prefab.
/// This script is responsible for controlling the pickable items in the game.
/// It handles the pickup, drop, and destroy actions for the items.
/// </summary>
public class PickableController : MonoBehaviour
{
    [Header("Pickup")]
    [Tooltip("The icon that represents this item in the UI.")]
    public Image icon;

    [Tooltip("The factor by which the item is scaled down when held.")]
    [Range(0.1f, 1f)]
    public float scaleFactor = 0.1f;

    private Vector3 originalScale;
    private Transform originalParent;

    private void Start()
    {
        originalScale = transform.localScale;
        originalParent = transform.parent;
    }

    public void Pickup(Transform cameraT, GameObject player)
    {
        transform.SetParent(cameraT, true);
        transform.localPosition = new Vector3(0.5f, 0, 1);
        transform.localRotation = Quaternion.identity;

        // this.transform.GetChild(1).localScale = originalScale * scaleFactor;

        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        this.gameObject.GetComponent<Collectible>().IsPickup();

        GameEventsManager.instance.trailEvents.OnItemPickup(GetComponent<Collectible>().GetCode(), player);
    }

    public void Drop()
    {
        transform.SetParent(null, true);
        transform.localScale = originalScale;

        this.gameObject.GetComponent<Collectible>().IsPickup();

        GameEventsManager.instance.trailEvents.OnItemRelease(GetComponent<Collectible>().GetCode());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
