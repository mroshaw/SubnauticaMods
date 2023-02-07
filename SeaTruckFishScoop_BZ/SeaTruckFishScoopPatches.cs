using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ
{
    /// <summary>
    /// Patches for the SeaTruck Fish Scoop Mod
    /// </summary>
    public class SeaTruckFishScoopPatches
    {
        /// <summary>
        /// Patch the SeaTruckMotor class
        /// </summary>
        [HarmonyPatch(typeof(SeaTruckSegment))]
        internal class SeaTruckSegmentPatch
        {
            /// <summary>
            /// Add a FishScoop to every spawned SeaTruck
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPatch(nameof(SeaTruckSegment.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(SeaTruckSegment __instance)
            {
                if (__instance.isMainCab)
                {
                    SeaTruckFishScoopPluginBz.Log.LogDebug("Adding SeaTruckFishScoopComponent...");
                    __instance.gameObject.AddComponent<SeaTruckFishScoop>();
                    SeaTruckFishScoopPluginBz.Log.LogDebug("SeaTruckFishScoopComponent added.");
                }
            }
        }

        /// <summary>
        /// Patch the LiveMixin class
        /// </summary>
        [HarmonyPatch(typeof(LiveMixin))]
        internal class LiveMixinPatch
        {
            /// <summary>
            /// Here, we're prefixing the TakeDamage method to intecept damage being dealt to a
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
                if(fishScoop!= null)
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
}

