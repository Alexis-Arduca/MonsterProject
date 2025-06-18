using UnityEngine;

public class PlaytestCollectible : MonoBehaviour
{
    [SerializeField] private Transform[] Scores;
    private int index;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.playtestEvent.OnCollect();

            Scores[index].gameObject.SetActive(true);
            Scores[index].position = other.transform.position;

            if (index < 10)
                index++;
            else
            {
                index = 0;
            }

            Destroy(gameObject);
        }
    }
}
