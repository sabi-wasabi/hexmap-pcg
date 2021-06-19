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

    /// <summary>
    /// Unordered Gameobject set with all registered POIs.
    /// </summary>
    public GameObject[] PoiSet => _poiSet.Items.ToArray();
    /// <summary>
    /// POI tracking data sorted by path distance.
    /// </summary>
    public PoiTrackingContext[] PoisSortedByDistance => _sortedPaths.Values.ToArray();
    /// <summary>
    /// POI tracking data for the nearest POI.
    /// </summary>
    public PoiTrackingContext NearestPoi => PoisSortedByDistance.Count() > 0 ? PoisSortedByDistance[0] : new PoiTrackingContext();


    private void Awake()
    {
        _agent = GetComponent<HexAgent>();
    }

    private void OnDrawGizmos()
    {
        if (_debugDrawPaths)
        {
            bool shortest = true;
            foreach (var ctx in PoisSortedByDistance)
            {
                Gizmos.color = shortest ? Color.cyan : Color.blue;
                for (int i=1; i < ctx.Path.corners.Length; i++)
                {
                    Gizmos.DrawLine(ctx.Path.corners[i - 1], ctx.Path.corners[i]);
                    Gizmos.DrawCube(ctx.Path.corners[i], Vector3.one * .5f);
                }

                Gizmos.color = shortest ? Color.magenta : Color.red;
                if (ctx.NextHex != null)
                    Gizmos.DrawSphere(ctx.NextHex.transform.position, shortest ? 4f : 3f);
                
                shortest = false;
            }
        }
    }


    public void OnEnterHex(HexBase _) => Refresh();

    public void OnLeaveHex(HexBase _) => Refresh();


    private void Refresh()
    {
        _sortedPaths.Clear();

        foreach (var poi in _poiSet.Items)
        {
            var poiHex = poi.GetComponent<HexBase>();
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(_agent.CurrentHex.transform.position, poiHex.GetWorldPosition(), NavMesh.AllAreas, path))
            {
                PoiTrackingContext ctx = new PoiTrackingContext
                {
                    PoiHex = poiHex,
                    NextHex = poiHex,
                    Path = path,
                    HasArrived = false,
                };

                ctx.Distance = CalculatePathDistance(path);

                if (poiHex == _agent.CurrentHex)
                {
                    ctx.HasArrived = true;
                    ctx.NextHex = poiHex;
                }
                else if (_agent.CurrentHex.TryGetComponent(out Hex currentHex) && path.corners.Length >= 2)
                {
                    Vector3 direction3D = path.corners[1] - path.corners[0];
                    Vector2 direction2D = new Vector2(direction3D.x, direction3D.z);
                    Vector2Int directionHex = HexUtility.Vector2ToHexCoordsDirection(direction2D);
                    if (currentHex.Neighbors.ContainsKey(directionHex))
                    {
                        HexBase neighbor = currentHex.Neighbors[directionHex];
                        ctx.NextHex = neighbor;
                    }
                }

                _sortedPaths.Add(ctx.Distance, ctx);
            }
        }
    }


    public struct PoiTrackingContext
    {
        public HexBase PoiHex;
        public HexBase NextHex;
        public NavMeshPath Path;
        public bool HasArrived;
        public float Distance;
    }


    /// <summary>
    /// Calculate traveling distance for a Nav Mesh Path.
    /// </summary>
    /// <param name="path">the Nav Mesh Path to get the distance from.</param>
    /// <returns>traveling distance of the path.</returns>
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
