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
    public bool isOnIce = false;
    private Vector3 movement;
    private Rigidbody rb;

    [Header("ReadOnly Value")]
    private readonly float minDrag = 0.5f;
    private readonly float maxDrag = 3f;
    private readonly float minSpeedMultiplier = 1f;
    private readonly float maxSpeedMultiplier = 2f;
    private readonly float minInertiaOnIce = 0.01f;
    private readonly float maxIntertiaOnIce = 0.2f;

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
        float targetDrag = (isGrounded || isOnIce) ? (isOnIce ? minDrag : maxDrag) : 0f;
        rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, Time.deltaTime * maxDrag);

        moveSpeed = baseMoveSpeed * effectSpeedMultiplier * (isSprinting ? maxSpeedMultiplier : minSpeedMultiplier);
    }

    /// <summary>
    /// Handle player movement based on some parameters
    /// </summary>
    /// <param name="cameraTransform"></param>
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
        float inertia = isOnIce ? minInertiaOnIce : maxIntertiaOnIce;

        rb.linearVelocity = Vector3.Lerp(currentVelocity, targetVelocity, inertia);
    }

    /// <summary>
    /// Handle Eat interaction and buff (speed multiplier)
    /// </summary>
    public void EatInteraction()
    {
        effectSpeedMultiplier = 2f;
        StartCoroutine(ResetSpeedEffect(10f));
    }

    /// <summary>
    /// Handle Drink interaction and buff (jump upgrade)
    /// </summary>
    public void DrinkInteraction()
    {
        jumpForce = 3.0f;
        StartCoroutine(ResetJumpEffect(5f));
    }

    /// <summary>
    /// Handle Lick interaction and nerf (slow the player and reduce jump)
    /// </summary>
    public void LickInteraction()
    {
        effectSpeedMultiplier = 0.5f;
        jumpForce = 1.0f;
        StartCoroutine(ResetLickEffect(5f));
    }

    /// <summary>
    /// Next three functions are just use to reset the stats after a moment (usage of Coroutine)
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Just handle the sprint (simple to understand)
    /// </summary>
    public void HandleSprint()
    {
        isSprinting = !isSprinting;
    }

    /// <summary>
    /// Just handle the jump
    /// </summary>
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
