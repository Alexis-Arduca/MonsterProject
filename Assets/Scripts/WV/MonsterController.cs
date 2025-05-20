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
        _agent = GetComponent<NavMeshAgent>();

        _thoughtBubble = GetComponentInChildren<ThoughtBubbleController>();
        _thoughtBubble.SetWantedItem(wantedItem);

        _thoughtBubble.ShowBubble();
        _thoughtBubble.HideText();
        _thoughtBubble.ShowItem();
    }

    public void Interact(PickableController item)
    {
        if (item.icon == wantedItem)
        {
            _thoughtBubble.HideBubble();
            _following = true;
            item.Destroy();
        }
        else
        {
            _thoughtBubble.ShowItem();
            _thoughtBubble.HideText();
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
