using UnityEngine;
using System.Collections.Generic;

public class Biomes : MonoBehaviour
{
    public List<Monster> assignedMonsters;
    public float spawnDistance = 5f;

    void Start()
    {
        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.zero);

        SpawnMonsters(bounds);
    }

    void Update()
    {
    }

    /// <summary>
    /// Function that will spawn Monster in a Biome at random position
    /// Also compare with each other monster position to see if they are close or not
    /// </summary>
    /// <param name="bounds"></param>
    private void SpawnMonsters(Bounds bounds)
    {
        if (assignedMonsters == null || assignedMonsters.Count == 0)
            return;

        List<Vector3> monstersPositions = new List<Vector3>();

        foreach (var monster in assignedMonsters)
        {
            Vector3 randomPos = new Vector3();
            bool validPosition = false;

            while (!validPosition)
            {
                randomPos = new Vector3(
                    Random.Range(bounds.min.x + 1f, bounds.max.x - 1f),
                    bounds.min.y + 0.5f,
                    Random.Range(bounds.min.z + 1f, bounds.max.z - 1f)
                );

                validPosition = true;
                foreach (Vector3 monsterPos in monstersPositions)
                {
                    Vector2 posXZ = new Vector2(randomPos.x, randomPos.z);
                    Vector2 monsterXZ = new Vector2(monsterPos.x, monsterPos.z);
                    if (Vector2.Distance(posXZ, monsterXZ) < spawnDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }
        
            monstersPositions.Add(randomPos);
            Instantiate(monster.gameObject, randomPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Delete all monsters
    /// </summary>
    public void ClearBiome()
    {
        Monster[] monsters = Object.FindObjectsByType<Monster>(FindObjectsSortMode.None);

        foreach (var monster in monsters)
        {
            GameObject.DestroyImmediate(monster.gameObject);
        }
    }

    /// <summary>
    /// Usage for test in UnityEditor mode
    /// </summary>
    public void Executer()
    {
        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.zero);

        Debug.Log("DESTROY EVERY MONSTERS !");
        ClearBiome();

        Debug.Log("BIOMES EXECUTION IN EDITOR MODE !");
        SpawnMonsters(bounds);
    }
}
