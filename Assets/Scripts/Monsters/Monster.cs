using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class Monster : MonoBehaviour
{
    public enum ElementType { Null, Fire, Ice, Electric, Psychic }
    public enum State { Patrolling, Following, Siting }

    [Header("Description")]
    [SerializeField] protected string monsterName;
    [SerializeField] protected string description;

    [Header("Parameters")]
    [SerializeField] protected int code;
    [SerializeField] protected ElementType element = ElementType.Null;
    [SerializeField] protected List<ElementType> weakness = new List<ElementType>();
    [SerializeField] protected List<ElementType> immunity = new List<ElementType>();
    [SerializeField] protected bool isFriendly = false;
    [SerializeField] protected BiomesTemplate.BiomeType spawnBiome;
    [SerializeField] protected State currentState;
    private Animator monsterAnimator;
    protected Rigidbody rb;
    private static readonly Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    [Header("Patrol")]
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float patrolChangeInterval = 5f;
    [SerializeField] protected float maxPatrolDistance = 15f;
    [SerializeField] protected float pauseDurationMin = 3f;
    [SerializeField] protected float pauseDurationMax = 6f;
    [SerializeField] protected float sitDurationMin = 3f;
    [SerializeField] protected float sitDurationMax = 8f;
    [SerializeField] protected float sitChance = 0.1f;
    private Vector3 basePosition;
    private float patrolTimer;
    private Vector3 patrolDirection;
    private float pauseTimer;
    private bool isPaused;
    private float sitTimer;
    private bool isSiting;
    private bool isInteract;

    [Header("Follow")]
    [SerializeField] protected float maxFollowDistance = 2f;
    private NavMeshAgent agent;
    private Vector3 playerPos;
    private float jumpHeight = 2f;
    private float jumpDuration = 0.5f;
    private bool isJumping = false;
    private bool itemGive = false;

    [Header("Rotation")]
    [SerializeField] protected float rotationSpeed = 5f; // Vitesse de rotation du monstre

    [Header("Bubble")]
    public ThoughtBubbleController thoughtBubble;
    public Image wantedItem;

    protected virtual void Awake()
    {
        monsterAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning($"No Rigidbody found on {gameObject.name}. Adding one.");
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"No NavMeshAgent found on {gameObject.name}. Please add one.");
            enabled = false;
            return;
        }

        agent.autoTraverseOffMeshLink = false;

        currentState = State.Patrolling;
        basePosition = transform.position;

        GameEventsManager.instance.trailEvents.onItemPickup += ActivateTrail;
        GameEventsManager.instance.trailEvents.onItemRelease += DeactivateTrail;

        // Bubble (Deactivate for Animation test)
        // thoughtBubble.SetWantedItem(wantedItem);
        // thoughtBubble.ShowBubble();
        // thoughtBubble.HideText();
        // thoughtBubble.ShowItem();
    }

    protected virtual void OnDisable()
    {
        GameEventsManager.instance.trailEvents.onItemPickup -= ActivateTrail;
        GameEventsManager.instance.trailEvents.onItemRelease -= DeactivateTrail;
    }

    protected virtual void Update()
    {
        if (!agent.isOnNavMesh) return;

        if (PlayerInput.all.Count == 0) return;

        var playerInput = PlayerInput.all[0];
        playerPos = playerInput.transform.position;

        if (isFriendly && Vector3.Distance(playerPos, transform.position) > maxFollowDistance)
        {
            currentState = State.Following;
        }

        switch (currentState)
        {
            case State.Patrolling: HandlePatrolling(); break;
            case State.Following: HandleFollowing(); break;
            case State.Siting: HandleSiting(); break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterCollectible") && !itemGive)
        {
            Interact(other.gameObject);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isSiting && !isJumping)
        {
            StartCoroutine(PlayInteractAnimation());
        }
    }

    private IEnumerator PlayInteractAnimation()
    {
        isInteract = true;
        ResetMonsterAnimation();
        monsterAnimator.SetBool("interact", true);
        yield return new WaitForSeconds(1.5f);
        ResetMonsterAnimation();
        monsterAnimator.SetBool("interact", false);
        isInteract = false;
    }

    public void Interact(GameObject item)
    {
        int itemCode = item.GetComponent<Collectible>().GetCode();

        if (itemCode == code)
        {
            thoughtBubble.HideBubble();
            currentState = State.Following;
            DeactivateTrail(itemCode);
            Destroy(item);
            itemGive = true;
        }
        else
        {
            thoughtBubble.ShowItem();
            thoughtBubble.HideText();
            currentState = State.Patrolling;
            monsterAnimator.SetBool("isMoving", true);
        }
    }

    /// <summary>
    /// Patrolling Section
    /// </summary>
    protected virtual void HandlePatrolling()
    {
        if (isInteract || isSiting) { return; }

        ResetMonsterAnimation();

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                isPaused = false;

                if (Random.value <= sitChance)
                {
                    currentState = State.Siting;
                    isSiting = true;
                    monsterAnimator.SetBool("isSiting", true);
                    sitTimer = Random.Range(sitDurationMin, sitDurationMax);
                }
                else
                {
                    SetNewPatrolDirection();
                    patrolTimer = patrolChangeInterval;
                    monsterAnimator.SetBool("isMoving", true);
                }
            }
            return;
        }

        // Activer l'animation de déplacement
        monsterAnimator.SetBool("isMoving", true);

        Vector3 nextPosition = rb.position + patrolDirection * patrolSpeed * Time.deltaTime;
        float distanceFromBase = Vector3.Distance(basePosition, nextPosition);

        // Déplacer le monstre
        if (distanceFromBase <= maxPatrolDistance)
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            patrolDirection = (basePosition - rb.position).normalized;
            rb.MovePosition(rb.position + patrolDirection * patrolSpeed * Time.deltaTime);
        }

        // Tourner le monstre vers la direction du mouvement
        RotateTowardsDirection(patrolDirection);

        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0)
        {
            isPaused = true;
            pauseTimer = Random.Range(pauseDurationMin, pauseDurationMax);
            monsterAnimator.SetBool("isMoving", false);
        }
    }

    protected virtual void SetNewPatrolDirection()
    {
        patrolDirection = directions[Random.Range(0, directions.Length)];
    }

    /// <summary>
    /// Siting Section
    /// </summary>
    protected virtual void HandleSiting()
    {
        ResetMonsterAnimation();
        monsterAnimator.SetBool("isSiting", true);

        sitTimer -= Time.deltaTime;
        if (sitTimer <= 0)
        {
            isSiting = false;

            SetNewPatrolDirection();
            patrolTimer = patrolChangeInterval;
            monsterAnimator.SetBool("isSiting", false);

            if (isFriendly)
            {
                currentState = State.Following;
            }
            else
            {
                currentState = State.Patrolling;
            }
        }
    }

    /// <summary>
    /// Following Section
    /// </summary>
    protected virtual void HandleFollowing()
    {
        ResetMonsterAnimation();

        if (Vector3.Distance(playerPos, transform.position) > maxFollowDistance)
        {
            monsterAnimator.SetBool("isMoving", true);
            agent.SetDestination(playerPos);

            // Tourner vers la direction du mouvement (basée sur la vélocité de l'agent)
            Vector3 moveDirection = agent.velocity.normalized;
            if (moveDirection != Vector3.zero)
            {
                RotateTowardsDirection(moveDirection);
            }
        }
        else
        {
            agent.isStopped = true;
            monsterAnimator.SetBool("isSiting", true);
        }

        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(DoJump());
        }
    }

    /// <summary>
    /// Handle monster jump based on NavMeshLink
    /// </summary>
    protected virtual IEnumerator DoJump()
    {
        if (jumpDuration <= 0)
        {
            jumpDuration = 0.5f;
        }

        isJumping = true;
        bool wasKinematic = rb.isKinematic;
        rb.isKinematic = true;
        agent.updatePosition = false;

        OffMeshLinkData link = agent.currentOffMeshLinkData;
        Vector3 startPos = transform.position;
        Vector3 endPos = link.endPos;

        float t = 0;
        ResetMonsterAnimation();
        monsterAnimator.SetBool("isJumping", true);
        while (t < jumpDuration)
        {
            float normalizedTime = t / jumpDuration;
            float height = 4 * jumpHeight * normalizedTime * (1 - normalizedTime);
            transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + Vector3.up * height;
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        agent.CompleteOffMeshLink();
        agent.updatePosition = true;
        rb.isKinematic = wasKinematic;

        isJumping = false;
        ResetMonsterAnimation();
        monsterAnimator.SetBool("isMoving", true);
    }

    /// <summary>
    /// Nouvelle méthode pour gérer la rotation fluide du monstre
    /// </summary>
    protected virtual void RotateTowardsDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return; // Ne pas tourner si aucune direction

        // Calculer la rotation cible
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Appliquer une rotation fluide
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Reset all animation states to avoid conflicts
    /// </summary>
    protected virtual void ResetMonsterAnimation()
    {
        monsterAnimator.SetBool("isMoving", false);
        monsterAnimator.SetBool("isSiting", false);
        monsterAnimator.SetBool("isJumping", false);
        monsterAnimator.SetBool("interact", false); // Ajout pour éviter des conflits avec l'animation d'interaction
    }

    /// <summary>
    /// Getter functions
    /// </summary>
    public virtual BiomesTemplate.BiomeType GetBiomeSpawn()
    {
        return spawnBiome;
    }

    /// <summary>
    /// Trail Section
    /// </summary>
    public virtual void SetupCode(int newCode)
    {
        code = newCode;
    }

    protected virtual void ActivateTrail(int monster, GameObject player)
    {
        if (monster == code)
        {
            MonsterTrail trail = GetComponent<MonsterTrail>();
            trail.SetPlayer(player);

            if (trail != null)
            {
                trail.enabled = true;
            }
        }
    }

    protected virtual void DeactivateTrail(int monster)
    {
        if (monster == code)
        {
            MonsterTrail trail = GetComponent<MonsterTrail>();
            if (trail != null)
            {
                trail.enabled = false;
            }
        }
    }
}
