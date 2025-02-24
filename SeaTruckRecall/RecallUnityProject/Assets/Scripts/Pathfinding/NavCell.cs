using System;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    [Serializable]
    public struct NavCell
    {
        public Vector3 Position;
        public bool hasColliders;
        public string Name;
    }
}