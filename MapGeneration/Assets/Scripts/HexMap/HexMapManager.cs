using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapManager : MonoBehaviour
{
    [SerializeField] private GameObject _hexBase; // TODO make this a deck
    [SerializeField] private GameObject[] _specialHexBases; // TODO make this a deck

    [SerializeField] private MapGenerator _generator;
    [SerializeField] private int _size;
    [SerializeField] private bool _pointyTop;
    [SerializeField] private float _seed;

    private float _hexRadius;
    private Vector2Int[] _positions;
    private Dictionary<Vector2Int, GameObject> _specialHexBasePositions = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, HexManager> _hexMap = new Dictionary<Vector2Int, HexManager>();

    private void Start()
    {
        _hexRadius = _hexBase.GetComponent<HexManager>().GetRadius();
        Generate();
    }

    private void Generate()
    {
        // Generate Positions in HexGrid
        _positions = _generator.GenerateMapGrid(_size);

        // Modify Positions with special hex-constellation that consist of multiple hexes (for bigger areas within the map)
        foreach(var specialHex in _specialHexBases)
        {
            var specialPos = specialHex.GetComponent<HexPlacement>().PlaceSelf(ref _positions);
            _specialHexBasePositions.Add(specialPos, specialHex);
        }

        // Create some wholes within the general position with noise

        // Spawn Hexes
        SpawnHexes();

        // Recursive Backtracking
    }

    private void SpawnHexes()
    {
        foreach(var specialHex in _specialHexBasePositions)
        {
            Spawn(specialHex.Key, specialHex.Value);
        }

        foreach (var position in _positions)
        {
            Spawn(position, _hexBase);
        }
    }

    private void Spawn(Vector2Int position, GameObject prefab)
    {
        var pos = HexUtility.HexGridToWorld(position, _hexRadius, transform.position.y, _pointyTop);
        var hex = Instantiate(prefab, pos, prefab.transform.rotation, transform);

        var hexManager = hex.GetComponent<HexManager>();
        hexManager.SetCoordinates(position);
        _hexMap.Add(position, hexManager);
    }
}
