using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoiGuide : MonoBehaviour
{
    protected PoiTracker _tracker = default;

    protected virtual void Awake()
    {
        _tracker = transform.root.GetComponentInChildren<PoiTracker>();
    }
}
