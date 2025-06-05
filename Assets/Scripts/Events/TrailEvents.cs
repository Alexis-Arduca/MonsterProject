using System;
using UnityEngine;

public class TrailEvents
{
    public event Action<int, GameObject> onItemPickup;
    public void OnItemPickup(int monster, GameObject player)
    {
        if (onItemPickup != null)
        {
            onItemPickup(monster, player);
        }
    }

    public event Action<int> onItemGive;
    public void OnItemGive(int monster)
    {
        if (onItemGive != null)
        {
            onItemGive(monster);
        }
    }
}
