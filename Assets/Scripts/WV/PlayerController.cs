using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    private float _moveSpeed = 5f;
    private float _jumpForce = 5f;
    private bool _isGrounded;
    private Camera _camera;
    private GameObject _pickable;
    private Rigidbody _pickableRb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        HandleJump();
        if (Input.GetKeyDown(KeyCode.E))
            HandlePickup();
    }

    private void HandlePickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                _pickable = hit.collider.gameObject;
                _pickableRb = _pickable.GetComponent<Rigidbody>();
                _pickableRb.isKinematic = true; // Disable physics
                _pickable.transform.SetParent(_camera.transform);
                _pickable.transform.localPosition = transform.position + _camera.transform.forward * 2f; // Position in front of the camera
            }
        }
    }

    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 cameraRotation = _camera.transform.localEulerAngles;

        cameraRotation.x -= mouseY;
        cameraRotation.y += mouseX;
        _camera.transform.localEulerAngles = cameraRotation;
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _moveDirection.Normalize();

        _rb.MovePosition(transform.position + Time.deltaTime * _moveSpeed * _moveDirection);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }
}
