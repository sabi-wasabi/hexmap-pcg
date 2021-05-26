using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public enum Orientation
    {
        East = 0,
        NorthEast = 60,
        NorthWest = 120,
        West = 180,
        SouthWest = 240,
        SouthEast = 300,
    }

    [SerializeField] private GameObject _wallPF;
    [SerializeField] private bool[] _wallSettingsModifier = new bool[6];

    private Transform _wallParent;
    private (Orientation, bool)[] _wallSettings = new (Orientation, bool)[6]; // degrees / isWallThere

    private float _hexRadius; // too short???
    private float _apothem;

    void Awake()
    {
        _wallParent = transform.Find("Walls");

        _hexRadius = transform.parent.GetComponent<HexManager>().GetRadius();
        _apothem = Mathf.Sqrt(Mathf.Pow(_hexRadius, 2) - Mathf.Pow((_hexRadius / 2), 2));

        _wallSettings = new (Orientation, bool)[]
        {
            (Orientation.East, _wallSettingsModifier[0]),
            (Orientation.NorthEast, _wallSettingsModifier[1]),
            (Orientation.NorthWest, _wallSettingsModifier[2]),
            (Orientation.West, _wallSettingsModifier[3]),
            (Orientation.SouthWest, _wallSettingsModifier[4]),
            (Orientation.SouthEast, _wallSettingsModifier[5])
        };
    }

    public void InstantiateWalls()
    {
        foreach(var wallSetting in _wallSettings)
        {
            if(wallSetting.Item2)
            {
                var wall = Instantiate(_wallPF, transform.position, Quaternion.identity);
                wall.transform.localScale = new Vector3(1, 1, _hexRadius);

                var pos = new Vector3(wall.transform.position.x, wall.transform.position.y + wall.transform.localScale.y, wall.transform.position.z);

                var movement = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * (float)wallSetting.Item1),
                    0,
                    Mathf.Sin(Mathf.Deg2Rad * (float)wallSetting.Item1)
                ).normalized;

                wall.transform.position = pos + movement * _apothem;

                wall.transform.Rotate(Vector3.up, -(int)wallSetting.Item1);

                wall.transform.SetParent(_wallParent);
            }
        }
    }

    public void SetWall(Orientation orientation, bool isActive)
    {
        int idx = (int)orientation / 60;
        _wallSettings[idx].Item2 = isActive;

    }
}
