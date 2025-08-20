using System;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{

    /// <summary>
    /// Wrapper class for a Waypoint based UnityEvent
    /// </summary>
    internal class WaypointChangedEvent : UnityEvent<Waypoint>
    {
    }

    /// <summary>
    /// Internal Waypoint class definition.
    /// </summary>
    [Serializable]
    internal class Waypoint
    {
        // Target transform
        internal Vector3 Position { get; }
        internal Quaternion Rotation { get; }

        // Whether to rotate while moving or before moving
        internal bool RotateBeforeMoving { get; }

        // Waypoint name for useful feedback
        internal string Name { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        internal Waypoint(Vector3 position, Quaternion rotation, bool rotateBeforeMoving, string name)
        {
            Position = position;
            Rotation = rotation;
            RotateBeforeMoving = rotateBeforeMoving;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}:, Pos: {Position}, Rot: {Rotation}";
        }
    }
}