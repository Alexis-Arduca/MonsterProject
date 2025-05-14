using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    private ThoughtBubbleController _thoughtBubble;
    public Image wantedItem;
    private PickableController _pickableController;
    private NavMeshAgent _agent;
    public Transform target;

    private void Start()
    {
        _thoughtBubble = GetComponentInChildren<ThoughtBubbleController>();
        _thoughtBubble.ShowBubble();
        _thoughtBubble.SetWantedItem(wantedItem);
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = true;
    }

    public void Interact(PickableController item)
    {
        if (wantedItem == item.icon)
        {
            _thoughtBubble.HideBubble();
            item.Destroy();
            _agent.SetDestination(target.position);
        }
    }
}
