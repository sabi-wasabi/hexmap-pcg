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

    #region Layouts
    class Layout
    {
        public Layout(float xQ, float xR, float yQ, float yR)
        {
            xModifierQ = xQ;
            xModifierR = xR;
            yModifierQ = yQ;
            yModifierR = yR;
        }

        public float xModifierQ, xModifierR;
        public float yModifierQ, yModifierR;
    }

    private Layout _layoutPointy = new Layout
    (
        Mathf.Sqrt(3f),
        Mathf.Sqrt(3f) / 2,
        0f,
        3f / 2f
    );

    private Layout _layoutFlat = new Layout
    (
        3f / 2f,
        0f,
        Mathf.Sqrt(3f) / 2,
        Mathf.Sqrt(3f)
    );
    #endregion

    private void Start()
    {
        _hexRadius = _hexBase.GetComponent<HexManager>().GetRadius();
        Generate();
    }

    private void Generate()
    {
        // Generate Positions in HexGrid
        _positions = _generator.GenerateMapGrid(_size);

        // Modify Positions with special hexes
        foreach(var specialHex in _specialHexBases)
        {
            var specialPos = specialHex.GetComponent<HexPlacement>().PlaceSelf(ref _positions);
            _specialHexBasePositions.Add(specialPos, specialHex);
        }

        // Spawn Debug Hexes
        SpawnHexes();

        // Create some wholes with noise

        // Recursive Backtracking
    }

    private void SpawnHexes()
    {
        foreach(var specialHex in _specialHexBasePositions)
        {
            var hex = Instantiate(specialHex.Value, GridToWorld(specialHex.Key.x, specialHex.Key.y), specialHex.Value.transform.rotation, transform);
            var hexManager = hex.GetComponent<HexManager>();
            hexManager.SetCoordinates(specialHex.Key);
            _hexMap.Add(specialHex.Key, hexManager);
        }

        foreach (var position in _positions)
        {
            var hex = Instantiate(_hexBase, GridToWorld(position.x, position.y), _hexBase.transform.rotation, transform);
            var hexManager = hex.GetComponent<HexManager>();
            hexManager.SetCoordinates(position);
            _hexMap.Add(position, hexManager);
        }
    }

    #region Utility
    private Vector3 GridToWorld(int q, int r)
    {
        Layout layout = _pointyTop ? _layoutPointy : _layoutFlat;
        return new Vector3
        (
            (layout.xModifierQ * q + layout.xModifierR * r) * _hexRadius,
            transform.position.y,
            (layout.yModifierQ * q + layout.yModifierR * r) * _hexRadius
        );
    }

    private Vector3Int AxialToCubeCoords(Vector2Int coords)
    {
        return new Vector3Int
        (
           coords.x,
           coords.y,
           coords.x * (-1) - coords.y * (-1)
        );
    }

    private Vector2Int CubeToAxialCoords(Vector3Int coords)
    {
        return new Vector2Int
        (
            coords.x,
            coords.y
        );
    }
    #endregion
}
