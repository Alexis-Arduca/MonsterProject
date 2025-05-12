using UnityEngine;

public class PickableController : MonoBehaviour
{
    private Rigidbody _rb;

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
        transform.SetParent(cameraT);
        transform.localPosition = new Vector3(1, 0, 2);
    }

    public void Drop()
    {
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        transform.SetParent(null);
    }
}
