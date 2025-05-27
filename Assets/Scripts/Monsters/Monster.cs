using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class Monster : MonoBehaviour
{
    public enum ElementType { Null, Fire, Ice, Electric, Psychic }
    public enum State { Patrolling, Power, Following }

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
    protected State currentState;
    protected Rigidbody rb;
    private static readonly Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    [Header("Power")]
    [SerializeField] protected Power power;

    [Header("Patrol")]
    [SerializeField] protected float patrolSpeed = 1.5f;
    [SerializeField] protected float patrolChangeInterval = 3f;
    [SerializeField] protected float maxPatrolDistance = 5f;
    private Vector3 basePosition;
    private float patrolTimer;
    private Vector3 patrolDirection;

    [Header("Follow")]
    [SerializeField] protected float maxFollowDistance = 2f;
    private NavMeshAgent agent;
    private bool isClose;
    private Vector3 playerPos;
    private float jumpHeight = 2f;
    private float jumpDuration = 0.5f;
    private bool isJumping = false;

    protected virtual void Start()
    {
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

        currentState = State.Following;
        basePosition = transform.position;

        GameEventsManager.instance.trailEvents.onItemPickup += ActivateTrail;
        GameEventsManager.instance.trailEvents.onItemGive += DeactivateTrail;
    }

    protected virtual void OnDisable()
    {
        GameEventsManager.instance.trailEvents.onItemPickup -= ActivateTrail;
        GameEventsManager.instance.trailEvents.onItemGive -= DeactivateTrail;
    }

    protected virtual void Update()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on a NavMesh!");
            return;
        }

        playerPos = GameObject.Find("Player").transform.position;
        if (playerPos == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        if (isFriendly && Vector3.Distance(playerPos, transform.position) > maxFollowDistance)
        {
            currentState = State.Following;
        }

        switch (currentState)
        {
            case State.Patrolling: HandlePatrolling(); break;
            case State.Power: HandlePower(); break;
            case State.Following: HandleFollowing(); break;
        }
    }

    /// <summary>
    /// Patrolling Section
    /// </summary>
    protected virtual void HandlePatrolling()
    {
        Vector3 nextPosition = rb.position + patrolDirection * patrolSpeed * Time.deltaTime;
        float distanceFromBase = Vector3.Distance(basePosition, nextPosition);

        if (distanceFromBase <= maxPatrolDistance)
        {
            rb.MovePosition(nextPosition);
        }
        else
        {
            patrolDirection = (basePosition - rb.position).normalized;
            rb.MovePosition(rb.position + patrolDirection * patrolSpeed * Time.deltaTime);
        }

        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0)
        {
            SetNewPatrolDirection();
            patrolTimer = patrolChangeInterval;
        }
    }

    protected virtual void SetNewPatrolDirection()
    {
        patrolDirection = directions[Random.Range(0, directions.Length)];
    }

    /// <summary>
    /// Power Section
    /// </summary>    
    protected virtual void HandlePower()
    {
        power.PowerEffect(transform.position);
        currentState = State.Patrolling;
    }

    /// <summary>
    /// Following Section
    /// </summary>
    protected virtual void HandleFollowing()
    {
        if (Vector3.Distance(playerPos, transform.position) > maxFollowDistance)
        {
            agent.SetDestination(playerPos);
        }

        if (agent.isOnOffMeshLink && !isJumping)
        {
            StartCoroutine(DoJump());
        }
    }

    /// <summary>
    /// Handle monster jump based on NavMeshLink. Use for "platformer" and when he is friendly (follow the player)
    /// </summary>
    /// <returns></returns>
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
        Debug.Log("Jump completed");
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

    protected virtual void ActivateTrail(int monster)
    {
        if (monster == code)
        {
            MonsterTrail trail = GetComponent<MonsterTrail>();
            if (trail != null)
                trail.enabled = true;
        }
    }

    protected virtual void DeactivateTrail(int monster)
    {
        if (monster == code)
        {
            MonsterTrail trail = GetComponent<MonsterTrail>();
            if (trail != null)
                trail.enabled = false;
        }
    }
}
