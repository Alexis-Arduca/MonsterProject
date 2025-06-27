using UnityEngine;

public class scoreManager : MonoBehaviour
{

    [SerializeField] private float radius;
    public GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        FindThePlayer();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            transform.LookAt(player.transform);
            transform.Rotate(0, 90, 0);
        }
    }

    private void FindThePlayer()
    {
        Collider[] CoinColl = Physics.OverlapSphere(transform.position, radius);

        foreach (var c in CoinColl)
        {
            if (c.CompareTag("Player"))
            { 
                transform.position = Vector3.MoveTowards(transform.position, c.transform.position
                    + new Vector3(0f, 2f, 0f), Time.deltaTime * 25f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
