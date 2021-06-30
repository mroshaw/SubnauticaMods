using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;


namespace SeaTruckFishScoop_BZ
{
    class AquariumMod
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
            public static void TakeDamage(LiveMixin __instance, GameObject dealer = null)
            {

                if (dealer == null)
                {
                    return;
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

                // Let's see if what took the damage was a compatible aquarium fish
                if (!IsValidObject(rootTaker))
                {
                    return;
                }
                Logger.Log(Logger.Level.Debug, $"Taker is a supported fish");

                // Let's see if whatever dealt the damage was a SeaTruck main cab
                SeaTruckSegment seaTruckSegment = rootDealer.GetComponent<SeaTruckSegment>();
                if (seaTruckSegment == null)
                {
                    return;
                }
                if (!seaTruckSegment.isMainCab)
                {
                    return;
                }
                Logger.Log(Logger.Level.Debug, $"SeaTruck cab is dealer");

                // We hit a supported fish with our SeaTruck cab. Iterate over all Aquarium modules and add the fish to
                // the first one with space
                SeaTruckAquarium[] seaTruckAquariums = rootDealer.GetComponentsInChildren<SeaTruckAquarium>();
                Logger.Log(Logger.Level.Debug, $"Found {seaTruckAquariums.Length} aquarium modules");

                foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
                {
                    if (AddFishToAquarium(seaTruckAquarium, rootTaker))
                    {
                        Logger.Log(Logger.Level.Debug, $"Fish successfully added");
                        return;
                    }
                    else
                    {
                        Logger.Log(Logger.Level.Debug, $"Unable to add fish to this aquarium. Likely full or fish is already in one.");
                    }
                }
            }
        }

        /// <summary>
        /// Is the object hit valid for inclusion in the Aquarium?
        /// </summary>
        /// <param name="colliderGameObject"></param>
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

        /// <summary>
        /// Add our fish to the chosen Aquarium
        /// </summary>
        /// <param name="seaTruckAquarium"></param>
        /// <param name="collidedWith"></param>
        private static bool AddFishToAquarium (SeaTruckAquarium seaTruckAquarium, GameObject auquariumFish)
        {
            Pickupable pickupable = auquariumFish.GetComponent<Pickupable>();
  
            if (seaTruckAquarium.storageContainer.container.HasRoomFor(pickupable))
            {
                global::Utils.PlayFMODAsset(seaTruckAquarium.collectSound, auquariumFish.transform, 20f);
                pickupable.Initialize();
                InventoryItem item = new InventoryItem(pickupable);
                seaTruckAquarium.storageContainer.container.UnsafeAdd(item);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

