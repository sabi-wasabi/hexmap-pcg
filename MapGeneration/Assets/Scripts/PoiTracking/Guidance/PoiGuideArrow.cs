using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiGuideArrow : PoiGuide
{
    [SerializeField] GameObject _arrowPrefab = default;
    [Space()]
    [SerializeField] bool _usePathfinding = false;
    [SerializeField] float _offset = 1f;

    private GameObject _arrow = default;

    public bool UsePathfinding
    {
        get => _usePathfinding;
        set => _usePathfinding = value;
    }


    protected override void Awake()
    {
        base.Awake();

        _arrow = Instantiate(_arrowPrefab, _tracker.transform);
        _arrow.SetActive(enabled);
    }

    private void OnEnable()
    {
        _arrow.SetActive(true);
    }

    private void OnDisable()
    {
        _arrow.SetActive(false);
    }

    private void Update()
    {
        if (_tracker.PoisSortedByDistance.Length > 0 &&
            !_tracker.NearestPoi.HasArrived)
        {
            Vector3 target = _usePathfinding ? _tracker.NearestPoi.NextHex.GetWorldPosition() : _tracker.NearestPoi.PoiHex.GetWorldPosition();
            Vector3 direction = target - _tracker.transform.position;
            direction.Normalize();
            _arrow.transform.localPosition = direction * _offset;
            _arrow.transform.LookAt(target);
        }
    }
}
