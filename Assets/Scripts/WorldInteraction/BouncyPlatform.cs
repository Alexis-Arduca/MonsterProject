using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] private float initialBounceForce = 10f;
    [SerializeField] private float bounceReduction = 0.8f;
    [SerializeField] private int maxBounces = 5;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Bouncer bouncer = collision.gameObject.GetComponent<Bouncer>();
            if (bouncer == null)
            {
                bouncer = collision.gameObject.AddComponent<Bouncer>();
                bouncer.currentBounceForce = initialBounceForce;
                bouncer.bounceReduction = bounceReduction;
                bouncer.maxBounces = maxBounces;
            }

            if (bouncer.bounceCount < bouncer.maxBounces)
            {
                Vector3 velocity = rb.linearVelocity;
                velocity.y = 0;
                rb.linearVelocity = velocity;

                rb.AddForce(Vector3.up * bouncer.currentBounceForce, ForceMode.Impulse);

                bouncer.currentBounceForce *= bouncer.bounceReduction;
                bouncer.bounceCount++;

                if (bouncer.bounceCount >= bouncer.maxBounces)
                {
                    bouncer.currentBounceForce = initialBounceForce;
                    bouncer.bounceCount = 0;
                }
            }
        }
    }
}

public class Bouncer : MonoBehaviour
{
    public float currentBounceForce;
    public float bounceReduction;
    public int maxBounces;
    public int bounceCount = 0;
}
