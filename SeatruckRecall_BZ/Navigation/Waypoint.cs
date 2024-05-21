using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    // Controls when to rotate on a given Waypoint
    internal enum WaypointAction { RotateOnMove, RotateBeforeMove }

    /// <summary>
    /// Internal Waypoint class definition.
    /// </summary>
    internal class Waypoint
    {
        // Target transform
        internal Transform Transform { get; }

        // Whether to rotate while moving or before moving
        internal WaypointAction Action { get; }

        // Waypoint name for useful feedback
        internal string Name { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="action"></param>
        internal Waypoint(Transform transform, WaypointAction action, string name)
        {
            Transform = transform;
            Action = action;
            Name = name;
        }
    }
}
