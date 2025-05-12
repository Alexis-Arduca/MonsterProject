using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSpit", menuName = "Powers/Spit")]
public class Spit : Power
{
    public GameObject spitObject;
    public int spitEntityNumber = 3;
    public float timeBeforeRemoval = 1f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void PowerEffect(Vector3 origin)
    {
        base.PowerEffect(origin);

        for (int i = 0; i < spitEntityNumber; i++)
        {
            GameObject obj = Instantiate(spitObject, origin, Quaternion.identity);
            Destroy(obj, timeBeforeRemoval);
        }
    }
}
