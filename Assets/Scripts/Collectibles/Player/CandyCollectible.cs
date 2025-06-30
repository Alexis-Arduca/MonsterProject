using UnityEngine;

public class PlaytestCollectible : MonoBehaviour
{
    public GameObject Star;
    public GameObject plusOne;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.playtestEvent.OnCollect();
            plusOne.SetActive(true);

            // GetComponent<MeshRenderer>().enabled = false;
            // GetComponent<SphereCollider>().enabled = false;

            // Star.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}