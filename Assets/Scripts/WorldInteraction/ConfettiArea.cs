using UnityEngine;

public class ConfettiArea : MonoBehaviour
{
    public ParticleSystem confettiPrefab;
    private Transform player;
    private bool playerInside = false;
    private Vector3 lastPlayerPosition;
    private float spawnCooldown = 0.2f;
    private float spawnTimer = 0f;

    void Start()
    {
        confettiPrefab.Stop();
    }

    void Update()
    {
        if (playerInside && player != null)
        {
            spawnTimer -= Time.deltaTime;

            float movement = (player.position - lastPlayerPosition).magnitude;
            if (movement > 0.01f && spawnTimer <= 0f)
            {
                Vector3 playerPos = player.position;
                playerPos.y = 0.1f;

                ParticleSystem confetti = Instantiate(confettiPrefab, playerPos, Quaternion.identity);
                confetti.Play();
                Destroy(confetti.gameObject, 10f);

                spawnTimer = spawnCooldown;
            }

            lastPlayerPosition = player.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            lastPlayerPosition = player.position;
            playerInside = true;
            spawnTimer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }
}
