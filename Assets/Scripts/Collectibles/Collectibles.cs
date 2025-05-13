using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Collectible : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] protected string itemName;
    private int code;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.trailEvents.OnItemPickup(code);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Trail Section
    /// </summary>
    public virtual void SetupCode(int newCode)
    {
        code = newCode;
    }
}
