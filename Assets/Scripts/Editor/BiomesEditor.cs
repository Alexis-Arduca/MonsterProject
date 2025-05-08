using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Biomes))]
public class MonScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Biomes script = (Biomes)target;
        if (GUILayout.Button("Spawn Monsters"))
        {
            script.Executer();
        }

        if (GUILayout.Button("Delete Monsters"))
        {
            script.ClearBiome();
        }
    }
}
