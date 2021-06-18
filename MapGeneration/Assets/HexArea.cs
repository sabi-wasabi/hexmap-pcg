using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexBase))]
public class HexArea : MonoBehaviour
{
    private readonly List<HexAgent> _currentAgents = new List<HexAgent>();
    private HexBase _hex = default;

    public HexAgent[] Agents => _currentAgents.ToArray();

    private void Awake()
    {
        _hex = GetComponent<HexBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HexAgent agent))
        {
            _currentAgents.Add(agent);
            agent.EnterHex(_hex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HexAgent agent))
        {
            _currentAgents.Remove(agent);
            agent.LeaveHex(_hex);
        }
    }
}
