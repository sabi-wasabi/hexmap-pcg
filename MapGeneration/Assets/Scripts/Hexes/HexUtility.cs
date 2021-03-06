using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class HexUtility
{
    // A static class for various hex-related conversions and calculations
    public class Layout
    {
        public Layout(float xQ, float xR, float yQ, float yR)
        {
            xModifierQ = xQ;
            xModifierR = xR;
            yModifierQ = yQ;
            yModifierR = yR;
        }

        public float xModifierQ, xModifierR;
        public float yModifierQ, yModifierR;
    }

    private static readonly Layout LayoutPointy = new Layout
    (
        Mathf.Sqrt(3f),
        Mathf.Sqrt(3f) / 2,
        0f,
        3f / 2f
    );

    public static Vector3 HexGridToWorld(Vector2Int position, float radius, float yPos)
    {
        return HexGridToWorld(position.x, position.y, radius, yPos);
    }

    public static Vector3 HexGridToWorld(int q, int r, float radius, float yPos)
    {
        Layout layout = LayoutPointy;
        return new Vector3
        (
            (layout.xModifierQ * q + layout.xModifierR * r) * radius,
            yPos,
            (layout.yModifierQ * q + layout.yModifierR * r) * radius
        );
    }

    public static Vector3Int AxialToCubeCoords(Vector2Int coords)
    {
        return new Vector3Int
        (
           coords.x,
           coords.y,
           coords.x * (-1) - coords.y * (-1)
        );
    }

    public static Vector2Int CubeToAxialCoords(Vector3Int coords)
    {
        return new Vector2Int
        (
            coords.x,
            coords.y
        );
    }
}
