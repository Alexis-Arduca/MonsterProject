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

    private void Start()
    {
        drinkValue = 0.1f;
        emptyLevelY = transform.position.y - drinkValue * 3;
    }

    public void LowerWaterLevel()
    {
        transform.position -= new Vector3(0, drinkValue, 0); // Lower the water level by 1 unit
    }

    public bool IsWaterLevelEmpty()
    {
        return transform.position.y <= emptyLevelY; // Check if the water level is below the empty level
    }
}
