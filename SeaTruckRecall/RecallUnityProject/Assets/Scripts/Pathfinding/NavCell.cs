using System;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    /// <summary>
    /// Represents a "Cell" in the 3 dimensional nav grid
    /// </summary>
    [Serializable]
    internal struct NavCell : IEquatable<NavCell>
    {
        internal Vector3 Position;
        internal bool HasColliders;
        internal string Name;

        public bool Equals(NavCell other)
        {
            return Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            return obj is NavCell other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ HasColliders.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}