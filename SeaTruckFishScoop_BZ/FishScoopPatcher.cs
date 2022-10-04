using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;


namespace SeaTruckFishScoop_BZ
{
    class FishScoopPatcher
    {
        [HarmonyPatch(typeof(SeaTruckMotor))]
        [HarmonyPatch("Start")]
        internal class AddFishScoop
        {
            /// <summary>
            /// Add a FishScoop to every spawned SeaTruck
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            public static void AddFishScoopToSeaTruck(SeaTruckMotor __instance)
            {
                __instance.gameObject.AddComponent<FishScoopComponent>();
            }
        }

        [HarmonyPatch(typeof(LiveMixin))]
        [HarmonyPatch("TakeDamage")]
        internal class VehicleCollisionMod
        {
            /// <summary>
            /// Here, we're prefixing the TakeDamage method to intecept damage being dealt to a
            /// Creature by the SeaTruck cab.
            /// For context, "taker" is the object taking damage, "dealer" is the object dealing damage.
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="dealer"></param>
            [HarmonyPrefix]
            public static bool TakeDamage(LiveMixin __instance, GameObject dealer = null)
            {
     
                if (dealer == null)
                {
                    return true;
                }

                // Get the root context of the damage taker
                GameObject taker = __instance.gameObject;
                Logger.Log(Logger.Level.Debug, $"Damage: {dealer.name} did damage to: {taker.name}");
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
                Logger.Log(Logger.Level.Debug, $"Dealer root: {rootDealer.name}. Taker root: {rootTaker.name}");

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
                FishScoopComponent fishScoop = __instance.gameObject.GetComponent<FishScoopComponent>();
                bool scoopSuccess = fishScoop.Scoop(rootTaker);
                return !scoopSuccess;
            }


        }
    }
}

