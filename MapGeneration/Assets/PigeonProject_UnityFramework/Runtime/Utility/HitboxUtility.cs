using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PigeonProject
{
    public static class HitboxUtility
    {
        private const float GIZMO_DRAW_DURATION = .5f;

        static public Collider2D[] CheckHitbox2D(Vector2 center, float radius, int layer, GameObject caller)
        {
#if UNITY_EDITOR
            DrawGizmo(caller, center, radius);
#endif
            List<Collider2D> hitColliders = Physics2D.OverlapCircleAll(center, radius, layer).ToList();

            for (int i=hitColliders.Count-1; i>= 0; i--)
            {
                var collider = hitColliders[i];
                if (collider.gameObject == caller)
                    hitColliders.Remove(collider);
            }

            return hitColliders.ToArray();
        }

        static public Collider[] CheckHitbox3D(Vector3 center, float radius, GameObject caller, int layerMask = Physics.AllLayers)
        {
#if UNITY_EDITOR
            DrawGizmo(caller, center, radius);
#endif
            List<Collider> hitColliders = Physics.OverlapSphere(center, radius, layerMask).ToList();

            for (int i = hitColliders.Count - 1; i >= 0; i--)
            {
                var collider = hitColliders[i];
                if (collider.gameObject == caller)
                    hitColliders.Remove(collider);
            }

            return hitColliders.ToArray();
        }


        private static void DrawGizmo(GameObject gameObject, Vector3 position, float radius)
        {
            if (!gameObject.TryGetComponent(out GizmoDrawer drawer))
                drawer = gameObject.AddComponent<GizmoDrawer>();
            drawer.RegisterGizmo(new GizmoDescriptor()
            {
                Position = position,
                Radius = radius,
                Duration = GIZMO_DRAW_DURATION,
            });
        }
    }

    public class GizmoDescriptor
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public float Duration { get; set; }
    }
}
