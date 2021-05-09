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
        // TODO Split into smaller functions

        // Modify the valid positions - eg. only x away from spawn etc.
        Vector2Int[] validPositions = Modify(positions);

        // Check if the hex constellation would fit
        validPositions = CheckSize(validPositions);

        // Pick one of the valid positions randomly and remove the occupied fields from positions
        var finalPosition = validPositions[Random.Range(0, validPositions.Length)];

        // Remove the newly occupied fields from all positions
        positions = RemovePositions(finalPosition, positions);

        // TODO what to do if no position valid?

        return finalPosition;
    } 

    private Vector2Int[] Modify(Vector2Int[] positions)
    {
        // TODO Implement placement modifiers
        return positions;
    }

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
