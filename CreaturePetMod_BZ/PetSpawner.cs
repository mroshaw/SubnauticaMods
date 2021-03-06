using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Logger = QModManager.Utility.Logger;

namespace MroshawMods.CreaturePetMod_BZ
{
    /// <summary>
    /// Class to support spawning of a new Pet
    /// </summary>
    internal static class PetSpawner
    {
        /// <summary>
        /// Main class for handling pet spawn
        /// </summary>
        internal static bool SpawnCreaturePet()
        {
            // Decide where to spawn. This will be 1m in front of the player, at floor level
            Logger.Log(Logger.Level.Debug, "Getting spawn position");

            // Determine spawn position and rotation
            GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition);

            // First up, we want to check that we can spawn here. Use the toggle combined with IsInterior to decide
            Logger.Log(Logger.Level.Debug, "Checking player position");
            GameObject interiorGameObject = GetPlayerBaseRoom();
            bool isSpawnInBoundary = IsSpawnInBoundary(spawnPosition);

            if (!isSpawnInBoundary)
            {
                Logger.Log(Logger.Level.Debug, "Spawn location too close to wall or object");
                ErrorMessage.AddMessage("Too close to wall or object!");
                return false;
            }

            if (!interiorGameObject && QMod.Config.IndoorPetOnly && !IsSpawnInBoundary(spawnPosition))
            {
                Logger.Log(Logger.Level.Debug, "Not inside and indoor pet only, so won't spawn");
                ErrorMessage.AddMessage("Can't spawn pet outside!");
                return false;
            }

            // Have we reach the limit of pets per room?
            if (NumPetsInRoom(interiorGameObject) >= QMod.Config.MaxPetsPerRoom )
            {
                ErrorMessage.AddMessage("Maximum pets reached!");
                Logger.Log(Logger.Level.Debug, "Reached max pet numbers");
                return false;
            }

            // Call the routine to find the prefab and instantiate the creature
            Logger.Log(Logger.Level.Debug, "Setting up Creature!");
            CoroutineHost.StartCoroutine(SetUpCreaturePet(interiorGameObject, spawnPosition, spawnRotation, QMod.Config.PetType));
            return true;
        }

        /// <summary>
        /// Gets the players interior GameObject, if they are inside
        /// </summary>
        /// <returns></returns>
        private static GameObject GetPlayerBaseRoom()
        {
            IInteriorSpace playerInterior = Player.main.currentInterior;
            if (playerInterior == null)
            {
                Logger.Log(Logger.Level.Debug, "Can't find interior, so must be outside");
                return null;
            }
            // We have an interior object, so let's check we're really in it. No reason why we wouldn't be
            // but checking for "belt and braces" purposes!
            GameObject interiorGameObject = playerInterior.GetGameObject();
            bool isPlayerInside = playerInterior.IsPlayerInside();
            Logger.Log(Logger.Level.Debug, $"Is Player inside: {isPlayerInside}");
            if (!isPlayerInside)
            {
                Logger.Log(Logger.Level.Debug, "Player is not inside");
                return null;
            }
            Logger.Log(Logger.Level.Debug, $"Player is inside: {interiorGameObject.name}");

            return interiorGameObject;
        }

        /// <summary>
        /// Gets the number of creatures currently in the room
        /// </summary>
        /// <param name="interior"></param>
        /// <returns></returns>
        private static int NumPetsInRoom(GameObject interior)
        {
            Logger.Log(Logger.Level.Debug, "Counting room pets");
            CreaturePet[] petsInRoom = interior.GetAllComponentsInChildren<CreaturePet>();
            int alivePetCount = 0;
            foreach (CreaturePet pet in petsInRoom)
            {
                if(pet.IsPetAlive())
                {
                    alivePetCount++;
                }
            }
            Logger.Log(Logger.Level.Debug, "Found: {alivePetCount} live pets");
            return alivePetCount;
        }

        /// <summary>
        /// 
        /// Finds the correct spawn location for the creature based on player / camera position and floor level
        /// </summary>
        /// <param name="spawnRotation"></param>
        /// <param name="spawnPosition"></param>
        private static void GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition)
        {
            // Start with the camera position and rotation
            // Could use player, but mouse can change where the player is "looking" without rotation
            Transform cameraTransform = MainCameraControl.main.transform;
            Transform playerTransform = Player.main.transform;
            spawnPosition = cameraTransform.position;
            // Get the creature looking at the player
            spawnRotation = cameraTransform.rotation * Quaternion.Euler(0, 180f, 0);

            Logger.Log(Logger.Level.Debug, $"Camera position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            // Give a little room to spawn the beastie
            spawnPosition = playerTransform.position + (cameraTransform.forward * 1.2f);
            Logger.Log(Logger.Level.Debug, $"Camera forward position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");

            // Get ground height
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, 10.0f))
            {
                Logger.Log(Logger.Level.Debug, $"Hit position is: {hit.point.x} {hit.point.y} {hit.point.z}");
                spawnPosition = hit.point + new Vector3(0, cameraTransform.localScale.y / 2, 0);
                Logger.Log(Logger.Level.Debug, $"Spawn position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, "Raycast didn't hit. Using player position");
            }
        }

        /// <summary>
        /// Check to see if there is anything between the player and the spawn position
        /// This helps avoid attempts to spawn outside of base objects
        /// </summary>
        /// <param name="spawnPosition"></param>
        /// <returns></returns>
        private static bool IsSpawnInBoundary(Vector3 spawnPosition)
        {
            Vector3 fromPosition = MainCameraControl.main.transform.position;
            Vector3 toPosition = spawnPosition;
            if (Physics.Linecast(fromPosition, toPosition, out RaycastHit hit))
            {
                Logger.Log(Logger.Level.Debug, $"Spawn would be outside boundary. Hit: {hit.collider.gameObject.name}");
                return false;
            }

            Logger.Log(Logger.Level.Debug, "Spawn would be within boundary");
            return true;
        }

        /// <summary>
        /// Sets up our creature Prefab and GameObject
        /// </summary>
        /// <param name="parentBase"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <param name="petType"></param>
        /// <returns></returns>
        private static IEnumerator SetUpCreaturePet(GameObject parentBase, Vector3 spawnPosition, Quaternion spawnRotation, PetCreatureType petType)
        {
            // Setup Prefab
            Logger.Log(Logger.Level.Debug, "In SetUpCreaturePet");

            // Get the TechType for the required Prefab
            TechType petTechType;
            switch (petType)
            {
                case PetCreatureType.SnowstalkerBaby:
                    petTechType = TechType.SnowStalkerBaby;
                    break;
             //   case PetCreatureType.SnowstalkerJuvinile:
             //       petTechType = TechType.SnowStalker;
             //       break;
                case PetCreatureType.PenglingBaby:
                    petTechType = TechType.PenguinBaby;
                    break;
                case PetCreatureType.PenglingAdult:
                    petTechType = TechType.Penguin;
                    break;
                case PetCreatureType.Pinnicarid:
                    petTechType = TechType.Pinnacarid;
                    break;
                default:
                    Logger.Log(Logger.Level.Debug, $"Invalid Pet Choice: {petType}");
                    yield break;
            }

            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(petTechType);
            yield return task;
            GameObject creaturePetPrefab = task.GetResult();
            Logger.Log(Logger.Level.Debug, "Instantiating new GameObject");
            GameObject petCreatureGameObject;

            if (parentBase)
            {
 #pragma warning disable CS0618 // Type or member is obsolete
                petCreatureGameObject = GameObject.Instantiate(creaturePetPrefab, spawnPosition, spawnRotation, parentBase.transform);
 #pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                petCreatureGameObject = GameObject.Instantiate(creaturePetPrefab, spawnPosition, spawnRotation);
            }

            // Set to inactive, while we configure
            petCreatureGameObject.SetActive(false);

            // Do the configuration
            Logger.Log(Logger.Level.Debug, "Adding CreaturePet component...");
            string petName = QMod.Config.PetName.ToString();
            Creature creature = petCreatureGameObject.GetComponent<Creature>();
            CreaturePet creaturePet = petCreatureGameObject.AddComponent<CreaturePet>();
            Logger.Log(Logger.Level.Debug, "Configuring Pet...");
            PetDetails petDetails =  creaturePet.ConfigurePet(QMod.Config.PetType, QMod.Config.PetName.ToString());

            // Set the GameObject to Active
            Logger.Log(Logger.Level.Debug, "Enable Game Object...");
            petCreatureGameObject.SetActive(true);

            // Refresh Actions based on amended Component allocation
            creature.ScanCreatureActions();

            Logger.Log(Logger.Level.Debug, "Adding to HashSet...");
            if (QMod.PetDetailsHashSet == null)
            {
                QMod.PetDetailsHashSet = new HashSet<PetDetails>();

            }
            QMod.PetDetailsHashSet.Add(petDetails);
            Logger.Log(Logger.Level.Debug, "Adding to HashSet... Done");

            // Done
            ErrorMessage.AddMessage($"You have a new pet! Welcome, {petName}");
        }
    }
}
