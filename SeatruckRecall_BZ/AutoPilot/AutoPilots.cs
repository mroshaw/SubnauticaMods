﻿using System.Collections.Generic;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    internal static class AutoPilots
    {
        private static readonly List<SeaTruckAutoPilot> AllAutoPilots;
        private static int Count => AllAutoPilots.Count;

        static AutoPilots()
        {
            AllAutoPilots = new List<SeaTruckAutoPilot>();
        }

        /// <summary>
        /// Add a new autopilot
        /// </summary>
        internal static void AddInstance(SeaTruckAutoPilot autoPilot)
        {
            AllAutoPilots.Add(autoPilot);
            Log.LogDebug($"AutoPilot: Registered new instance: {autoPilot.gameObject.name} with MoveMethod: {ConfigFile.RecallMoveMethod.ToString()}");
        }

        /// <summary>
        /// Remove autopilot
        /// </summary>
        internal static void RemoveInstance(SeaTruckAutoPilot autoPilot)
        {
            AllAutoPilots.Remove(autoPilot);
            Log.LogDebug($"DockRecaller: Removed instance: {autoPilot.gameObject.name}");
        }

        /// <summary>
        /// Update all AutoPilot settings (speed)
        /// </summary>
        internal static void UpdateAllSpeed(float speed)
        {
            foreach (SeaTruckAutoPilot autoPilot in AllAutoPilots)
            {
                autoPilot.GetComponent<WaypointNavigation>().MoveSpeed = speed;
            }
        }

        /// <summary>
        /// Update all AutoPilot settings (rotation)
        /// </summary>
        internal static void UpdateAllRotation(float rotation)
        {
            foreach (SeaTruckAutoPilot autoPilot in AllAutoPilots)
            {
                autoPilot.GetComponent<WaypointNavigation>().RotateSpeed = rotation;
            }
        }

        internal static SeaTruckAutoPilot GetClosestAutoPilot(Vector3 sourcePosition, float maxDistance)
        {
            float closestDistance = Mathf.Infinity;
            SeaTruckAutoPilot closestSeaTruck = null;

            if (Count == 0)
            {
                Log.LogInfo("No Seatrucks registered.");
                return null;
            }

            // Loop through each seatruck, find out which is closest
            foreach (SeaTruckAutoPilot seatruck in AllAutoPilots)
            {
                // Check if already docked
                SeaTruckSegment segment = seatruck.GetComponent<SeaTruckSegment>();
                if (segment.isDocked || !seatruck.IsReady())
                {
                    Log.LogDebug($"Seatruck {seatruck.gameObject.name} is already docking or docked. Skipping...");
                    continue;
                }

                Log.LogDebug($"Checking distance to {seatruck.gameObject.name}...");
                float currDistance = Vector3.Distance(sourcePosition, seatruck.gameObject.transform.position);
                {
                    Log.LogDebug($"Distance is: {currDistance}, closest so far is: {closestDistance}");
                    if ((closestDistance == 0 || currDistance < closestDistance) && currDistance <= maxDistance)
                    {
                        Log.LogDebug("New closest Seatruck found!");
                        closestDistance = currDistance;
                        closestSeaTruck = seatruck;
                    }
                }
            }

            // Check to see if we've found anything in range
            Log.LogDebug(closestSeaTruck == null ? $"No SeaTrucks found within range!" : $"Closest Seatruck found: {closestSeaTruck.gameObject.name} at {closestDistance}");

            return closestSeaTruck;
        }
    }
}