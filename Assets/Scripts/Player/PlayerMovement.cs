using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;

    [Header("Jump")]
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;

    private Vector3 movement;
    private Rigidbody rb;
    private bool isRuning = false;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
    }

    public void HandleMovement(Transform cameraTransform)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRuning = true;
            moveSpeed = 10;
        }
        else if (isRuning == true)
        {
            isRuning = false;
            moveSpeed = 5;
        }

        forward.Normalize();
        right.Normalize();

        movement = (forward * vertical + right * horizontal).normalized * moveSpeed;

        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
    }

    public void HandleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }
    
    void OnCollisionExit()
    {
        isGrounded = false;
    }
}
