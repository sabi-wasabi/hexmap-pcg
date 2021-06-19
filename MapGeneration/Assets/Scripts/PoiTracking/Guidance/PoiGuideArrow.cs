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


    protected override void Awake()
    {
        base.Awake();

        if (enabled)
            _arrow = Instantiate(_arrowPrefab, _tracker.transform);
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
