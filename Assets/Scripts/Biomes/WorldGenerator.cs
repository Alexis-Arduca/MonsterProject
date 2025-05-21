using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldGenerator : MonoBehaviour
{
    [Header("Ressources")]
    public Biomes lobby;
    public List<Biomes> biomes;
    public List<Monster> monsters;
    public List<Collectible> collectibles;
    private List<int> codes = new List<int>();

    [Header("Parameters")]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (!HandleParameters()) { return; }

        InitialiseCode();
        StartGeneration();
    }

    /// <summary>
    /// Handle world generation
    /// </summary>
    private void StartGeneration()
    {
        List<Biomes> biomesToSpawn = new List<Biomes>();

        foreach (Biomes biome in biomes)
        {
            BiomesTemplate.BiomeType biomeType = biome.GetComponent<BiomesTemplate>().biomeType;

            List<Monster> monsterToAdd = AddMonsters(biomeType);
            List<Collectible> collectibleToAdd = AddCollectibles(biomeType);
            List<int> biomeCode = AddCodes(monsterToAdd.Count);

            biome.FillBiome(monsterToAdd, collectibleToAdd, biomeCode);
            biomesToSpawn.Add(biome);
        }

        HandleBiomeSpawning(biomesToSpawn);
    }

    private void HandleBiomeSpawning(List<Biomes> biomesToSpawn)
    {
        Instantiate(lobby, Vector3.zero, Quaternion.identity);

        List<Vector3> positions = new List<Vector3>
        {
            new Vector3(0, 0, 25),
            new Vector3(-25, 0, -25),
            new Vector3(25, 0, -25)
        };

        List<Biomes> shuffledBiomes = biomesToSpawn.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < shuffledBiomes.Count; i++)
        {
            Instantiate(shuffledBiomes[i], positions[i], Quaternion.identity);
        }

        GameObject.Find("LoreManager").GetComponent<BiomeCameraTravelling>().InitializeBiomes();
    }

    /// <summary>
    /// Add monster to the corresponding biomes
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    private List<Monster> AddMonsters(BiomesTemplate.BiomeType biomeType)
    {
        List<Monster> monsterToAdd = new List<Monster>();

        foreach (Monster monster in monsters)
        {
            if (monster.GetBiomeSpawn() == biomeType)
            {
                Debug.Log("Monster added !");
                monsterToAdd.Add(monster);
            }
        }

        return monsterToAdd;
    }

    /// <summary>
    /// Add collectible to the corresponding biomes
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    private List<Collectible> AddCollectibles(BiomesTemplate.BiomeType biomeType)
    {
        List<Collectible> collectibleToAdd = new List<Collectible>();

        foreach (Collectible collectible in collectibles)
        {
            if (collectible.GetBiomeSpawn() == biomeType)
            {
                Debug.Log("Collectible added !");
                collectibleToAdd.Add(collectible);
            }
        }

        return collectibleToAdd;
    }

    /// <summary>
    /// Add code to biomes and remove them from the list
    /// </summary>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    private List<int> AddCodes(int nbMonster)
    {
        List<int> codesToAdd = new List<int>();

        for (int i = 0; i < nbMonster && codes.Count > 0; i++)
        {
            int index = Random.Range(0, codes.Count);
            int selectedCode = codes[index];
            codesToAdd.Add(selectedCode);
            codes.RemoveAt(index);
        }

        return codesToAdd;
    }

    // =============================================================================================================== \\

    /// <summary>
    /// Be sure that every parameters are good to avoid any error during the world generation
    /// </summary>
    /// <returns></returns>
    private bool HandleParameters()
    {
        if (monsters == null || collectibles == null || biomes == null)
        {
            Debug.Log("Invalid Parameters: There is at least 1 empty parameters !");
            return false;
        }

        if (biomes.Count != 3)
        {
            Debug.Log("Invalid Parameters: We need exactly 3 biomes !");
            return false;
        }

        if (monsters.Count != collectibles.Count)
        {
            Debug.Log("Invalid Parameters: It is required to have as many monsters as collectibles !");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Initialise all code for the monsters ans collectibles later
    /// </summary>
    private void InitialiseCode()
    {
        List<int> availableCodes = new List<int>();

        for (int i = 0; i < monsters.Count; i++)
        {
            availableCodes.Add(i);
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            int index = Random.Range(0, availableCodes.Count);
            int code = availableCodes[index];
            codes.Add(code);
            availableCodes.RemoveAt(index);
        }
    }

    /// <summary>
    /// Clear the current scene (use as an Editor function)
    /// </summary>
    public void ClearWorld()
    {
        Debug.Log("DESTROY THE WORLD !");

        Biomes[] biome = Object.FindObjectsByType<Biomes>(FindObjectsSortMode.None);
        foreach (var biomeToDestroy in biomes)
        {
            DestroyImmediate(biomeToDestroy.gameObject);
        }

        Monster[] monsters = Object.FindObjectsByType<Monster>(FindObjectsSortMode.None);
        foreach (var monster in monsters)
        {
            DestroyImmediate(monster.gameObject);
        }

        Collectible[] collectibles = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None);
        foreach (var collectible in collectibles)
        {
            DestroyImmediate(collectible.gameObject);
        }

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (var obstacle in obstacles)
        {
            DestroyImmediate(obstacle.gameObject);
        }
    }
    
    /// <summary>
    /// Unity Editor functions
    /// </summary>
    public void Executer()
    {
        ClearWorld();

        Start();
    }
}
