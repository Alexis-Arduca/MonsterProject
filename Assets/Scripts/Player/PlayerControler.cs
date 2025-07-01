using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerControler : MonoBehaviour
{
    private bool playerAction = false;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private EdibleHandler currentEdible;
    private Camera playerCamera;
    public int playerId;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    public float mouseSensitivity = 0.5f;
    public float gamepadSensitivity = 300f;

    private const float InteractDistance = 2f;
    private RaycastHit _hit;
    private PickableController _pickableController;
    private readonly float cameraMaxRotationY = 50f;
    private readonly float cameraMinRotationY = -50f;
    private readonly Vector3 spawnPosition = new Vector3(-2.4f, 2.74f, 14.3f);
    private PauseManager pauseManager;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;
        playerCamera = GetComponentInChildren<Camera>();
        pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        GameEventsManager.instance.loreEvents.onImportantLoreEvent += ChangeAction;
        GameEventsManager.instance.pauseEvents.onPauseButtonPressed += ChangeAction;
    }

    void OnDisable()
    {
        GameEventsManager.instance.loreEvents.onImportantLoreEvent -= ChangeAction;
        GameEventsManager.instance.pauseEvents.onPauseButtonPressed -= ChangeAction;
    }

    void Update()
    {
        if (!playerAction && playerCamera != null)
        {
            playerMovement.HandleMovement(playerCamera.transform, moveInput);

            float mouseX, mouseY;
            if (playerInput.currentControlScheme == "Keyboard")
            {
                Cursor.visible = false;
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                mouseX = mouseDelta.x * mouseSensitivity;
                mouseY = mouseDelta.y * mouseSensitivity;
            }
            else
            {
                mouseX = lookInput.x * gamepadSensitivity * Time.deltaTime;
                mouseY = lookInput.y * gamepadSensitivity * Time.deltaTime;
            }
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, cameraMinRotationY, cameraMaxRotationY);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out _hit, InteractDistance))
        {
            //
        }
    }

    public void OnAction(InputValue value)
    {
        if (currentEdible != null && currentEdible.GetInteraction())
        {
            currentEdible.InteractWith();
            return;
        }

        if (_hit.collider != null)
        {
            if (_hit.collider.TryGetComponent(out PickableController pickableController))
            {
                HandlePickup(pickableController);
            }
            else if (_pickableController != null)
            {
                HandleDrop();
            }
        }
        else if (_pickableController != null)
        {
            HandleDrop();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!playerAction)
        {
            playerMovement.HandleJump();
        }
    }

    public void OnSprint(InputValue value)
    {
        bool isSprinting = value.Get<float>() > 0.5f;
        playerMovement.HandleSprint(isSprinting);
    }

    public void OnDebug(InputValue value)
    {
        if (pauseManager.GetOnPause())
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
        else
        {
            BackOnSpawn();
        }
    }

    public void OnPause(InputValue value)
    {
        GameEventsManager.instance.pauseEvents.OnPauseButtonPressed();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void ChangeAction()
    {
        playerAction = !playerAction;
    }

    private void BackOnSpawn()
    {
        transform.position = spawnPosition;
    }

    private void HandlePickup(PickableController pickableController)
    {
        _pickableController = pickableController;
        _pickableController.Pickup(playerCamera.transform, this.gameObject);
    }

    private void HandleDrop()
    {
        _pickableController.Drop();
        _pickableController = null;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<EdibleHandler>(out var edible))
        {
            currentEdible = edible;
            edible.SetCanInteract(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EdibleHandler>(out var edible))
        {
            currentEdible = edible;
            edible.SetCanInteract(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent<EdibleHandler>(out var edible) && edible == currentEdible)
        {
            edible.SetCanInteract(false);
            currentEdible = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<EdibleHandler>(out var edible) && edible == currentEdible)
        {
            edible.SetCanInteract(false);
            currentEdible = null;
        }
    }
}
