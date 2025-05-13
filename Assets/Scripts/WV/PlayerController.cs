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
        if (!_pickableController)
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
                monsterController.Interact(_pickableController);
            }
        }
        else
        {
            _pickableController.Drop();
            _pickableController = null;
        }
    }

    private void HandlePickup()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, PickupDistance))
        {
            if (hit.collider.TryGetComponent(out _pickableController))
            {
                _pickableController.Pickup(_camera.transform);
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
        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;

        _rb.MovePosition(transform.position + Time.deltaTime * MoveSpeed * (moveVertical * cameraForward + moveHorizontal * cameraRight));
    }
}
