using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    private int appleCount;
    public TMP_Text appleCountUI;

    private bool playerAction = false;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        appleCount = 0;

        UpdateAppleUI();

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
    private void UpdateAppleUI()
    {
        appleCountUI.text = $"{appleCount}";
    }

    public int GetApples()
    {
        return appleCount;
    }

    private void AppleCollected(int value)
    {
        appleCount += value;
        UpdateAppleUI();
    }

    private void AppleUsed(int value)
    {
        appleCount -= value;
        UpdateAppleUI();
    }
}