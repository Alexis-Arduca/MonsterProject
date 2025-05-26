using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerControler : MonoBehaviour
{
    private bool playerAction = false;
    private PlayerMovement playerMovement;
    public PlayerControls controls;

    void Start()
    {
        controls = new PlayerControls();
        controls.Gameplay.Enable();
        playerMovement = GetComponent<PlayerMovement>();

        GameEventsManager.instance.loreEvents.onImportantLoreEvent += ChangeAction;

        controls.Gameplay.Jump.performed += ctx => {
            if (!playerAction) playerMovement.HandleJump();
        };
        
        controls.Gameplay.Sprint.performed += ctx => playerMovement.HandleSprint();
        controls.Gameplay.Sprint.canceled += ctx => playerMovement.HandleSprint();
    }

    void OnDisable()
    {
        GameEventsManager.instance.loreEvents.onImportantLoreEvent -= ChangeAction;
    }

    void Update()
    {
        if (!playerAction)
        {
            playerMovement.HandleMovement(Camera.main.transform);
        }
    }

    private void ChangeAction()
    {
        playerAction = !playerAction;
    }
}
