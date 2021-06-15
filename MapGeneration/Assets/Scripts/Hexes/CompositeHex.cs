using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeHex : HexBase
{
    [SerializeField] private HexBase[] _hexes;
    [SerializeField] private Vector2Int[] _hexPositionOffsets;

    public override float GetRadius()
    {
        return _hexes[0].GetRadius();
    }

    public override void SetCoordinates(Vector2Int coords)
    {
        for (int i = 0; i < _hexes.Length; i++)
        {
            _hexes[i].SetCoordinates(_hexPositionOffsets[i] + coords);
        }
    }

    public override Vector2Int[] GetHexPositionOffsets()
    {
        return _hexPositionOffsets;
    }

    public override void RegisterNeighbors()
    {
        throw new System.NotImplementedException();
    }

    public override void InstantiateWalls()
    {
        foreach(var hex in _hexes)
            hex.InstantiateWalls();
    }
}
