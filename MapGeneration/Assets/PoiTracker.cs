using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using PigeonProject;

public class PoiTracker : MonoBehaviour
{
    [SerializeField] GameObjectSet _poiSet = default;
    [SerializeField] bool _debugDrawPaths = false;

    private HexAgent _agent = default;
    private SortedList<float, PoiTrackingContext> _sortedPaths = new SortedList<float, PoiTrackingContext>();

    public GameObject[] PoiSet => _poiSet.Items.ToArray();
    public PoiTrackingContext[] PoisSortedByDistance => _sortedPaths.Values.ToArray();
    public PoiTrackingContext NearestPoi => PoisSortedByDistance[0];


    private void Awake()
    {
        _agent = GetComponent<HexAgent>();
    }

    private void OnDrawGizmos()
    {
        if (_debugDrawPaths)
        {
            foreach (var ctx in PoisSortedByDistance)
            {
                Gizmos.color = Color.blue;
                for (int i=1; i < ctx.Path.corners.Length; i++)
                {
                    Gizmos.DrawLine(ctx.Path.corners[i - 1], ctx.Path.corners[i]);
                    Gizmos.DrawCube(ctx.Path.corners[i], Vector3.one * .5f);
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(ctx.NextHex.transform.position, 3f);
            }
        }
    }


    public void OnEnterHex(GameObject _) => Refresh();

    public void OnLeaveHex(GameObject _) => Refresh();


    private void Refresh()
    {
        _sortedPaths.Clear();

        foreach (var poiHex in _poiSet.Items)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(_agent.CurrentHex.transform.position, poiHex.transform.position, NavMesh.AllAreas, path))
            {
                PoiTrackingContext ctx = new PoiTrackingContext
                {
                    PoiHex = poiHex,
                    Path = path,
                };

                ctx.Distance = CalculatePathDistance(path);

                if (poiHex == _agent.CurrentHex)
                {
                    ctx.HasArrived = true;
                    ctx.NextHex = poiHex;
                }
                else
                {
                    ctx.HasArrived = false;
                    if (_agent.CurrentHex.TryGetComponent(out Hex currentHex))
                    {
                        Vector3 direction3D = path.corners[1] - path.corners[0];
                        Vector2 direction2D = new Vector2(direction3D.x, direction3D.z);
                        Vector2Int directionHex = HexUtility.Vector2ToHexCoordsDirection(direction2D);
                        HexBase neighbor = currentHex.Neighbors[directionHex];
                        ctx.NextHex = neighbor.gameObject;
                    }
                }

                _sortedPaths.Add(ctx.Distance, ctx);
            }
        }
    }


    public struct PoiTrackingContext
    {
        public GameObject PoiHex;
        public GameObject NextHex;
        public NavMeshPath Path;
        public bool HasArrived;
        public float Distance;
    }


    public static float CalculatePathDistance(NavMeshPath path)
    {
        float distance = 0f;
        for (int i=1; i<path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return distance;
    }
}
