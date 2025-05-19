using System;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("UI")] public TextMeshProUGUI interactionText;

    [Header("Movement")] private const float MoveSpeed = 5f;
    private Vector3 _moveDirection;

    [Header("Initialization")] private Rigidbody _rb;
    private Camera _camera;

    [Header("Interaction")] private const float InteractDistance = 2f;
    private RaycastHit _hit;
    private PickableController _pickableController;
    private MonsterController _monsterController;

    [Header("Camera")] [SerializeField] [Range(0, 10)]
    private float mouseSensitivity = 2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        interactionText.gameObject.SetActive(false);
        _pickableController = null;
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _hit, InteractDistance))
        {
            if (_hit.collider.TryGetComponent(out PickableController pickableController))
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press E to pick up";
                HandlePickup(pickableController);
            }
            else if (_hit.collider.TryGetComponent(out MonsterController monsterController) && _pickableController != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press E to give item";
                HandleItem(monsterController);
            }
            else
            {
                interactionText.gameObject.SetActive(false);
                if (Input.GetKeyDown(KeyCode.E) && _pickableController != null)
                {
                    _pickableController.Drop();
                    _pickableController = null;
                }
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E) && _pickableController != null)
            {
                _pickableController.Drop();
                _pickableController = null;
            }
        }
    }

    private void HandleItem(MonsterController monsterController)
    {
        _monsterController = monsterController;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _monsterController.Interact(_pickableController);
            interactionText.gameObject.SetActive(false);
            _pickableController = null;
        }
    }

    private void HandlePickup(PickableController pickableController)
    {
        _pickableController = pickableController;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _pickableController.Pickup(_camera.transform);
            interactionText.gameObject.SetActive(false);
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

        _rb.MovePosition(transform.position +
                         Time.deltaTime * MoveSpeed * (moveVertical * cameraForward + moveHorizontal * cameraRight));
    }
}
