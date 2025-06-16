using UnityEngine;
using UnityEngine.InputSystem;

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
    private MonsterController _monsterController;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;
        playerCamera = GetComponentInChildren<Camera>();
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
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out _hit, InteractDistance))
        {
            if (_hit.collider.TryGetComponent(out PickableController pickableController))
            {
                HandlePickup(pickableController);
            }
            else if (_hit.collider.TryGetComponent(out MonsterController monsterController) && _pickableController != null)
            {
                HandleItem(monsterController);
            }
            else
            {
                if (_pickableController != null)
                {
                    HandleDrop();
                }
            }
        }
        else
        {
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

    public void OnAction(InputValue value)
    {
        if (currentEdible != null && currentEdible.GetInteraction())
        {
            currentEdible.InteractWith();
        }
    }

    public void OnDebug(InputValue value)
    {
        BackOnSpawn();
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
        transform.position = new Vector3(-2.4f, 2.74f, 14.3f);
    }

    private void HandleDrop()
    {
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
            _pickableController = null;
        }
    }

    private void HandlePickup(PickableController pickableController)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _pickableController = pickableController;
            _pickableController.Pickup(playerCamera.transform);
        }
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
