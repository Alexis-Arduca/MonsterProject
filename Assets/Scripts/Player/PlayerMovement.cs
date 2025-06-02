using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    private float baseMoveSpeed;
    private float effectSpeedMultiplier = 1f;
    private bool isSprinting = false;

    [Header("Jump")]
    public float jumpForce = 10.0f;
    public float fallMultiplier = 2.5f;
    private readonly float fallReducer = 1f;
    public bool isGrounded;
    public bool isOnIce = false;
    private Vector3 movement;
    private Rigidbody rb;
    public AudioClip jumpClip;

    [Header("Boost")]
    private float jumpBaseValue;
    public float jumpNerf = 7.0f;
    public float jumpUpgrade = 13.0f;
    private float speedMultiplierBaseValue;
    public float speedMultiplierNerf = 0.5f;
    public float speedMultiplierUpgrade = 2f;

    [Header("ReadOnly Value")]
    private readonly float minDrag = 0.5f;
    private readonly float maxDrag = 3f;
    private readonly float minSpeedMultiplier = 1f;
    private readonly float maxSpeedMultiplier = 2f;
    private readonly float minInertiaOnIce = 0.01f;
    private readonly float maxInertiaOnIce = 0.2f;

    void Start()
    {
        jumpBaseValue = jumpForce;
        speedMultiplierBaseValue = effectSpeedMultiplier;

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

        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - fallReducer) * Time.deltaTime;
        }
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
        Vector3 targetVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        float inertia = isOnIce ? minInertiaOnIce : maxInertiaOnIce;

        rb.linearVelocity = Vector3.Lerp(currentVelocity, targetVelocity, inertia);
    }

    public void EatInteraction()
    {
        effectSpeedMultiplier = speedMultiplierUpgrade;
        StartCoroutine(ResetSpeedEffect(10f));
    }

    public void DrinkInteraction()
    {
        jumpForce = jumpUpgrade;
        StartCoroutine(ResetJumpEffect(5f));
    }

    public void LickInteraction()
    {
        effectSpeedMultiplier = speedMultiplierNerf;
        jumpForce = jumpNerf;
        StartCoroutine(ResetLickEffect(5f));
    }

    private IEnumerator ResetSpeedEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        effectSpeedMultiplier = speedMultiplierBaseValue;
    }

    private IEnumerator ResetJumpEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = jumpBaseValue;
    }

    private IEnumerator ResetLickEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        effectSpeedMultiplier = speedMultiplierBaseValue;
        jumpForce = jumpBaseValue;
    }

    public void HandleSprint()
    {
        isSprinting = !isSprinting;
    }

    public void HandleJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            // AudioSource.PlayClipAtPoint(jumpClip, transform.position);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
