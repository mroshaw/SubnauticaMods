using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    /// <summary>
    /// Instant navigation to the destination position and rotation
    /// </summary>
    public class InstantNavigation : MonoBehaviour
    {
        internal void MoveToDestination(List<Waypoint> waypoints)
        {
            if (waypoints == null || waypoints.Count == 0)
            {
                return;
            }

            Vector3 lastPosition = waypoints[waypoints.Count - 1].Position;

            // If only one target, final rotation is direction from source to the last target
            if (waypoints.Count == 1)
            {
                Vector3 direction = lastPosition - transform.position;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            // If more than one target, final rotation is direction from second to last to last target
            else
            {
                Vector3 secondLastPosition = waypoints[waypoints.Count - 2].Position;
                Vector3 direction = secondLastPosition - lastPosition;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            transform.position = lastPosition;

        }
    }
}