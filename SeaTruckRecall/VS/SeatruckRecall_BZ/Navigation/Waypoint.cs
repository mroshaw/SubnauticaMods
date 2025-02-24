using System;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{

    /// <summary>
    /// Internal Waypoint class definition.
    /// </summary>
    [Serializable]
    internal class Waypoint
    {
        // Target transform
        internal Transform Transform { get; }

        // Whether to rotate while moving or before moving
        internal bool RotateBeforeMoving { get; }

        // Waypoint name for useful feedback
        internal string Name { get; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        internal Waypoint(Transform transform, bool rotateBeforeMoving, string name)
        {
            Transform = transform;
            RotateBeforeMoving = rotateBeforeMoving;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}:, Pos: {Transform.position}, Rot: {Transform.rotation}";
        }
    }
}