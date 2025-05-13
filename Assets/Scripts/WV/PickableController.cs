using System;
using UnityEngine;
using UnityEngine.UI;

public class PickableController : MonoBehaviour
{
    private Rigidbody _rb;
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
        transform.SetParent(cameraT);
        transform.localPosition = new Vector3(.5f, 0, 1);
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

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
