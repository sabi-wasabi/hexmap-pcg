using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/HexMapGenerator")]
public class HexagonalGridGenerator : GridGenerator
{
    public override Vector2Int[] GenerateMapGrid(int size)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int q = -size; q <= size; q++)
        {
            int rMin = Mathf.Max(-size, -q - size);
            int rMax = Mathf.Min(size, -q + size);

            for (int r = rMin; r <= rMax; r++)
            {
                positions.Add(new Vector2Int(q, r));
            }
        }

        return positions.ToArray();
    }
}
