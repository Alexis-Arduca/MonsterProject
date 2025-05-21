using System;
using UnityEngine;

public class LoreEvents
{
    public event Action onImportantLoreEvent;
    public void OnImportantLoreEvent()
    {
        if (onImportantLoreEvent != null)
        {
            onImportantLoreEvent();
        }
    }
}
