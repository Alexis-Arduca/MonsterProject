using UnityEngine;

// <summary>
// This script is responsible for controlling the water level of the pond.
// It handles lowering the water level and checking if the water level is empty.
// </summary>

public class PondController : MonoBehaviour
{
    // <summary>
    // Set the position of the empty level.
    // This is used to ensure the water level is below the pond.
    // </summary>
    [SerializeField] private float emptyLevelY;
    [SerializeField] private float drinkValue;

    public void LowerWaterLevel()
    {
        var children = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        transform.position -= new Vector3(0, drinkValue, 0); // Lower the water level by 1 unit

        foreach (var child in children)
        {
            child.transform.position += new Vector3(0, drinkValue, 0); // Move the child objects up by the same amount
        }

    }

    public bool IsWaterLevelEmpty()
    {
        return transform.position.y <= emptyLevelY; // Check if the water level is below the empty level
    }
}
