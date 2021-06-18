using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour
{
    private readonly List<GameObject> _currentHexes = new List<GameObject>();

    public GameObject[] OverlappingHexes => _currentHexes.ToArray();
    public GameObject CurrentHex => _currentHexes[_currentHexes.Count - 1];


    public void EnterHex(GameObject hex)
    {
        _currentHexes.Add(hex);
        SendMessage("OnEnterHex", hex, SendMessageOptions.DontRequireReceiver);
    }

    public void LeaveHex(GameObject hex)
    {
        if (_currentHexes.Remove(hex))
            SendMessage("OnLeaveHex", hex, SendMessageOptions.DontRequireReceiver);
    }
}
