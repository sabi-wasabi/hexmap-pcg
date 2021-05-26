using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HexBase : MonoBehaviour
{
    public abstract float GetRadius();
    public abstract void SetCoordinates(Vector2Int coords);
    public abstract Vector2Int[] GetHexPositionOffsets();
}
