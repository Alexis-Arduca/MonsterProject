using System;
using UnityEngine;

public class TrailEvents
{
    public event Action<int> onItemPickup;
    public void OnItemPickup(int monster)
    {
        if (onItemPickup != null)
        {
            onItemPickup(monster);
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
