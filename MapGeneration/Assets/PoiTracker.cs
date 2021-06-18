using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using PigeonProject;

public class PoiTracker : MonoBehaviour
{
    [SerializeField] GameObjectSet _poiSet = default;

    private HexAgent _agent = default;
    private SortedList<float, PoiTrackingContext> _sortedPaths = new SortedList<float, PoiTrackingContext>();

    public GameObject[] PoiSet => _poiSet.Items.ToArray();
    public PoiTrackingContext[] PoisSortedByDistance => _sortedPaths.Values.ToArray();
    public PoiTrackingContext NearestPoi => PoisSortedByDistance[0];


    private void Awake()
    {
        _agent = GetComponent<HexAgent>();
    }


    public void OnEnterHex(GameObject hex)
    {
        // Consider hex at end of list
    }

    public void OnLeaveHex(GameObject hex)
    {
        // Consider hex at start of list
    }


    private void Refresh()
    {
        _sortedPaths.Clear();

        foreach (var poi in _poiSet.Items)
        {
            if (poi == _agent.CurrentHex)
            {
                // Player is on poi hex
            }

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(_agent.CurrentHex.transform.position, poi.transform.position, NavMesh.AllAreas, path))
            {
                float distance = CalculatePathDistance(path);

                //TODO: calculate next hex
                if (_agent.CurrentHex.TryGetComponent(out Hex currentHex))
                {
                    Vector3 direction3D = path.corners[1] - path.corners[0];
                    Vector2 direction2D = new Vector2(direction3D.x, direction3D.z);
                    Vector2Int directionHex = HexUtility.Vector2ToHexCoordsDirection(direction2D);
                    // TOD: get next hex via neighbors of current hex
                }

                PoiTrackingContext ctx = new PoiTrackingContext
                {
                    PoiHex = poi,
                    Path = path,
                };
                _sortedPaths.Add(distance, ctx);
            }
        }
    }


    public struct PoiTrackingContext
    {
        public GameObject PoiHex;
        public GameObject NextHex;
        public NavMeshPath Path;
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
