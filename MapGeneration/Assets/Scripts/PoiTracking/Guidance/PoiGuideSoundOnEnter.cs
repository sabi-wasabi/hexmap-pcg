using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PigeonProject;

public class PoiGuideSoundOnEnter : PoiGuide
{
    [SerializeField] Sound _soundEffectCorrect = default;
    [SerializeField] Sound _soundEffectWrong = default;

    private HexBase _currentNext = default;


    private void Start()
    {
        _currentNext = _tracker.NearestPoi.NextHex;
    }


    public void OnEnterHex(HexBase hex)
    {
        if (!enabled)
            return;

        if (hex == _currentNext)
        {
            if (_soundEffectCorrect != null)
                _soundEffectCorrect.Play(gameObject);
        }
        else if (_soundEffectWrong != null)
            _soundEffectWrong.Play(gameObject);

        _currentNext = _tracker.NearestPoi.NextHex;
    }

    public void OnLeaveHex(HexBase hex) => _currentNext = _tracker.NearestPoi.NextHex;


    private void Update() { }   //RM only here so the enable button gets exposed in the editor
}
