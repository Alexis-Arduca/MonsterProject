using UnityEngine;

public class PlaytestCollectible : MonoBehaviour
{
    public GameObject Star;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.playtestEvent.OnCollect();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Star.SetActive(false);
        }
    }
}
