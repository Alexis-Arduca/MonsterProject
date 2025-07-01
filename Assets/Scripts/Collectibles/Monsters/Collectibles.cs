using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] protected string itemName;
    [SerializeField] protected BiomesTemplate.BiomeType spawnBiome;
    public int monsterCode;
    private int code;

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

    public virtual int GetCode()
    {
        return code;
    }
}
