using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] protected string itemName;
    [SerializeField] protected BiomesTemplate.BiomeType spawnBiome;
    private GameObject interactImage;
    public int monsterCode;
    private int code;
    private bool isPickup = false;

    public void SetImage(GameObject image)
    {
        interactImage = image;
    }

    public void IsPickup()
    {
        isPickup = !isPickup;
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

    public virtual int GetCode()
    {
        return code;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPickup)
        {
            interactImage.SetActive(true);
        }
        else
        {
            interactImage.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactImage.SetActive(false);
        }
    }
}
