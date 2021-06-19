using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour
{
    private readonly List<HexBase> _currentHexes = new List<HexBase>();

    public HexBase[] OverlappingHexes => _currentHexes.ToArray();
    public HexBase CurrentHex => _currentHexes.Count == 0 ? null : _currentHexes[_currentHexes.Count - 1];


    public void EnterHex(HexBase hex)
    {
        _currentHexes.Add(hex);
        SendMessage("OnEnterHex", hex, SendMessageOptions.DontRequireReceiver);
    }

    public void LeaveHex(HexBase hex)
    {
        if (_currentHexes.Remove(hex))
            SendMessage("OnLeaveHex", hex, SendMessageOptions.DontRequireReceiver);
    }
}
