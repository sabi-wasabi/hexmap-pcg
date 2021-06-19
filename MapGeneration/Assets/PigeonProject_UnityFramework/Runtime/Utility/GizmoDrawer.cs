using PigeonProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawer : MonoBehaviour
{
    private List<GizmoDescriptor> _gizmos = new List<GizmoDescriptor>();

    public void RegisterGizmo(GizmoDescriptor gizmo) => _gizmos.Add(gizmo);

    private void OnDrawGizmos()
    {
        for (int i = _gizmos.Count - 1; i >= 0; i--)
        {
            var gizmo = _gizmos[i];
            gizmo.Duration = gizmo.Duration - Time.deltaTime;
            if (gizmo.Duration <= 0)
                _gizmos.Remove(gizmo);
            else
                Gizmos.DrawWireSphere(gizmo.Position, gizmo.Radius);
        }
    }
}
