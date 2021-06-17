using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour
{
    private readonly List<GameObject> _currentHexes = new List<GameObject>();

    public GameObject[] CurrentHexes => _currentHexes.ToArray();


    public void EnterHex(GameObject hex)
    {
        _currentHexes.Add(hex);
        SendMessage("OnEnterHex", hex);
    }

    public void LeaveHex(GameObject hex)
    {
        if (_currentHexes.Remove(hex))
            SendMessage("OnLeaveHex", hex);
    }
}
