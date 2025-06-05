using System;
using UnityEngine;

public class PlaytestEvent
{
    public event Action onCollect;
    public void OnCollect()
    {
        if (onCollect != null)
        {
            onCollect();
        }
    }
}
