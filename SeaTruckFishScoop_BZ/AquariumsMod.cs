using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace SeaTruckFishScoop_BZ
{
    /// <summary>
    /// Mods to enable additonal controls that can be used within the Seatruck cab
    /// </summary>
    internal static class AquariumsMod
    {

        // Static FMODAsset for playing sounds
        private static readonly FMODAsset SoundsToPlay = ScriptableObject.CreateInstance<FMODAsset>();
        private static readonly string fishScoopPowerOnSoundPath = "event:/sub/cyclops/start";
        private static readonly string fishScoopPowerOffSoundPath = "event:/sub/base/power_off";
        private static readonly string aquariumPurgeSoundPath = "event:/player/bubbles";
        /// <summary>
        /// Toggle the FishScoop on and off
        /// </summary>
        internal static void ToggleFishScoop(bool currentState)
        {

            Logger.Log(Logger.Level.Debug, $"Toggling fish scoop from: {currentState}...");
            QMod.Config.EnableFishScoop = !currentState;
            Logger.Log(Logger.Level.Debug, $"Toggled to: {QMod.Config.EnableFishScoop}");
            SeaTruckMotor mainMotor = Player.main.GetComponentInParent<SeaTruckMotor>();
            if (currentState)
            {
                ErrorMessage.AddMessage($"Fish scoop DISABLED");
                SoundsToPlay.path = fishScoopPowerOffSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, mainMotor.transform.position);
            }
            else
            {
                ErrorMessage.AddMessage($"Fish scoop ENABLED");
                SoundsToPlay.path = fishScoopPowerOnSoundPath;
                FMODUWE.PlayOneShot(SoundsToPlay, mainMotor.transform.position);
            }
        }

        /// <summary>
        /// Iterates across all aquariums and purges into surrounding water
        /// </summary>
        /// <param name="seaTruck"></param>
        internal static void PurgeAllFish()
        {
            // Get the main motor being piloted, and grab a list of aquariums to parse
            SeaTruckMotor mainMotor = Player.main.GetComponentInParent<SeaTruckMotor>();
            if (mainMotor == null)
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find Main Motor!");
                return;
            }
                SeaTruckAquarium[] seaTruckAquariums = mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            if (seaTruckAquariums == null)
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find any Aquariums!");
                return;
            }
            Logger.Log(Logger.Level.Debug, $"Found {seaTruckAquariums.Length} aquarium modules");
            SoundsToPlay.path = aquariumPurgeSoundPath;
            FMODUWE.PlayOneShot(SoundsToPlay, mainMotor.transform.position);
            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                PurgeFishFromAquarium(seaTruckAquarium);
                Logger.Log(Logger.Level.Debug, $"Purged aquarium: {seaTruckAquarium.name}");
            }
            ErrorMessage.AddMessage($"All aquariums purged!");
        }

        private static void PurgeFishFromAquarium(SeaTruckAquarium seaTruckAquarium)
        {
            // Get all creatures / aquariumfish
            ItemsContainer container = seaTruckAquarium.storageContainer.container;

            // Using ToSet() allows us to amend while iterating
            foreach (InventoryItem fishItem in container.ToSet())
            {
                // Release around the SeaTruck
                Pickupable fishPickupable = fishItem.item;
                Transform playerTransform = Player.main.transform;
                Vector3 fishPosition = playerTransform.position + (playerTransform.forward * 3.0f);
                Logger.Log(Logger.Level.Debug, $"Dropping fish at: {fishPosition}");
                fishPickupable.Drop(fishPosition);

                // Remove from aquarium container
                container.RemoveItem(fishPickupable, true);
                Logger.Log(Logger.Level.Debug, $"Removed {fishPickupable.name}");
            }
        }

        internal static bool AddFishToFreeAquarium(SeaTruckMotor seaTruck, GameObject fish)
        {
            // We hit a supported fish with our SeaTruck cab. Iterate over all Aquarium modules and add the fish to
            // the first one with space
            SeaTruckAquarium[] seaTruckAquariums = seaTruck.GetComponentsInChildren<SeaTruckAquarium>();
            Logger.Log(Logger.Level.Debug, $"Found {seaTruckAquariums.Length} aquarium modules");

            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                if (AddFishToAquarium(seaTruckAquarium, fish))
                {
                    string friendlyFishName = fish.name.Replace("(Clone)", "");
                    Logger.Log(Logger.Level.Debug, $"Fish successfully added ({friendlyFishName})");
                    ErrorMessage.AddMessage($"Fish scoop successful! Added {friendlyFishName}");
                    return true;
                }
                else
                {
                    Logger.Log(Logger.Level.Debug, $"Unable to add fish to this aquarium ({seaTruckAquarium.name}). Likely full or fish is already in one.");
                }
            }
            Logger.Log(Logger.Level.Debug, "No free aquariums!");
            ErrorMessage.AddMessage($"Aquariums are full. Fish scoop failed!");
            return false;
        }
  
        /// <summary>
        /// Add our fish to the chosen Aquarium
        /// </summary>
        /// <param name="seaTruckAquarium"></param>
        /// <param name="auquariumFish"></param>
        /// <returns></returns>
        private static bool AddFishToAquarium(SeaTruckAquarium seaTruckAquarium, GameObject auquariumFish)
        {
            Pickupable pickupable = auquariumFish.GetComponent<Pickupable>();

            if (seaTruckAquarium.storageContainer.container.HasRoomFor(pickupable))
            {
                Utils.PlayFMODAsset(seaTruckAquarium.collectSound, auquariumFish.transform, 20f);
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
