using UnityEngine;

public class WallPondController : MonoBehaviour
{
    public PondController pondController;

    public void Drink()
    {
        pondController.LowerWaterLevel();
    }
}
