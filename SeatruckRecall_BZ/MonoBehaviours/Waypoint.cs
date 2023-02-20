using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours
{
    // Controls when to rotate on a given Waypoint
    internal enum WaypointAction { RotateOnMove, RotateBeforeMove }

    /// <summary>
    /// Internal Waypoint class definition.
    /// </summary>
    internal class Waypoint
    {
        // Target transform
        public Transform Transform { get; set; }

        // Whether to rotate while moving or once at target
        public WaypointAction Action { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="action"></param>
        internal Waypoint(Transform transform, WaypointAction action)
        {
            Transform = transform;
            Action = action;
        }
    }
}
