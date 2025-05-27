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

    [HideInInspector] public bool isOnIce = false;

    private Vector3 movement;
    private Rigidbody rb;
    private bool isRuning = false;

    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float targetDrag = (isGrounded || isOnIce) ? (isOnIce ? 0.5f : 3f) : 0f;
        rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, Time.deltaTime * 3f);
    }

    public void HandleMovement(Transform cameraTransform)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        movement = (forward * vertical + right * horizontal).normalized * moveSpeed;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(movement.x, currentVelocity.y, movement.z);
        float inertia = isOnIce ? 0.05f : 0.2f;

        rb.linearVelocity = Vector3.Lerp(currentVelocity, targetVelocity, inertia);
    }

    public void HandleSprint()
    {
        isRuning = !isRuning;

        if (moveSpeed == 5)
        {
            moveSpeed = 10;
        }
        else
        {
            moveSpeed = 5;
        }
    }

    public void HandleJump()
    {
        if (isGrounded)
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
