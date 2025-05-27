using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private bool playerAction = false;
    public Transform playerBody;
    public PlayerMovement playerMovement;

    private float xRotation = 0f;

    private PlayerControls controls;
    private Vector2 lookInput;
    private readonly float mouseSensitivity = 0.5f;
    private readonly float gamepadSensitivity = 250f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controls = new PlayerControls();
        controls.Gameplay.Enable();

        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;

        if (playerMovement == null && playerBody != null)
        {
            playerMovement = playerBody.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement does not exist !");
            }
        }

        GameEventsManager.instance.loreEvents.onImportantLoreEvent += ChangeAction;
    }

    void OnDisable()
    {
        GameEventsManager.instance.loreEvents.onImportantLoreEvent -= ChangeAction;
        controls.Gameplay.Disable();
    }

    private void ChangeAction()
    {
        playerAction = !playerAction;

        if (playerAction) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (!playerAction)
        {
            float mouseX, mouseY;

            if (Mouse.current != null && Mouse.current.delta.IsActuated())
            {
                mouseX = lookInput.x * mouseSensitivity;
                mouseY = lookInput.y * mouseSensitivity;
            }
            else
            {
                mouseX = lookInput.x * gamepadSensitivity * Time.deltaTime;
                mouseY = lookInput.y * gamepadSensitivity * Time.deltaTime;
            }

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            if (playerMovement != null)
            {
                playerMovement.HandleMovement(transform);
            }
        }
    }
}
