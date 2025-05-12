using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private const float MoveSpeed = 5f;
    private Vector3 _moveDirection;

    [Header("Initialization")]
    private Rigidbody _rb;
    private Camera _camera;

    [Header("Pickup")]
    private const float PickupDistance = 2f;
    private bool _isHoldingItem;
    private PickableController _pickableController;

    [Header("Camera")]
    [SerializeField] [Range(0, 10)] private float mouseSensitivity = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.E))
            HandleAction();
    }

    private void HandleAction()
    {
        if (!_isHoldingItem)
        {
            HandlePickup();
        }
        else
        {
            HandleItem();
        }
    }

    private void HandleItem()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, PickupDistance))
        {
            if (hit.collider.TryGetComponent(out MonsterController monsterController))
            {
                monsterController.Interact(_pickableController.name);
            }
            else
            {
                _pickableController.Drop();
            }
            _isHoldingItem = false;
        }
    }

    private void HandlePickup()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, PickupDistance))
        {
            if (hit.collider.TryGetComponent(out _pickableController))
            {
                _pickableController.Pickup(_camera.transform);
                _isHoldingItem = true;
            }
        }
    }

    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 cameraRotation = _camera.transform.localEulerAngles;

        cameraRotation.x -= mouseY * mouseSensitivity;
        cameraRotation.y += mouseX * mouseSensitivity;
        _camera.transform.localEulerAngles = cameraRotation;
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _moveDirection.Normalize();

        _rb.MovePosition(transform.position + Time.deltaTime * MoveSpeed * _moveDirection);
    }
}
