using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 movement;
    private Rigidbody rb;
    private bool isRuning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMovement(Transform cameraTransform)
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRuning = true;
            moveSpeed = 10;
        } else if (isRuning == true) {
            isRuning = false;
            moveSpeed = 5;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        movement = (forward * vertical + right * horizontal).normalized * moveSpeed;

        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }
}
