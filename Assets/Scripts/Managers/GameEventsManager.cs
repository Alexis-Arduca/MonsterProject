using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public TrailEvents trailEvents;
    public BiomeEvents biomeEvents;
    public LoreEvents loreEvents;
    public EdibleEvents edibleEvents;
    public PlaytestEvent playtestEvent;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        // initialize all events
        trailEvents = new TrailEvents();
        biomeEvents = new BiomeEvents();
        loreEvents = new LoreEvents();
        edibleEvents = new EdibleEvents();
        playtestEvent = new PlaytestEvent();
    }
}
