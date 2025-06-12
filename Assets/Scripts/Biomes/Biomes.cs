using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Biomes : MonoBehaviour
{
    [Header("Biome Parameters")]
    public List<Monster> assignedMonsters;
    public List<Collectible> assignedCollectibles;
    public List<int> codeList;
    public List<GameObject> monsterSpawnpoints;
    private int monsterNumber;
    private bool hasBeenTriggered = false;

    [Header("Spawn Settings")]
    private readonly float topViewRotation = 40f;

    void Start()
    {
        if (GetComponent<BiomesTemplate>().biomeType != BiomesTemplate.BiomeType.Lobby)
        {
            SpawnMonstersAndCollectibles();
        }
    }

    /// <summary>
    /// Fill the biome with monsters, collectibles, and codes
    /// </summary>
    public void FillBiome(List<Monster> myMonsters, List<Collectible> myCollectibles, List<int> myCodes)
    {
        assignedMonsters = new List<Monster>(myMonsters);
        assignedCollectibles = new List<Collectible>(myCollectibles);
        codeList = new List<int>(myCodes);
    }

    /// <summary>
    /// Handle the spawn of monsters and collectibles at predefined spawn points
    /// </summary>
    private void SpawnMonstersAndCollectibles()
    {
        if (assignedMonsters == null || assignedCollectibles == null || codeList == null)
        {
            Debug.LogWarning($"Monsters spawn cancelled for biome {name}: not enough data.");
            return;
        }

        if (monsterSpawnpoints == null || monsterSpawnpoints.Count < assignedMonsters.Count)
        {
            Debug.LogWarning($"Not enough monster spawn points for biome {name}. Required: {assignedMonsters.Count}, Available: {monsterSpawnpoints?.Count ?? 0}");
            return;
        }

        monsterNumber = assignedMonsters.Count;
        List<GameObject> availableSpawnPoints = new List<GameObject>(monsterSpawnpoints);
        availableSpawnPoints = availableSpawnPoints.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < monsterNumber; i++)
        {
            if (availableSpawnPoints.Count < 1)
            {
                Debug.LogWarning($"Ran out of spawn points for biome {name}!");
                return;
            }

            int codeIndex = Random.Range(0, codeList.Count);
            int sharedCode = codeList[codeIndex];
            codeList.RemoveAt(codeIndex);

            int monsterIndex = Random.Range(0, assignedMonsters.Count);
            Monster monsterPrefab = assignedMonsters[monsterIndex];
            assignedMonsters.RemoveAt(monsterIndex);

            int spawnIndex = 0;
            GameObject monsterSpawnPoint = availableSpawnPoints[spawnIndex];
            Vector3 monsterPos = monsterSpawnPoint.transform.position;
            GameObject monsterObj = Instantiate(monsterPrefab.gameObject, monsterPos, Quaternion.identity);
            monsterObj.GetComponent<Monster>().SetupCode(sharedCode);
            availableSpawnPoints.RemoveAt(spawnIndex);

            int collectibleIndex = Random.Range(0, assignedCollectibles.Count);
            Collectible collectiblePrefab = assignedCollectibles[collectibleIndex];
            assignedCollectibles.RemoveAt(collectibleIndex);

            Vector3 collectiblePos = GetCollectibleSpawnPosition(monsterSpawnPoint);
            GameObject collectibleObj = Instantiate(collectiblePrefab.gameObject, collectiblePos, Quaternion.identity);
            collectibleObj.GetComponent<Collectible>().SetupCode(sharedCode);
        }
    }

    /// <summary>
    /// Get a spawn position for the collectible from the child spawn points of the monster's spawn point
    /// </summary>
    private Vector3 GetCollectibleSpawnPosition(GameObject monsterSpawnPoint)
    {
        List<Transform> collectibleSpawnPoints = new List<Transform>();
        foreach (Transform child in monsterSpawnPoint.transform)
        {
            collectibleSpawnPoints.Add(child);
        }

        if (collectibleSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, collectibleSpawnPoints.Count);
            return collectibleSpawnPoints[randomIndex].position;
        }

        Debug.LogWarning($"No collectible spawn points found as children of {monsterSpawnPoint.name} in biome {name}. Falling back to monster position.");
        return monsterSpawnPoint.transform.position;
    }

    /// <summary>
    /// Handle Camera travelling when entering a biome
    /// </summary>
    private Vector3 GetTopViewPosition(Transform biomeTransform)
    {
        return biomeTransform.position + Vector3.up * topViewRotation;
    }

    private Quaternion GetTopViewRotation()
    {
        return Quaternion.Euler(90f, 0f, 0f);
    }

    /// <summary>
    /// Reset the Biome. Use for Unity Editor
    /// </summary>
    public void ClearBiome()
    {
        Debug.Log($"Clearing biome {name}!");

        Monster[] monsters = GetComponentsInChildren<Monster>();
        foreach (var monster in monsters)
        {
            DestroyImmediate(monster.gameObject);
        }

        Collectible[] collectibles = GetComponentsInChildren<Collectible>();
        foreach (var collectible in collectibles)
        {
            DestroyImmediate(collectible.gameObject);
        }

        assignedMonsters?.Clear();
        assignedCollectibles?.Clear();
        codeList?.Clear();
    }

    /// <summary>
    /// Collision to check if we enter in a biome
    /// </summary>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) { return; }

        if (other.CompareTag("Player") && GetComponent<BiomesTemplate>().biomeType != BiomesTemplate.BiomeType.Lobby)
        {
            hasBeenTriggered = true;

            switch (GetComponent<BiomesTemplate>().biomeType)
            {
                case BiomesTemplate.BiomeType.Ice:
                    GameEventsManager.instance.biomeEvents.OnIceBiomeEnter("Assets/Story/IceBiome.txt");
                    break;

                case BiomesTemplate.BiomeType.Stormy:
                    GameEventsManager.instance.biomeEvents.OnThunderBiomeEnter("Assets/Story/ThunderBiome.txt");
                    break;

                case BiomesTemplate.BiomeType.Lava:
                    GameEventsManager.instance.biomeEvents.OnFireBiomeEnter("Assets/Story/LavaBiome.txt");
                    break;
            }
        }
    }
}
