using UnityEngine;
using UnityEngine.UI;

// <summary>
// Attached to the Pickable Prefab.
// This script is responsible for controlling the pickable items in the game.
// It handles the pickup, drop, and destroy actions for the items.
// </summary>

public class PickableController : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Pickup")]
    [Tooltip("The icon that represents this item in the UI.")]
    public Image icon;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Pickup(Transform cameraT)
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
        transform.SetParent(cameraT, true);
        transform.localPosition = new Vector3(.5f, 0, 1);
    }

    public void Drop()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        transform.SetParent(null, true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
