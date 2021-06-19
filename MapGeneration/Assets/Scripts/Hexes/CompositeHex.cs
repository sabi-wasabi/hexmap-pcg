using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeHex : HexBase
{
    [SerializeField] private Hex[] _hexes;
    [SerializeField] private Vector2Int[] _hexPositionOffsets;

    private void Awake()
    {
        visited = false;
        RegisterRelativeHexes();
    }

    public override float GetRadius()
    {
        return _hexes[0].GetRadius();
    }

    public override void SetCoordinates(Vector2Int coordinates)
    {
        this.coords = coordinates;
        for (int i = 0; i < _hexes.Length; i++)
        {
            _hexes[i].SetCoordinates(_hexPositionOffsets[i] + coordinates);
        }
    }

    public override Vector2Int[] GetHexPositionOffsets()
    {
        return _hexPositionOffsets;
    }

    public override void RegisterNeighbors(Dictionary<Vector2Int, HexBase> hexGrid)
    {
        foreach (var hex in _hexes)
            hex.RegisterNeighbors(hexGrid);
    }

    public override void InstantiateWalls()
    {
        foreach(var hex in _hexes)
            hex.InstantiateWalls();
    }

    public override void SetWall(Vector2Int offSet, bool isActive, Vector2Int otherCoords)
    {
        Vector2Int affectedHexCoords = (otherCoords - offSet) - coords;
        int affectedHexIdx = Array.IndexOf(_hexPositionOffsets, affectedHexCoords);
        _hexes[affectedHexIdx].SetWall(offSet, isActive, otherCoords);

    }

    public override void Visit()
    {
        visited = true;
        foreach (var hex in _hexes) hex.visited = true;

        bool hexesWithUnvisitedNeighborsLeft = true;
        while (hexesWithUnvisitedNeighborsLeft)
        {
            List<Hex> hexesWithUnvisitedNeighbors = new List<Hex>();
            foreach (var hex in _hexes)
            {
                if (hex.GetUnvisitedNeighbors().Length > 0)
                {
                    hexesWithUnvisitedNeighbors.Add(hex);
                }
            }

            if (hexesWithUnvisitedNeighbors.Count > 0)
            {
                var randomHex = hexesWithUnvisitedNeighbors[UnityEngine.Random.Range(0, hexesWithUnvisitedNeighbors.Count)];
                randomHex.Visit();
            }
            else
            {
                hexesWithUnvisitedNeighborsLeft = false;
            }
        }
    }

    private void RegisterRelativeHexes()
    {
        for (int i = 0; i < _hexes.Length; i++)
        {
            List<Vector2Int> friendOffsets = new List<Vector2Int>();
            for (int j = 0; j < _hexPositionOffsets.Length; j++)
            {
                friendOffsets.Add(_hexPositionOffsets[j] - _hexPositionOffsets[i]);
            }
            _hexes[i].SetSelfOffsets(friendOffsets.ToArray());
        }
    }
}
