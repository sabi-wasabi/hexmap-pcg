using System;
using UnityEngine;

namespace PigeonProject
{
    public static class GeometryUtil
    {
        public static bool ColliderContains(Collider collider, Vector3 point)
        {
            var collisions = Physics.OverlapSphere(point, 0f);
            return Array.IndexOf(collisions, collider) >= 0;
        }

        public static Vector3 ColliderClosestPointFromInside(BoxCollider collider, Vector3 point, bool ignoreX = false, bool ignoreY = false, bool ignoreZ = false)
        {
            Vector3 toPoint = point - collider.transform.position;
            toPoint.Normalize();
            Vector3 extents = MathUtil.Vector3MultiplyMemberwise(collider.transform.localScale, collider.size) * .5f;

            float distanceX = ignoreX ? float.MaxValue : Math.Abs(toPoint.x - extents.x);
            float distanceY = ignoreY ? float.MaxValue : Math.Abs(toPoint.y - extents.y);
            float distanceZ = ignoreZ ? float.MaxValue : Math.Abs(toPoint.z - extents.z);

            var min = Mathf.Min(distanceX, distanceY, distanceZ);

            Vector3 pointOnBorder = point;
            if (min == distanceX)
                pointOnBorder.x = collider.transform.position.x + extents.x * Mathf.Sign(toPoint.x);
            else if (min == distanceY)
                pointOnBorder.y = collider.transform.position.y + extents.y * Mathf.Sign(toPoint.y);
            else
                pointOnBorder.z = collider.transform.position.z + extents.z * Mathf.Sign(toPoint.z);

            return pointOnBorder;
        }
    }
}
