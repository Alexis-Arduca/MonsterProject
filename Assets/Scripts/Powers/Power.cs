using UnityEngine;
using System;

public class Power : MonoBehaviour
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

    public virtual void PowerEffect()
    {
        // Each power will implement is own effect
    }
}
