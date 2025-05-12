using UnityEngine;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
    public enum ElementType { Null, Fire, Ice, Electric, Psychic }
    public enum State { Patrolling, Power }

    [Header("Description")]
    [SerializeField] protected string monsterName;
    [SerializeField] protected string description;

    [Header("Parameters")]
    [SerializeField] protected ElementType element = ElementType.Null;
    [SerializeField] protected List<ElementType> weakness = new List<ElementType>();
    [SerializeField] protected List<ElementType> immunity = new List<ElementType>();

    [Header("Power")]
    [SerializeField] protected Power power;

    [Header("Patrol")]
    [SerializeField] protected float patrolSpeed = 1.5f;
    [SerializeField] protected float patrolChangeInterval = 3f;
    private float patrolTimer;
    private Vector3 patrolDirection;
    protected State currentState;
    protected Rigidbody rb;
    private static readonly Vector3[] directions = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = State.Patrolling;
    }

    private float stateChangeTimer = 0f;
    protected virtual void Update()
    {
        stateChangeTimer += Time.deltaTime;

        if (stateChangeTimer >= 5f)
        {
            currentState = State.Power;
            stateChangeTimer = 0f;
        }

        switch (currentState)
        {
            case State.Patrolling: HandlePatrolling(); break;
            case State.Power: HandlePower(); break;
        }
    }

    /// <summary>
    /// Patrol Section
    /// </summary>
    protected virtual void HandlePatrolling()
    {
        rb.MovePosition(rb.position + patrolDirection * patrolSpeed * Time.deltaTime);
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
}
