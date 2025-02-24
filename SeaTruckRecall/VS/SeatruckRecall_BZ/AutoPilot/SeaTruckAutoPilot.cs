using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    /// <summary>
    /// MonoBehavior implementing SeaTruck specific AutoPilot behavior
    /// the game.
    /// </summary>
    internal class SeaTruckAutoPilot : AutoPilot
    {
        internal UnityEvent OnDockedEvent = new UnityEvent();

        
    }
}