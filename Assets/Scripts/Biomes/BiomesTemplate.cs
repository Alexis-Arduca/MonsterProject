using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BiomesTemplate : MonoBehaviour
{
    public enum BiomeType { Null, Lava, Stormy, Ice, Lobby, All }
    public BiomeType biomeType = BiomeType.Null;
}
