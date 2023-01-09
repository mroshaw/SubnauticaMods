using HarmonyLib;
using UnityEngine;

namespace Mroshaw.SeaTruckFishScoopMod_BZ
{
    /// <summary>
    /// Patches for the SeaTruck Fish Scoop Mod
    /// </summary>
    public class SeaTruckFishScoopMod
    {
        /// <summary>
        /// Patch the SeaTruckMotor class
        /// </summary>
        [HarmonyPatch(typeof(SeaTruckMotor))]
        internal class SeaTruckMotor_Patch
        {
            /// <summary>
            /// Add a FishScoop to every spawned SeaTruck
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPatch(nameof(SeaTruckMotor.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(SeaTruckMotor __instance)
            {
                __instance.gameObject.AddComponent<SeaTruckFishScoopComponent>();
            }
        }

        /// <summary>
        /// Patch the LiveMixin class
        /// </summary>
        [HarmonyPatch(typeof(LiveMixin))]
        internal class LiveMixin_Patch
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
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Damage: {dealer.name} did damage to: {taker.name}");
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
                SeaTruckFishScoopPlugin_BZ.Log.LogDebug($"Dealer root: {rootDealer.name}. Taker root: {rootTaker.name}");

                // Let's see if whatever dealt the damage was a SeaTruck main cab
                SeaTruckSegment seaTruckSegment = rootDealer.GetComponent<SeaTruckSegment>();
                if (seaTruckSegment == null)
                {
                    return true;
                }
                if (!seaTruckSegment.isMainCab)
                {
                    return true;
                }

                // Invoke the might of the scoop
                SeaTruckFishScoopComponent fishScoop = __instance.gameObject.GetComponent<SeaTruckFishScoopComponent>();
                bool scoopSuccess = fishScoop.Scoop(rootTaker);
                return !scoopSuccess;
            }
        }
    }
}

