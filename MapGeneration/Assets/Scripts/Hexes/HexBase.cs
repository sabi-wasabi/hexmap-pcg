using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class HexBase : MonoBehaviour
{
    public abstract float GetRadius();
    public abstract void SetCoordinates(Vector2Int coords);
    public abstract Vector2Int[] GetHexPositionOffsets();
    public abstract void RegisterNeighbors();
    public abstract void InstantiateWalls();
}
