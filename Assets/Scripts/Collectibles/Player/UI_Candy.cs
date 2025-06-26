using UnityEngine;
using System;
using TMPro;

public class PlaytestUI : MonoBehaviour
{
    [Header("Playtest Parameters")]
    public int maxGoals;
    private int currentGoals = 0;
    public TMP_Text showCollected;

    void Start()
    {
        GameEventsManager.instance.playtestEvent.onCollect += GetCandy;
        GameObject collectibles = GameObject.Find("Collectibles");

        maxGoals = collectibles.transform.childCount;
        showCollected.text = currentGoals + "/" + maxGoals;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playtestEvent.onCollect -= GetCandy;
    }

    private void GetCandy()
    {
        currentGoals += 1;

        showCollected.text = currentGoals + "/" + maxGoals;
    }
}
