/*
 using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using HarmonyLib;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
namespace DaftAppleGames.SubnauticaPets.Patches
{

    /// <summary>
    /// Patches Pickupable to handle functions relating to Pet DNA and the Databank
    /// </summary>
    [HarmonyPatch(typeof(Pickupable))]
    internal class PickupablePatches
    {
        /// <summary>
        /// Patches the Pickup meothd
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Pickupable.Pickup))]
        [HarmonyPostfix]
        public static void Pickup_Postfix(Pickupable __instance, bool events)
        {
            // If we've picked up PetDNA, trigger the Databank entry
            PetDna petDna = __instance.gameObject.GetComponent<PetDna>();
            if (petDna)
            {
                Log.LogDebug("Pickupable: in Pickup postfix, calling DataBank method...");
                petDna.OnPickupDna();
                Log.LogDebug("Pickupable: in Pickup postfix, calling DataBank method... Done.");

            }
        }
    }
}
*/