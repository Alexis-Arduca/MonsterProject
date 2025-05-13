using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class MonsterTrail : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;
    private NavMeshPath path;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (agent.isOnNavMesh && player != null)
        {
            NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                lineRenderer.positionCount = path.corners.Length;
                lineRenderer.SetPositions(path.corners);
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }
    }
}
