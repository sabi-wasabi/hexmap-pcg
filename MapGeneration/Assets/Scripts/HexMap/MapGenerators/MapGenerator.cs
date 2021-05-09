using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator : ScriptableObject
{
    // Map Generators determine the shape of Map
    public abstract Vector2Int[] GenerateMapGrid(int size);
}
