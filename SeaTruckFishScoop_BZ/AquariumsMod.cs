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
            
            // If we're not in the SeaTruck, call it a day
            if (!mainMotor)
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find Main Motor!");
                return;
            }

            // Check if we have any aquarium modules attached
            if (!AquariumsMod.IsAquariumAttached(mainMotor))
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find any Aquariums!");
                ErrorMessage.AddMessage($"No aquariums attached!");
                return;
            }

            // Toggle state
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
            if (!mainMotor)
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find Main Motor!");
                return;
            }

            // Check if we have any aquarium modules attached
            if (!IsAquariumAttached(mainMotor))
            {
                Logger.Log(Logger.Level.Debug, $"Couldn't find any Aquariums!");
                ErrorMessage.AddMessage($"No aquariums attached!");
                return;
            }

            // Checks all done, we can purge the modules
            SeaTruckAquarium[] seaTruckAquariums = mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
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

        /// <summary>
        /// Removes all fish from the specified Aquarium module
        /// </summary>
        /// <param name="seaTruckAquarium"></param>
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

        /// <summary>
        /// Returns true if at least one aquarium module is attached to the SeaTruckMotor
        /// </summary>
        /// <param name="mainMotor"></param>
        /// <returns></returns>
        internal static bool IsAquariumAttached(SeaTruckMotor mainMotor)
        {
            SeaTruckAquarium[] seaTruckAquariums = mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            Logger.Log(Logger.Level.Debug, $"Found {seaTruckAquariums.Length} aquarium modules");
            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                Logger.Log(Logger.Level.Debug, "No aquariums found.");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Adds the specified fish to an aquarium attached to the SeaTruckMotor
        /// </summary>
        /// <param name="mainMotor"></param>
        /// <param name="fish"></param>
        /// <returns></returns>
        internal static bool AddFishToFreeAquarium(SeaTruckMotor mainMotor, GameObject fish)
        {
            // We hit a supported fish with our SeaTruck cab. Iterate over all Aquarium modules and add the fish to
            // the first one with space
            SeaTruckAquarium[] seaTruckAquariums = mainMotor.GetComponentsInChildren<SeaTruckAquarium>();
            Logger.Log(Logger.Level.Debug, $"Found {seaTruckAquariums.Length} aquarium modules");

            // Check to see if there are any aquariums
            if (seaTruckAquariums.Length == 0)
            {
                Logger.Log(Logger.Level.Debug, "No aquariums found.");
                return false;
            }

            foreach (SeaTruckAquarium seaTruckAquarium in seaTruckAquariums)
            {
                if (AddFishToAquarium(seaTruckAquarium, fish))
                {
                    string friendlyFishName = GetFriendlyName(fish.name);
                    Logger.Log(Logger.Level.Debug, $"Fish successfully added {fish.name} as {friendlyFishName}");
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
        /// Returns a "user friendly" name for the fish caught
        /// </summary>
        /// <param name="fishName"></param>
        /// <returns></returns>
        private static string GetFriendlyName(string fishName)
        {
            return (fishName.Replace("(Clone)", ""));
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
