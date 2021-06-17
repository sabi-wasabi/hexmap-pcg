using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class HexBase : MonoBehaviour
{
    public bool visited { get; set; }
    public Vector2Int coords { get; set; }
    public abstract float GetRadius();
    public abstract void SetCoordinates(Vector2Int coords);
    public abstract Vector2Int[] GetHexPositionOffsets();
    public abstract void RegisterNeighbors(Dictionary<Vector2Int, HexBase> hexGrid);
    public abstract void InstantiateWalls();
    public abstract void SetWall(Vector2Int offSet, bool isActive, Vector2Int otherCoords);
    public abstract void Visit();
}
