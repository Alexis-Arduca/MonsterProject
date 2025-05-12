using UnityEngine;
using System;

// [CreateAssetMenu(fileName = "Spit", menuName = "Inventory/Bomb")]
public class Spit : Power
{
    public GameObject spitObject;
    public int spitEntityNumber = 3;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void PowerEffect()
    {
        base.PowerEffect();

        for (int i = 0; i < spitEntityNumber; i++)
        {
            Instantiate(spitObject);
        }
    }
}
