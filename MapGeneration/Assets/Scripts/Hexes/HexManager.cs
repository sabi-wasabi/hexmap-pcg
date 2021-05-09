using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexManager : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private GameObject[] _hexes;
    [SerializeField] private Vector2Int[] _hexDirections;
    [SerializeField] private Vector2Int[] _neighborDirections;

    private void Start()
    {
        RegisterNeighbors();
    }

    private void RegisterNeighbors()
    {
        if (_hexes.Length == 0 || _hexes.Length != _hexDirections.Length)
            Debug.LogError("Hexes not registered in the editor");

        List<Vector2Int> _neighbors = new List<Vector2Int>();
        foreach (var hex in _hexDirections)
        {
            for (int q = -1; q <= 1; q++)
            {
                for (int r = -1; r <= 1; r++)
                {
                    if (q == r) continue;

                    Vector2Int direction = new Vector2Int(hex.x + q, hex.y + r);
                    if (!_hexDirections.Contains(direction) &&
                        !_neighbors.Contains(direction))
                    {
                        _neighbors.Add(direction);
                    }
                }
            }
        }
        _neighborDirections = _neighbors.ToArray();
    }

    public void SetCoordinates(Vector2Int origin)
    {
        for(int i = 0; i < _hexes.Length; i++)
        {
            int q = origin.x + _hexDirections[i].x;
            int r = origin.y + _hexDirections[i].y;
            _hexes[i].GetComponent<Coordinates>().SetCoords(q, r);
        }
    }

    public Vector2Int GetRandomNeighbor()
    {
        return _neighborDirections[Random.Range(0, _neighborDirections.Length)];
    }

    public Vector2Int[] GetHexDirections()
    {
        return _hexDirections;
    }

    public float GetRadius()
    {
        return _radius;
    }
}
