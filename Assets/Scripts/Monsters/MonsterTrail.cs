using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class MonsterTrail : MonoBehaviour
{
    [SerializeField] private float sampleDistance = 0.5f;
    [SerializeField] private float heightOffset = 0.1f;
    [SerializeField] private int maxPoints = 100;

    private Transform player;
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
                Vector3[] sampledPoints = SamplePath(path, sampleDistance);
                lineRenderer.positionCount = sampledPoints.Length;
                lineRenderer.SetPositions(sampledPoints);
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject.transform;
    }

    private Vector3[] SamplePath(NavMeshPath path, float sampleDistance)
    {
        if (path.corners.Length < 2) return path.corners;

        List<Vector3> points = new List<Vector3>();
        points.Add(path.corners[0]);

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Vector3 start = path.corners[i];
            Vector3 end = path.corners[i + 1];
            float distance = Vector3.Distance(start, end);

            for (float d = sampleDistance; d < distance; d += sampleDistance)
            {
                float t = d / distance;
                Vector3 point = Vector3.Lerp(start, end, t);

                if (NavMesh.SamplePosition(point, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    points.Add(hit.position + Vector3.up * heightOffset);
                }
            }

            if (NavMesh.SamplePosition(end, out NavMeshHit endHit, 1.0f, NavMesh.AllAreas))
            {
                points.Add(endHit.position + Vector3.up * heightOffset);
            }
        }

        if (points.Count > maxPoints)
        {
            points = new List<Vector3>(points.Take(maxPoints));
        }

        return points.ToArray();
    }
}
