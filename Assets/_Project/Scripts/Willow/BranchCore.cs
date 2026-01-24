using UnityEngine;
using WaypointSystem;

namespace _Project.Scripts.Willow
{
    public class BranchCore : MonoBehaviour
    {
        [SerializeField] private Waypoint startWaypoint;

        public Waypoint GetWaypoint() => startWaypoint;
    }
}