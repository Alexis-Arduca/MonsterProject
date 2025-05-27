using System;
using UnityEngine;

public class EdibleEvents
{
    public event Action onDrink;
    public void OnDrink()
    {
        if (onDrink != null)
        {
            onDrink();
        }
    }

    public event Action onEat;
    public void OnEat()
    {
        if (onEat != null)
        {
            onEat();
        }
    }

    public event Action onLick;
    public void OnLick()
    {
        if (onLick != null)
        {
            onLick();
        }
    }
}
