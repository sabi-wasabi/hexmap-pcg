using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PigeonProject;

public class PoiGuideMusicDistance : PoiGuide
{
    [SerializeField] Sound _music = default;
    [Space()]
    [SerializeField] [Min(0)] float _maxDistance = 500f;
    [SerializeField] bool _usePathfinding = true;


    private GameObject _poi = default;
    private AudioSource _audioSource = default;


    private void Start()
    {
        if (!enabled)
            return;

        var poi = _tracker.NearestPoi;
        _audioSource = _music.Play(poi.PoiHex.gameObject);
    }

    private void Update()
    {
        var distance = _usePathfinding ? _tracker.NearestPoi.Distance : Vector3.Distance(_tracker.transform.position, _tracker.NearestPoi.PoiHex.transform.position);
        float relativeCloseness = 1 - (distance / _maxDistance);
        _audioSource.volume = relativeCloseness;
    }
}
