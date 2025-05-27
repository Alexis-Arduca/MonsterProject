using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;
    private float baseMoveSpeed;
    private float effectSpeedMultiplier = 1f;
    private bool isSprinting = false;

    [Header("Jump")]
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;

    [HideInInspector] public bool isOnIce = false;

    private Vector3 movement;
    private Rigidbody rb;

    void Start()
    {
        baseMoveSpeed = moveSpeed;
        isGrounded = true;
        rb = GetComponent<Rigidbody>();

        GameEventsManager.instance.edibleEvents.onEat += EatInteraction;
        GameEventsManager.instance.edibleEvents.onDrink += DrinkInteraction;
        GameEventsManager.instance.edibleEvents.onLick += LickInteraction;
    }

    void Update()
    {
        float targetDrag = (isGrounded || isOnIce) ? (isOnIce ? 0.5f : 3f) : 0f;
        rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, Time.deltaTime * 3f);

        moveSpeed = baseMoveSpeed * effectSpeedMultiplier * (isSprinting ? 2f : 1f);
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

    public void EatInteraction()
    {
        effectSpeedMultiplier = 2f;
        StartCoroutine(ResetSpeedEffect(10f));
    }

    public void DrinkInteraction()
    {
        jumpForce = 3.0f;
        StartCoroutine(ResetJumpEffect(5f));
    }

    public void LickInteraction()
    {
        effectSpeedMultiplier = 0.5f;
        jumpForce = 1.0f;
        StartCoroutine(ResetLickEffect(5f));
    }

    private IEnumerator ResetSpeedEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        effectSpeedMultiplier = 1f;
    }

    private IEnumerator ResetJumpEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = 2.0f;
    }

    private IEnumerator ResetLickEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        effectSpeedMultiplier = 1f;
        jumpForce = 2.0f;
    }

    public void HandleSprint()
    {
        isSprinting = !isSprinting;
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
