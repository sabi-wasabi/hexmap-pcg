using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hex : HexBase
{
    [SerializeField] private float _radius;
    private float _apothem;

    [SerializeField] private float _wallScaleX;
    [SerializeField] private float _wallScaleY;
    [SerializeField] private GameObject _wallPF;

    private List<(HexBase, Vector2Int)> _neighbors = new List<(HexBase, Vector2Int)>();
    private Vector2Int[] _relatives = new Vector2Int[]{ new Vector2Int(0,0)};

    private Transform _wallParent;
    private (Vector2Int, bool)[] _wallSettings = new (Vector2Int, bool)[6]; // Offset to neighbor in that direction, isWallThere?

    private static readonly Dictionary<Vector2Int, float> _wallOrientation = new Dictionary<Vector2Int, float>()
    {
        {new Vector2Int(1,0), 0},
        {new Vector2Int(0,1), 60},
        {new Vector2Int(-1,1), 120},
        {new Vector2Int(-1,0), 180},
        {new Vector2Int(0,-1), 240},
        {new Vector2Int(1,-1), 300}
    };

    public void Awake()
    {
        visited = false; 
        _wallParent = transform.Find("Walls");
        _apothem = Mathf.Sqrt(Mathf.Pow(_radius, 2) - Mathf.Pow((_radius / 2), 2));

        for (int i = 0; i < _wallSettings.Length; i++)
        {
            _wallSettings[i].Item1 = _wallOrientation.ElementAt(i).Key;
            _wallSettings[i].Item2 = true;
        }
    }

    public override float GetRadius()
    {
        return _radius;
    }

    public override void SetCoordinates(Vector2Int coords)
    {
        this.coords = coords;
        GetComponent<DebugCoordinates>().SetCoordinates(coords);
    }

    public override Vector2Int[] GetHexPositionOffsets()
    {
        return new[] { new Vector2Int(0, 0) };
    }

    public override void RegisterNeighbors(Dictionary<Vector2Int, HexBase> hexGrid)
    {
        for (int q = -1; q <= 1; q++)
        {
            for (int r = -1; r <= 1; r++)
            {
                if (q != r)
                {
                    Vector2Int offSet = new Vector2Int(q, r);
                    if (hexGrid.ContainsKey(coords + offSet))
                    {
                        if (_relatives.Contains(offSet))
                        {
                            SetWall(offSet, false, coords + offSet);
                        }
                        else
                        {
                            _neighbors.Add((hexGrid[coords + offSet], offSet));
                        }
                    }
                }
            }
        }
    }

    public override void InstantiateWalls()
    {
        foreach (var wallSetting in _wallSettings)
        {
            if (wallSetting.Item2)
            {
                var wall = Instantiate(_wallPF, transform.position, Quaternion.identity);
                wall.transform.localScale = new Vector3(1, _wallScaleY, _radius);

                var pos = new Vector3(wall.transform.position.x, wall.transform.position.y + wall.transform.localScale.y/2, wall.transform.position.z);

                var movement = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * _wallOrientation[wallSetting.Item1]),
                    0,
                    Mathf.Sin(Mathf.Deg2Rad * _wallOrientation[wallSetting.Item1])
                ).normalized;

                wall.transform.position = pos + movement * (_apothem - wall.transform.localScale.x / 2);

                wall.transform.Rotate(Vector3.up, -_wallOrientation[wallSetting.Item1]);

                wall.transform.SetParent(_wallParent);
            }
        }
    }

    public override void Visit()
    {
        visited = true;
        bool unvisitedNeighborsLeft = true;
        while (unvisitedNeighborsLeft)
        {
            var unvisitedNeighbors = GetUnvisitedNeighbors();

            if (unvisitedNeighbors.Length > 0)
            {
                var randomNeighbor = unvisitedNeighbors[UnityEngine.Random.Range(0, unvisitedNeighbors.Length)];

                SetWall(randomNeighbor.Item2, false, coords + randomNeighbor.Item2);
                randomNeighbor.Item1.SetWall(randomNeighbor.Item2 * (-1), false, coords);

                randomNeighbor.Item1.Visit();
            }
            else
            {
                unvisitedNeighborsLeft = false;
            }
        }
    }

    public override void SetWall(Vector2Int offSet, bool isActive, Vector2Int otherCoords)
    {
        _wallSettings[(int) _wallOrientation[offSet] / 60].Item2 = isActive;
    }

    public void SetSelfOffsets(Vector2Int[] self)
    {
        _relatives = self;
    }

    public (HexBase, Vector2Int)[] GetUnvisitedNeighbors()
    {
        List<(HexBase, Vector2Int)> unvisitedNeighbors = new List<(HexBase, Vector2Int)>();
        foreach (var neighbor in _neighbors)
        {
            if(!neighbor.Item1.visited) unvisitedNeighbors.Add(neighbor);
        }
        return unvisitedNeighbors.ToArray();
    }
}
