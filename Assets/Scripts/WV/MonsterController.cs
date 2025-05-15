using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    private ThoughtBubbleController _thoughtBubble;

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
        _thoughtBubble = GetComponentInChildren<ThoughtBubbleController>();
        _thoughtBubble.ShowBubble();
        _thoughtBubble.SetWantedItem(wantedItem);
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Interact(PickableController item)
    {
        if (wantedItem == item.icon)
        {
            _thoughtBubble.HideBubble();
            item.Destroy();
            _following = true;
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
