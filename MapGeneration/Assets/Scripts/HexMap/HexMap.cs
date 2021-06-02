using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    [Header("Decks")]
    [SerializeField] private GameObject _basicHex;
    [SerializeField] private GameObject[] _specialHexes;

    [Header("Settings")]
    [SerializeField] private int _mapSize;
    [SerializeField] private GridGenerator _gridGenerator;
    
    private float _hexSize;
    private Dictionary<Vector2Int, HexBase> _hexGrid = new Dictionary<Vector2Int, HexBase>();

    private void Start()
    {
        _hexSize = _basicHex.GetComponent<HexBase>().GetRadius();
        GenerateMap();
    }

    private void GenerateMap()
    {
        // Create HexGrid
        Vector2Int[] positions = _gridGenerator.GenerateMapGrid(_mapSize);
        List<(Vector2Int, GameObject)> specialHexPositionAndPrefab = new List<(Vector2Int, GameObject)>();

        // Modify HexGrid
        // Combine Positions for bigger special CompositeHexes

        foreach (var compositeHex in _specialHexes)
        {
            var specialPos = compositeHex.GetComponent<HexPlacement>().PlaceSelf(ref positions);
            specialHexPositionAndPrefab.Add((specialPos, compositeHex));
        }

        // Make some Noise (Put wholes in the map with some Noise texture)


        // Instantiate Hexes
        InstantiateHexes(positions, specialHexPositionAndPrefab);

        // Recursive Backtracking for Wall Placement

        // Instantiate Walls

    }

    private void InstantiateHexes(Vector2Int[] basicPositions, List<(Vector2Int, GameObject)> specialHexPositionAndPrefab)
    {
        foreach (var specialHex in specialHexPositionAndPrefab)
        {
            Spawn(specialHex.Item1, specialHex.Item2);
        }

        foreach (var position in basicPositions)
        {
            Spawn(position, _basicHex);
        }
    }

    private void Spawn(Vector2Int position, GameObject prefab)
    {
        var pos = HexUtility.HexGridToWorld(position, _hexSize, transform.position.y);
        var hexGO = Instantiate(prefab, pos, prefab.transform.rotation, transform);

        var hex = hexGO.GetComponent<HexBase>();
        hex.SetCoordinates(position);
        _hexGrid.Add(position, hex);
    }
    
    //private void InstantiateWalls()
    //{
    //    foreach(var hex in _hexMap)
    //    {
    //        var wallManagers = hex.Value.GetComponentsInChildren<WallManager>();
    //        foreach(var wallManager in wallManagers)
    //        {
    //            wallManager.InstantiateWalls();
    //        }
    //    }
    //}
}
