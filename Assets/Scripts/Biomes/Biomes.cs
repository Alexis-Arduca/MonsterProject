using UnityEngine;
using System.Collections.Generic;

public class Biomes : MonoBehaviour
{
    [Header("List for Testing")]
    public List<Monster> assignedMonsters;
    public List<Collectible> assignedCollectibles;
    public List<int> codeList;

    [Header("Biome Parameters")]
    public float spawnDistance = 5f;
    private int monsterNumber;

    void Start()
    {
        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.one * 10f);

        monsterNumber = 3;

        SpawnMonstersAndCollectibles(bounds);
    }

    void Update()
    {
    }

    /// <summary>
    /// Will handle the spawn of the monsters and the collectibles.
    /// Need to spawn each monster one time and their collectible next to them. Will also setup their shared code.
    /// </summary>
    /// <param name="bounds"></param>
    private void SpawnMonstersAndCollectibles(Bounds bounds)
    {
        if (assignedMonsters == null || assignedMonsters.Count < monsterNumber ||
            assignedCollectibles == null || assignedCollectibles.Count < monsterNumber ||
            codeList == null || codeList.Count < monsterNumber)
        {
            Debug.LogWarning("Spawn cancelled: not enough data.");
            return;
        }

        List<Vector3> usedPositions = new List<Vector3>();

        for (int i = 0; i < monsterNumber; i++)
        {
            int codeIndex = Random.Range(0, codeList.Count);
            int sharedCode = codeList[codeIndex];
            codeList.RemoveAt(codeIndex);

            int monsterIndex = Random.Range(0, assignedMonsters.Count);
            Monster monsterPrefab = assignedMonsters[monsterIndex];
            assignedMonsters.RemoveAt(monsterIndex);

            int collectibleIndex = Random.Range(0, assignedCollectibles.Count);
            Collectible collectiblePrefab = assignedCollectibles[collectibleIndex];
            assignedCollectibles.RemoveAt(collectibleIndex);

            Vector3 monsterPos = GetValidPosition(bounds, usedPositions);
            usedPositions.Add(monsterPos);
            GameObject monsterObj = Instantiate(monsterPrefab.gameObject, monsterPos, Quaternion.identity);
            monsterObj.GetComponent<Monster>().SetupCode(sharedCode);

            Vector3 collectiblePos = GetNearbyValidPosition(monsterPos, usedPositions, bounds);
            usedPositions.Add(collectiblePos);
            GameObject collectibleObj = Instantiate(collectiblePrefab.gameObject, collectiblePos, Quaternion.identity);
            collectibleObj.GetComponent<Collectible>().SetupCode(sharedCode);
        }
    }

    /// <summary>
    /// Get Valid Position for Monsters and Collectibles
    /// </summary>
    private Vector3 GetValidPosition(Bounds bounds, List<Vector3> existingPositions)
    {
        Vector3 pos = Vector3.zero;
        bool valid = false;
        int attempts = 0;

        while (!valid && attempts < 50)
        {
            pos = new Vector3(
                Random.Range(bounds.min.x + 1f, bounds.max.x - 1f),
                bounds.min.y + 0.5f,
                Random.Range(bounds.min.z + 1f, bounds.max.z - 1f)
            );

            valid = true;
            foreach (Vector3 p in existingPositions)
            {
                if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(p.x, p.z)) < spawnDistance)
                {
                    valid = false;
                    break;
                }
            }

            attempts++;
        }

        if (!valid)
        {
            Debug.LogWarning("Failed to find valid position for monster, fallback to center");
            pos = bounds.center;
        }

        return pos;
    }

    private Vector3 GetNearbyValidPosition(Vector3 center, List<Vector3> existingPositions, Bounds bounds)
    {
        Vector3 pos = center;
        bool valid = false;
        int attempts = 0;

        while (!valid && attempts < 50)
        {
            float radius = spawnDistance * 0.75f;
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(1f, radius);

            pos = new Vector3(
                center.x + Mathf.Cos(angle) * distance,
                center.y,
                center.z + Mathf.Sin(angle) * distance
            );

            Vector3 checkPos = new Vector3(pos.x, bounds.center.y, pos.z);
            if (!bounds.Contains(checkPos))
            {
                attempts++;
                continue;
            }

            valid = true;
            foreach (Vector3 p in existingPositions)
            {
                if (Vector2.Distance(new Vector2(pos.x, pos.z), new Vector2(p.x, p.z)) < spawnDistance)
                {
                    valid = false;
                    break;
                }
            }

            attempts++;
        }

        if (!valid)
        {
            Debug.LogWarning("Failed to find nearby valid position for collectible, fallback to monster position.");
            pos = center;
        }

        return pos;
    }

    /// <summary>
    /// Reset the Biome. Use for Unity Editor
    /// </summary>
    public void ClearBiome()
    {
        Debug.Log("DESTROY EVERY MONSTERS AND COLLECTIBLES !");

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

        monsterNumber = 3;
    }

    /// <summary>
    /// Execute the spawning. Use for Unity Editor
    /// </summary>
    public void Executer()
    {
        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.one * 10f);

        ClearBiome();
        monsterNumber = 3;

        Debug.Log("BIOMES EXECUTION IN EDITOR MODE !");
        SpawnMonstersAndCollectibles(bounds);
    }
}
