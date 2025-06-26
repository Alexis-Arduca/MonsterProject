using UnityEngine;

public class scoreManager : MonoBehaviour
{

    [SerializeField] private float radius;
    void Start()
    {
        
    }

    void Update()
    {
        FindThePlayer();
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
