using System;
using UnityEngine;

public class AppleEvents
{
    public event Action<int> onAppleCollected;
    public void OnAppleCollected(int value)
    {
        if (onAppleCollected != null)
        {
            onAppleCollected(value);
        }
    }

    public event Action<int> onAppleUsed;
    public void OnAppleUsed(int value)
    {
        if (onAppleUsed != null)
        {
            onAppleUsed(value);
        }
    }
}