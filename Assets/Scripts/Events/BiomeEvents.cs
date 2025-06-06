using System;
using UnityEngine;

public class BiomeEvents
{
    public event Action<string> onIceBiomeEnter;
    public void OnIceBiomeEnter(string file)
    {
        if (onIceBiomeEnter != null)
        {
            onIceBiomeEnter(file);
        }
    }

    public event Action<string> onFireBiomeEnter;
    public void OnFireBiomeEnter(string file)
    {
        if (onFireBiomeEnter != null)
        {
            onFireBiomeEnter(file);
        }
    }

    public event Action<string> onThunderBiomeEnter;
    public void OnThunderBiomeEnter(string file)
    {
        if (onThunderBiomeEnter != null)
        {
            onThunderBiomeEnter(file);
        }
    }
}
