using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

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
    private float stateChangeTimer = 0f;
    private Vector3 patrolDirection;

    [Header("Follow")]
    [SerializeField] protected float maxFollowDistance = 2f;
    private NavMeshAgent agent;
    private bool isClose;
    private Vector3 playerPos;

    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentState = State.Patrolling;
        basePosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

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
        playerPos = GameObject.Find("Player").transform.position;

                    // Power State \\
        // stateChangeTimer += Time.deltaTime;

        // if (stateChangeTimer >= 5f)
        // {
        //     currentState = State.Power;
        //     stateChangeTimer = 0f;
        // }

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
        else
        {
            basePosition = playerPos;
            maxPatrolDistance = maxFollowDistance;
            currentState = State.Patrolling;
        }
    }

    /// <summary>
    /// Getter functions
    /// </summary>
    /// <returns></returns>
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

            trail.enabled = true;
        }
    }

    protected virtual void DeactivateTrail(int monster)
    {
        if (monster == code)
        {
            MonsterTrail trail = GetComponent<MonsterTrail>();

            trail.enabled = false;
        }
    }
}
