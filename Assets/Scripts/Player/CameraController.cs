using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private bool playerAction = false;
    public float mouseSensitivity = 500f;
    public Transform playerBody;
    public PlayerMovement playerMovement;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (playerMovement == null && playerBody != null)
        {
            playerMovement = playerBody.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement does not exist !");
            }
        }
    }

    private void ChangeAction()
    {
        playerAction = !playerAction;

        if (playerAction == true) {
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (!playerAction) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            if (playerMovement != null)
            {
                playerMovement.HandleMovement(transform);
            }
            else
            {
                Debug.LogWarning("PlayerMovement does not exist !");
            }
        }
    }
}
