using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Biomes))]
public class MonScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (target == null)
            return;

        try
        {
            DrawDefaultInspector();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error - DrawDefaultInspector: " + e.Message);
        }

        Biomes script = (Biomes)target;
        if (GUILayout.Button("Generate Monsters and Collectibles"))
        {
            script.EditorMonsters();
        }

        if (GUILayout.Button("Generate Obstacles"))
        {
            script.EditorObstacles();
        }

        if (GUILayout.Button("Clear Biome"))
        {
            script.ClearBiome();
        }
    }
}
