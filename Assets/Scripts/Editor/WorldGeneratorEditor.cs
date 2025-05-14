using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
[CanEditMultipleObjects]
public class MonScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (targets == null || targets.Length == 0)
            return;

        try
        {
            DrawDefaultInspector();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error - DrawDefaultInspector: " + e.Message);
        }

        if (GUILayout.Button("Generate World"))
        {
            foreach (var t in targets)
            {
                WorldGenerator script = t as WorldGenerator;
                if (script != null)
                    script.Executer();
            }
        }

        if (GUILayout.Button("Clear World"))
        {
            foreach (var t in targets)
            {
                WorldGenerator script = t as WorldGenerator;
                if (script != null)
                    script.ClearWorld();
            }
        }
    }
}
