using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerControler : MonoBehaviour
{
    private bool playerAction = false;
    private PlayerMovement playerMovement;
    public PlayerControls controls;
    private EdibleHandler currentEdible;
    public int playerId;

    void Start()
    {
        controls = new PlayerControls();
        controls.Gameplay.Enable();
        playerMovement = GetComponent<PlayerMovement>();

        GameEventsManager.instance.loreEvents.onImportantLoreEvent += ChangeAction;
        GameEventsManager.instance.pauseEvents.onPauseButtonPressed += ChangeAction;

        controls.Gameplay.Jump.performed += ctx => {
            if (!playerAction) playerMovement.HandleJump();
        };
        
        controls.Gameplay.Sprint.performed += ctx => playerMovement.HandleSprint();
        controls.Gameplay.Sprint.canceled += ctx => playerMovement.HandleSprint();
        controls.Gameplay.Action.performed += ctx => {
            if (currentEdible != null && currentEdible.GetInteraction()) currentEdible.InteractWith();
        };
        controls.Gameplay.Debug.performed += ctx => BackOnSpawn();
        controls.Gameplay.Pause.performed += ctx => GameEventsManager.instance.pauseEvents.OnPauseButtonPressed();
    }

    void OnDisable()
    {
        GameEventsManager.instance.loreEvents.onImportantLoreEvent -= ChangeAction;
        GameEventsManager.instance.pauseEvents.onPauseButtonPressed -= ChangeAction;
    }

    void Update()
    {
        if (!playerAction)
        {
            playerMovement.HandleMovement(Camera.main.transform);
        }
    }

    public void ChangeAction()
    {
        playerAction = !playerAction;
    }

    private void BackOnSpawn()
    {
        this.transform.position = new Vector3(-2.4f, 2.74f, 14.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EdibleHandler>(out var edible))
        {
            currentEdible = edible;
            edible.SetCanInteract(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EdibleHandler>(out var edible) && edible == currentEdible)
        {
            edible.SetCanInteract(false);
            currentEdible = null;
        }
    }
}
