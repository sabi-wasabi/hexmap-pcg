using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : HexBase
{
    [SerializeField] private float _radius;
    private Vector2Int _coords;

    public override float GetRadius()
    {
        return _radius;
    }

    public override void SetCoordinates(Vector2Int coords)
    {
        _coords = coords;
        GetComponent<DebugCoordinates>().SetCoordinates(coords);
    }

    public override Vector2Int[] GetHexPositionOffsets()
    {
        return new []{new Vector2Int(0,0)};
    }

    public override void RegisterNeighbors()
    {
        for (int q = -1; q <= 1; q++)
        {
            for (int r = -1; r <= 1; r++)
            {
                if (q == r) continue;

                Vector2Int position = new Vector2Int(_coords.x + q, _coords.y + r);
            }
        }
    }
}
