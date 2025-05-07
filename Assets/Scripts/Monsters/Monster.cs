using UnityEngine;
using System;

public class Monster : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] protected string monsterName;
    [SerializeField] protected string description;

    [Header("Power")]
    [SerializeField] protected Power power;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }
}
