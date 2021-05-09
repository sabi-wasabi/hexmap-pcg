using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexPlacement : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] _placementModifiers;

    private Vector2Int[] _hexDirections { get { return GetComponent<HexManager>().GetHexDirections(); } }

    public Vector2Int PlaceSelf(ref Vector2Int[] positions)
    {
        Vector2Int[] validPositions = Modify(positions);
        validPositions = CheckSize(validPositions);

        // TODO what to do if no position is valid?

        var finalPosition = validPositions[Random.Range(0, validPositions.Length)];
        positions = RemovePositions(finalPosition, positions);

        return finalPosition;
    } 

    // sort out the available positions according to rules set with scriptable objects to allow for a lot of variation
    private Vector2Int[] Modify(Vector2Int[] positions)
    {
        // TODO Implement placement modifiers
        return positions;
    }

    // check for positions where the constellation of hexes fits
    private Vector2Int[] CheckSize(Vector2Int[] validPositions)
    {
        List<Vector2Int> newValidPositions = new List<Vector2Int>();
        foreach (var pos in validPositions)
        {
            bool fits = true;
            foreach (var hexDir in _hexDirections)
            {
                if (!validPositions.Contains(pos + hexDir)) 
                    fits = false;
                
            }
            if (fits) newValidPositions.Add(pos);
        }
        return newValidPositions.ToArray();
    }

    // remove the newly occupied spaces from the overall available positions
    private Vector2Int[] RemovePositions(Vector2Int finalPos, Vector2Int[] positions)
    {
        List<Vector2Int> newPositions = positions.ToList();

        foreach(var hexDir in _hexDirections)
        {
            newPositions.Remove(finalPos + hexDir);
        }

        return newPositions.ToArray();
    }
}
