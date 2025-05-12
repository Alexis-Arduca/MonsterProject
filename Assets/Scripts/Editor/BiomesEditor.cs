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
