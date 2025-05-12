using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool playerAction = false;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAction) {
            playerMovement.HandleMovement(Camera.main.transform);
        }
    }

    private void ChangeAction()
    {
        playerAction = !playerAction;
    }
}