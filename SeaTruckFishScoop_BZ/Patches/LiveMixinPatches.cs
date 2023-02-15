using DaftAppleGames.SeaTruckFishScoopMod_BZ.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ.Patches
{
    /// <summary>
    /// Patches for the SeaTruck Fish Scoop Mod
    /// Patches for the LiveMixin class
    /// </summary>
    [HarmonyPatch(typeof(LiveMixin))]
    public class LiveMixinPatches
    {
        /// <summary>
        /// Here, we're prefixing the TakeDamage method to intercept damage being dealt to a
        /// Creature by the SeaTruck cab.
        /// For context, "taker" is the object taking damage, "dealer" is the object dealing damage.
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="dealer"></param>
        [HarmonyPatch(nameof(LiveMixin.TakeDamage))]
        [HarmonyPrefix]
        public static bool TakeDamage_Prefix(LiveMixin __instance, GameObject dealer = null)
        {
            if (dealer == null)
            {
                return true;
            }

            // Get the root context of the damage taker
            GameObject taker = __instance.gameObject;
            SeaTruckFishScoopPluginBz.Log.LogDebug($"Damage: {dealer.name} did damage to: {taker.name}");
            GameObject rootTaker = UWE.Utils.GetEntityRoot(__instance.gameObject);
            if (rootTaker == null)
            {
                rootTaker = taker;
            }

            // Get the root context of the damage dealer
            GameObject rootDealer = UWE.Utils.GetEntityRoot(dealer);
            if (rootDealer == null)
            {
                rootDealer = dealer;
            }
            SeaTruckFishScoopPluginBz.Log.LogDebug($"Dealer root: {rootDealer.name}. Taker root: {rootTaker.name}");

            // Let's see if whatever dealt the damage was a SeaTruck main cab
            SeaTruckSegment seaTruckSegment = rootDealer.GetComponent<SeaTruckSegment>();
            if (seaTruckSegment == null)
            {
                SeaTruckFishScoopPluginBz.Log.LogDebug("SeaTruckSegment is null. No Scoop.");
                return true;
            }
            if (!seaTruckSegment.isMainCab)
            {
                SeaTruckFishScoopPluginBz.Log.LogDebug("SeaTruckSegment is not Main Cab. No Scoop.");
                return true;
            }

            // Invoke the might of the scoop
            SeaTruckFishScoop fishScoop = dealer.gameObject.GetComponent<SeaTruckFishScoop>();
            if (fishScoop != null)
            {
                SeaTruckFishScoopPluginBz.Log.LogDebug("Calling Scoop...");
                bool scoopSuccess = fishScoop.Scoop(rootTaker);
                return !scoopSuccess;
            }
            else
            {
                SeaTruckFishScoopPluginBz.Log.LogDebug("No FishScoop found. No Scoop.");
                return false;
            }
        }
    }
}