using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public Camera camera1;
    public Camera camera2;
    public int numberOfPlayers = 1;

    void Start()
    {
        if (numberOfPlayers >= 1)
        {
            Instantiate(player1Prefab, new Vector3(-2, 0, 0), Quaternion.identity);
            camera1.gameObject.SetActive(true);
            camera1.rect = new Rect(0f, 0f, 1f, 1f);
        }

        if (numberOfPlayers == 2)
        {
            Instantiate(player2Prefab, new Vector3(2, 0, 0), Quaternion.identity);
            camera2.gameObject.SetActive(true);
            camera1.rect = new Rect(0f, 0f, 0.5f, 1f);
            camera2.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
        else
        {
            camera2.gameObject.SetActive(false);
        }
    }
}
