using TMPro;
using UnityEngine;

// <summary>
// Attached to the Player Prefab.
// This script is responsible for controlling the player's movement and interactions.
// It handles the camera rotation, player movement, and interactions with pickable items and monsters.
// </summary>

public class PlayerController : MonoBehaviour
{
    [Header("UI")] public TextMeshProUGUI interactionText;
    [Header("Audio")] public AudioClip[] burpSounds;

    [Header("Camera")] [SerializeField] [Range(0, 10)]
    private float mouseSensitivity = 2f;

    private const float MoveSpeed = 5f;
    private const float InteractDistance = 2f;

    private Rigidbody _rb;
    private Camera _camera;
    private Vector3 _moveDirection;
    private RaycastHit _hit;
    private AudioSource _audioSource;

    private PickableController _pickableController;
    private MonsterController _monsterController;
    private WallPondController _wallPondController;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
        _audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        interactionText.gameObject.SetActive(false);
        _pickableController = null;
        _monsterController = null;
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
                HandlePickup(pickableController);
            }
            else if (_hit.collider.TryGetComponent(out MonsterController monsterController) && _pickableController != null)
            {
                interactionText.gameObject.SetActive(false);
                HandleItem(monsterController);
            }
            else if (_hit.collider.TryGetComponent(out WallPondController wallPondController) && _pickableController == null)
            {
                HandleDrinkPond(wallPondController);
            }
            else
            {
                interactionText.gameObject.SetActive(false);
                if (_pickableController != null)
                {
                    HandleDrop();
                }
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
            if (_monsterController != null)
            {
                _monsterController.thoughtBubble.HideText();
                _monsterController.thoughtBubble.ShowItem();
            }

            if (_pickableController != null)
            {
                HandleDrop();
            }
        }
    }

    private void HandleDrinkPond(WallPondController wallPondController)
    {
        _wallPondController = wallPondController;
        switch (_wallPondController.pondController.IsWaterLevelEmpty())
        {
            case true:
                interactionText.gameObject.SetActive(true);
                interactionText.text = "The pond is empty.";
                break;
            case false:
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press E to drink water";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _wallPondController.Drink();
                    RandomBurp();
                }
                break;
        }
    }

    // <summary>
    // Plays a random burp sound from the audio source.
    // Modify this method when using other audio libraries or systems.
    // </summary>

    private void RandomBurp()
    {
        if (burpSounds.Length == 0) return;

        int randomIndex = Random.Range(0, burpSounds.Length);
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(burpSounds[randomIndex]);
        }
    }

    private void HandleDrop()
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = "Press E to drop item";
        if (Input.GetKeyDown(KeyCode.E))
        {
            _pickableController.Drop();
            _pickableController = null;
        }
    }

    private void HandleItem(MonsterController monsterController)
    {
        _monsterController = monsterController;
        if (Vector3.Distance(_hit.transform.position, transform.position) <= InteractDistance + .6f)
        {
            _monsterController.thoughtBubble.HideItem();
            _monsterController.thoughtBubble.ShowText();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _monsterController.Interact(_pickableController);
            interactionText.gameObject.SetActive(false);
            _pickableController = null;
        }
    }

    private void HandlePickup(PickableController pickableController)
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = "Press E to pick up";
        if (Input.GetKeyDown(KeyCode.E))
        {
            _pickableController = pickableController;
            _pickableController.Pickup(_camera.transform);
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
