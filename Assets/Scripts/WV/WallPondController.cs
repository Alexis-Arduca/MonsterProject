using UnityEngine;

// <summary>
// Attached to the PondInteraction Prefab.
// This script is responsible for controlling the interaction with the pond.
// It allows the player to drink from the pond, which lowers the water level.
// </summary>

public class WallPondController : MonoBehaviour
{
    [Header("Pond Controller")]
    public PondController pondController;

    public void Drink()
    {
        pondController.LowerWaterLevel();
    }
}
