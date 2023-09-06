using DaftAppleGames.SubnauticaPets.CustomObjects;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{
    /// <summary>
    /// Simple MonoBehaviour class to manage Pet DNA collectible behaviour
    /// </summary>
    internal class PetDna : MonoBehaviour
    {
        private void Start()
        {
            // Subscribe to the pickupable event, and show the DataBank entry on pickup
            Pickupable pickupable = GetComponent<Pickupable>();
        }

        /// <summary>
        /// Adds the Pet DNA databank entry to players PDA
        /// </summary>
        public void OnPickupDna()
        {
            DatabankEntries.AddPetDnaDataBankEntry();
        }
    }
}
