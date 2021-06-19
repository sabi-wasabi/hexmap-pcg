using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    private List<HexBase> _allHexes = new List<HexBase>();

    private void Start()
    {
        _hexSize = _basicHex.GetComponent<HexBase>().GetRadius();
        GenerateMap();

        if (TryGetComponent(out NavMeshSurface navMesh))
        {
            navMesh.BuildNavMesh();
        }
    }

    private void GenerateMap()
    {
        // Create HexGrid
        Vector2Int[] positions = _gridGenerator.GenerateMapGrid(_mapSize);
        List<(Vector2Int, GameObject)> hexPositionsAndPrefab = new List<(Vector2Int, GameObject)>();

        // Modify HexGrid
            // Combine Positions for bigger special CompositeHexes
            // Fill the rest up with normal hexes
        foreach (var compositeHex in _specialHexes)
        {
            var specialPos = compositeHex.GetComponent<HexPlacement>().PlaceSelf(ref positions);
            hexPositionsAndPrefab.Add((specialPos, compositeHex));
        }

        foreach (var pos in positions)
        {
            hexPositionsAndPrefab.Add((pos, _basicHex));
        }

        // Instantiate Hexes
            // Register Neighbors
            // Setup all Walls for backtracking
        InstantiateHexes(hexPositionsAndPrefab);

        // Recursive Backtracking for Wall Placement
        _hexGrid.ElementAt(Random.Range(0, _hexGrid.Count)).Value.Visit();


        // Instantiate Walls
        foreach(var hex in _allHexes)
            hex.InstantiateWalls();
    }

    private void InstantiateHexes(List<(Vector2Int, GameObject)> hexPositionAndPrefab)
    {
        foreach (var hex in hexPositionAndPrefab)
        {
            Spawn(hex.Item1, hex.Item2);
        }

        foreach (var hex in _allHexes)
        {
            hex.RegisterNeighbors(_hexGrid);
        }
    }

    private void Spawn(Vector2Int position, GameObject prefab)
    {
        var pos = HexUtility.HexGridToWorld(position, _hexSize, transform.position.y);
        var hexGO = Instantiate(prefab, pos, prefab.transform.rotation, transform);

        var hex = hexGO.GetComponent<HexBase>();
        hex.SetCoordinates(position);

        _allHexes.Add(hex);
        var offsets = hex.GetHexPositionOffsets();
        foreach(var offset in offsets)  
            _hexGrid.Add(position + offset, hex);
    }
}
