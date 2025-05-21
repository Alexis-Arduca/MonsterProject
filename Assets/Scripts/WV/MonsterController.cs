using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// <summary>
// Attached to the Monster Prefab.
// This script is responsible for controlling the monster's behavior.
// It handles the monster's movement towards the player and the interaction with the player.
// </summary>

public class MonsterController : MonoBehaviour
{
    public ThoughtBubbleController thoughtBubble;

    private NavMeshAgent _agent;
    private bool _following;

    [Header("Thought Bubble")]
    [Tooltip("The item the monster wants.")]
    public Image wantedItem;

    [Header("NavMesh")]
    [Tooltip("The target the monster will follow.")]
    public Transform target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        thoughtBubble = GetComponentInChildren<ThoughtBubbleController>();
        thoughtBubble.SetWantedItem(wantedItem);

        thoughtBubble.ShowBubble();
        thoughtBubble.HideText();
        thoughtBubble.ShowItem();
    }

    public void Interact(PickableController item)
    {
        if (item.icon == wantedItem)
        {
            thoughtBubble.HideBubble();
            _following = true;
            item.Destroy();
        }
        else
        {
            thoughtBubble.ShowItem();
            thoughtBubble.HideText();
            _following = false;
        }
    }

    private void Update()
    {
        if (target && _following)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        _agent.SetDestination(target.position);
    }
}
