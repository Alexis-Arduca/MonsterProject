using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool playerAction = false;
    private int appleCount;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        appleCount = 0;

        GameEventsManager.instance.appleEvents.onAppleCollected += AppleCollected;
        GameEventsManager.instance.appleEvents.onAppleUsed += AppleUsed;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.appleEvents.onAppleCollected -= AppleCollected;
        GameEventsManager.instance.appleEvents.onAppleUsed -= AppleUsed;
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

    /// <summary>
    /// Apples function
    /// </summary>
    /// <returns></returns>
    public int GetApples()
    {
        return appleCount;
    }

    private void AppleCollected(int value)
    {
        appleCount += value;
    }

    private void AppleUsed(int value)
    {
        appleCount -= value;
    }
}