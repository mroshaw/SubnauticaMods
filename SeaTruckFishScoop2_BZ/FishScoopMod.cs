using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;


namespace SeaTruckFishScoop_BZ
{
    class FishScoopMod
    {
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
                // Check our config
                if (!QModHelper.Config.EnableFishScoop)
                {
                    Logger.Log(Logger.Level.Debug, "Fish Scoop is disabled.");
                    return true;
                }

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
                Logger.Log(Logger.Level.Debug, "SeaTruck cab is dealer");

                // Check if piloting against the config options
                SeaTruckMotor seaTruckMotor = rootDealer.GetComponent<SeaTruckMotor>();
                bool isPiloted = seaTruckMotor.IsPiloted();
                Logger.Log(Logger.Level.Debug, $"SeaTruckIsPiloted: {isPiloted}");
                float velicityMagnitude = seaTruckMotor.useRigidbody.velocity.magnitude;
                Logger.Log(Logger.Level.Debug, $"Velocity.Magnitude: {velicityMagnitude}");

                // Check if seatruck is being piloted
                if (!isPiloted && !QModHelper.Config.ScoopwhileNotPiloting)
                {
                    Logger.Log(Logger.Level.Debug, $"SeaTruck not being piloted, isPiloted: {isPiloted}, ScoopwhileNotPiloting: {QModHelper.Config.ScoopwhileNotPiloting}. No fish scoop!");
                    return true;
                }

                // Check if static against the config options
                if ((velicityMagnitude == 0.0f) && !QModHelper.Config.ScoopWhileStatic)
                {
                    Logger.Log(Logger.Level.Debug, $"SeaTruck is static, velocity.Magnitude: {velicityMagnitude}, ScoopWhileStatic: {QModHelper.Config.ScoopWhileStatic}. No fish scoop!");
                    return true;
                }

                // Let's see if what took the damage was a compatible aquarium fish
                if (!IsValidObject(rootTaker))
                {
                    Logger.Log(Logger.Level.Debug, "Not a valid fish. No fish scoop!");
                    return true;
                }

                // We've passed our checks, now try to add the fish
                Logger.Log(Logger.Level.Debug, "Taker is a supported fish");
                bool fishAdded = AquariumsMod.AddFishToFreeAquarium(seaTruckMotor, rootTaker);
                Logger.Log(Logger.Level.Debug, $"Status of AddFish: {fishAdded}, return value for AddFishToFreeAquarium: {!fishAdded}");
                return !fishAdded;
            }

            /// <summary>
            /// Is the object hit valid for inclusion in the Aquarium?
            /// </summary>
            /// <param name="takerGameObject"></param>
            /// <returns></returns>
            private static bool IsValidObject(GameObject takerGameObject)
            {
                if (!takerGameObject.GetComponent<AquariumFish>())
                {
                    return false;
                }
                WaterParkCreature waterParkCreature = takerGameObject.GetComponent<WaterParkCreature>();
                if (waterParkCreature && waterParkCreature.IsInsideWaterPark())
                {
                    return false;
                }
                return true;
            }
        }
    }
}

