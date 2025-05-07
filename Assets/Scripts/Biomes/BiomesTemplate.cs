using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BiomesTemplate : MonoBehaviour
{
    public enum RoomType { Null, Lava, Stormy, Ice }
    public RoomType roomType = RoomType.Null;

    public List<Monster> spawnableMonster;
}