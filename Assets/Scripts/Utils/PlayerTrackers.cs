using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerTracker : MonoBehaviour
{
    public int playerCount;
    public GameObject startMessage;
    public GameObject cameraPresentation;

    void Start()
    {
        playerCount = 0;
        startMessage.SetActive(true);
        cameraPresentation.SetActive(true);
    }

    void Update()
    {
        playerCount = PlayerInput.all.Count;

        if (playerCount == 0)
        {
            startMessage.SetActive(true);
            cameraPresentation.SetActive(true);
        }
        else
        {
            startMessage.SetActive(false);
            cameraPresentation.SetActive(false);
        }
    }
}
