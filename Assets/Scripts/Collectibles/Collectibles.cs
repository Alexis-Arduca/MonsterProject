using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] protected string itemName;
    [SerializeField] protected BiomesTemplate.BiomeType spawnBiome;
    private int code;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.instance.trailEvents.OnItemPickup(code, other.gameObject);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Getter functions
    /// </summary>
    /// <returns></returns>
    public virtual BiomesTemplate.BiomeType GetBiomeSpawn()
    {
        return spawnBiome;
    }

    /// <summary>
    /// Trail Section
    /// </summary>
    public virtual void SetupCode(int newCode)
    {
        code = newCode;
    }
}
