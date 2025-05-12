using UnityEngine;
using System;

public class Power : ScriptableObject
{
    [Header("Description")]
    [SerializeField] protected string powerName;
    [SerializeField] protected string description;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void PowerEffect(Vector3 origin)
    {
        // Each power will implement is own effect
    }
}
