using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float baseSpeed = 5f;
    public float sprintMultiplier = 2f;
    public bool isSprinting = false;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public bool isGrounded = true;
    public bool isOnIce = false;

    [Header("Effects")]
    public float jumpNerf = 7f;
    public float jumpUpgrade = 13f;
    public float speedMultiplierNerf = 0.5f;
    public float speedMultiplierUpgrade = 2f;

    private Rigidbody rb;
    private float speedMultiplier = 1f;
    private float jumpBase;
    private float speedBase;

    private float minDrag = 0.5f;
    private float maxDrag = 3f;
    private float minInertia = 0.01f;
    private float maxInertia = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpBase = jumpForce;
        speedBase = 1f;

        GameEventsManager.instance.edibleEvents.onEat += OnEat;
        GameEventsManager.instance.edibleEvents.onDrink += OnDrink;
        GameEventsManager.instance.edibleEvents.onLick += OnLick;
    }

    private void Update()
    {
        float targetDrag = (isGrounded || isOnIce) ? (isOnIce ? minDrag : maxDrag) : 0f;
        rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, Time.deltaTime * 10f);

        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
    }

    public void HandleMovement(Transform cameraTransform)
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 rawForward = cameraTransform.forward;
        Vector3 forward = Vector3.ProjectOnPlane(rawForward, Vector3.up);

        if (forward.magnitude < 0.1f)
        {
            forward = transform.forward;
        }

        forward.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;


        Vector3 direction = (forward * v + right * h).normalized;
        float targetSpeed = baseSpeed * speedMultiplier * (isSprinting ? sprintMultiplier : 1f);
        Vector3 targetVelocity = direction * targetSpeed;
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 finalVelocity = new Vector3(targetVelocity.x, currentVelocity.y, targetVelocity.z);

        float inertia = isOnIce ? minInertia : maxInertia;
        rb.linearVelocity = Vector3.Lerp(currentVelocity, finalVelocity, inertia);
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
        }
    }

    public void OnEat()
    {
        speedMultiplier = speedMultiplierUpgrade;
        StartCoroutine(ResetSpeedEffect(10f));
    }

    public void OnDrink()
    {
        jumpForce = jumpUpgrade;
        StartCoroutine(ResetJumpEffect(5f));
    }

    public void OnLick()
    {
        speedMultiplier = speedMultiplierNerf;
        jumpForce = jumpNerf;
        StartCoroutine(ResetLickEffect(5f));
    }

    private IEnumerator ResetSpeedEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedMultiplier = speedBase;
    }

    private IEnumerator ResetJumpEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = jumpBase;
    }

    private IEnumerator ResetLickEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        speedMultiplier = speedBase;
        jumpForce = jumpBase;
    }

    private void OnCollisionStay(Collision collision)
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

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
