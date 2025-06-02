using UnityEngine;

public class PlaytestCollectible : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.playtestEvent.OnCollect();
            Destroy(gameObject);
        }
    }
}
