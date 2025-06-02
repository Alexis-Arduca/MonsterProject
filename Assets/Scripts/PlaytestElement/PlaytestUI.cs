using UnityEngine;
using System;
using TMPro;

public class PlaytestUI : MonoBehaviour
{
    [Header("Playtest Parameters")]
    public int maxGoals = 5;
    private int currentGoals = 0;
    public TMP_Text showCollected;

    void Start()
    {
        GameEventsManager.instance.playtestEvent.onCollect += GetCandy;
        showCollected.text = currentGoals + "/" + maxGoals + " little candys collected";
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playtestEvent.onCollect -= GetCandy;
    }

    private void GetCandy()
    {
        currentGoals += 1;

        showCollected.text = currentGoals + "/" + maxGoals + " little candys collected";
    }
}
