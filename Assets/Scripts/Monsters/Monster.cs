using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    public enum ElementType { Null, Fire, Ice, Electric, Psychic }

    [Header("Description")]
    [SerializeField] protected string monsterName;
    [SerializeField] protected string description;

    [Header("Parameters")]
    [SerializeField] protected ElementType element = ElementType.Null;

    [Header("Power")]
    [SerializeField] protected Power power;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }
}
