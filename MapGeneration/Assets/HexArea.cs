using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexArea : MonoBehaviour
{
    private readonly List<GameObject> _currentAgents = new List<GameObject>();

    public GameObject[] Agents => _currentAgents.ToArray();


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HexAgent agent))
        {
            _currentAgents.Add(agent.gameObject);
            agent.EnterHex(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HexAgent agent))
        {
            _currentAgents.Remove(agent.gameObject);
            agent.LeaveHex(gameObject);
        }
    }
}
