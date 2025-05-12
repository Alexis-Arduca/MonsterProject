using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private string _wantedItem = "Pickable";

    public void Interact(string name)
    {
        if (name == _wantedItem)
        {
            Debug.Log($"Monster: I want {name}!");
            // Add logic for monster interaction here
        }
        else
        {
            Debug.Log($"Monster: I don't want {name}.");
        }
    }
}
