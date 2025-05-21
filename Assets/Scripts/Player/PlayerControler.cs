using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    private bool playerAction = false;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        GameEventsManager.instance.loreEvents.onImportantLoreEvent += ChangeAction;
    }

    void OnDisable()
    {
        GameEventsManager.instance.loreEvents.onImportantLoreEvent -= ChangeAction;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAction)
        {
            playerMovement.HandleMovement(Camera.main.transform);
            playerMovement.HandleJump();
        }
    }

    private void ChangeAction()
    {
        playerAction = !playerAction;
    }
}