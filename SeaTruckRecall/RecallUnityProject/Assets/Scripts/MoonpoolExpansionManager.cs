using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    public class MoonpoolExpansionManager : MonoBehaviour
    {
        public bool IsOccupied()
        {
            return false;
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Docked!");
            SeaTruckDockRecaller recaller = GetComponent<SeaTruckDockRecaller>();
            recaller.CurrentSeaTruckDocked();
        }

    }
}