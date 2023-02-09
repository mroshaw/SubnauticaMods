using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.CreaturePetMod_BZ.MonoBehaviours;
using UnityEngine;
using UWE;

namespace DaftAppleGames.CreaturePetMod_BZ.ModUtils
{
    /// <summary>
    /// Class to support spawning of a new Pet
    /// </summary>
    internal static class PetUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabId"></param>
        /// <returns></returns>
        internal static PetDetails GetPetDetailsWithPrefabId(string prefabId)
        {
            foreach (PetDetails petDetails in CreaturePetPluginBz.PetDetailsHashSet)
            {
                if (petDetails.PrefabId == prefabId)
                {
                    return petDetails;
                }
            }
            return null;
        }

        /// <summary>
        /// Main class for handling pet spawn
        /// </summary>
        internal static bool SpawnCreaturePet()
        {
            // Decide where to spawn. This will be 1m in front of the player, at floor level
            CreaturePetPluginBz.Log.LogDebug("Getting spawn position");

            // Determine spawn position and rotation
            GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition);

            // First up, we want to check that we can spawn here. Use the toggle combined with IsInterior to decide
            CreaturePetPluginBz.Log.LogDebug("Checking player position");
            GameObject interiorGameObject = GetPlayerBaseRoom();
            bool isSpawnInBoundary = IsSpawnInBoundary(spawnPosition);

            if (!isSpawnInBoundary)
            {
                CreaturePetPluginBz.Log.LogDebug("Spawn location too close to wall or object");
                ErrorMessage.AddMessage("Too close to wall or object!");
                return false;
            }

            // Call the routine to find the prefab and instantiate the creature
            CreaturePetPluginBz.Log.LogDebug("Setting up Creature!");
            CoroutineHost.StartCoroutine(SetUpCreaturePet(interiorGameObject, spawnPosition, spawnRotation, CreaturePetPluginBz.PetTypeConfig.Value));
            return true;
        }

        /// <summary>
        /// Gets the players interior GameObject, if they are inside
        /// </summary>
        /// <returns></returns>
        public static GameObject GetPlayerBaseRoom()
        {
            IInteriorSpace playerInterior = Player.main.currentInterior;
            if (playerInterior == null)
            {
                CreaturePetPluginBz.Log.LogDebug("Can't find interior, so must be outside");
                return null;
            }
            // We have an interior object, so let's check we're really in it. No reason why we wouldn't be
            // but checking for "belt and braces" purposes!
            GameObject interiorGameObject = playerInterior.GetGameObject();
            bool isPlayerInside = playerInterior.IsPlayerInside();
            CreaturePetPluginBz.Log.LogDebug($"Is Player inside: {isPlayerInside}");
            if (!isPlayerInside)
            {
                CreaturePetPluginBz.Log.LogDebug("Player is not inside");
                return null;
            }
            CreaturePetPluginBz.Log.LogDebug($"Player is inside: {interiorGameObject.name}");

            return interiorGameObject;
        }

        /// <summary>
        /// Gets the number of creatures currently in the room
        /// </summary>
        /// <param name="interior"></param>
        /// <returns></returns>
        private static int NumPetsInRoom(GameObject interior)
        {
            CreaturePetPluginBz.Log.LogDebug("Counting room pets");
            CreaturePet[] petsInRoom = interior.GetAllComponentsInChildren<CreaturePet>();
            int alivePetCount = 0;
            foreach (CreaturePet pet in petsInRoom)
            {
                if(pet.IsPetAlive())
                {
                    alivePetCount++;
                }
            }
            CreaturePetPluginBz.Log.LogDebug($"Found: {alivePetCount} live pets");
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

            // Give a little room to spawn the beastie
            CreaturePetPluginBz.Log.LogDebug($"Camera position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            spawnPosition = playerTransform.position + (cameraTransform.forward * 1.2f);
            CreaturePetPluginBz.Log.LogDebug($"Camera forward position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");

            // Get ground height
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, 10.0f))
            {
                CreaturePetPluginBz.Log.LogDebug($"Hit position is: {hit.point.x} {hit.point.y} {hit.point.z}");
                spawnPosition = hit.point + new Vector3(0, cameraTransform.localScale.y / 2, 0);
                CreaturePetPluginBz.Log.LogDebug($"Spawn position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            }
            else
            {
                CreaturePetPluginBz.Log.LogDebug("Raycast didn't hit. Using player position");
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
                CreaturePetPluginBz.Log.LogDebug($"Spawn would be outside boundary. Hit: {hit.collider.gameObject.name}");
                return false;
            }

            CreaturePetPluginBz.Log.LogDebug("Spawn would be within boundary");
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
        internal static IEnumerator SetUpCreaturePet(GameObject parentBase, Vector3 spawnPosition, Quaternion spawnRotation, PetCreatureType petType)
        {
            // Setup Prefab
            CreaturePetPluginBz.Log.LogDebug("In SetUpCreaturePet");

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
                case PetCreatureType.BlueTrivalve:
                    petTechType = TechType.TrivalveBlue;
                    break;
                case PetCreatureType.YellowTrivalve:
                    petTechType = TechType.TrivalveYellow;
                    break;

                default:
                    CreaturePetPluginBz.Log.LogDebug($"Invalid Pet Choice: {petType}");
                    yield break;
            }

            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(petTechType);
            yield return task;
            GameObject creaturePetPrefab = task.GetResult();
            CreaturePetPluginBz.Log.LogDebug("Instantiating new GameObject");
            GameObject petCreatureGameObject;

            if (parentBase)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                CreaturePetPluginBz.Log.LogDebug($"Parent is {parentBase.name}...");
                petCreatureGameObject = GameObject.Instantiate(creaturePetPrefab, spawnPosition, spawnRotation, parentBase.transform);
 #pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                CreaturePetPluginBz.Log.LogDebug("No parent base found...");
                petCreatureGameObject = GameObject.Instantiate(creaturePetPrefab, spawnPosition, spawnRotation);
            }

            // Set to inactive, while we configure
            petCreatureGameObject.SetActive(false);

            // Do the configuration
            CreaturePetPluginBz.Log.LogDebug("Adding CreaturePet component...");

            // Set up the name and type of Pet
            string newPetName = CreaturePetPluginBz.PetNameConfig.Value.ToString();
            PetCreatureType newPetType = CreaturePetPluginBz.PetTypeConfig.Value;

            // Add and configure the new component
            CreaturePet creaturePet = petCreatureGameObject.AddComponent<CreaturePet>();
            CreaturePetPluginBz.Log.LogDebug("Adding CreaturePet component... Done.");
            CreaturePetPluginBz.Log.LogDebug("Configuring Pet...");
            PetDetails petDetails =  creaturePet.ConfigurePet(newPetType, newPetName);
            CreaturePetPluginBz.Log.LogDebug("Configuring Pet... Done.");

            // Set the GameObject to Active
            CreaturePetPluginBz.Log.LogDebug("Enable Game Object...");
            petCreatureGameObject.SetActive(true);
            CreaturePetPluginBz.Log.LogDebug("Enable Game Object... Done.");

            // Update our internal Hashset for loading and saving
            CreaturePetPluginBz.Log.LogDebug("Adding to HashSet...");
            if (CreaturePetPluginBz.PetDetailsHashSet == null)
            {
                CreaturePetPluginBz.PetDetailsHashSet = new HashSet<PetDetails>();

            }
            CreaturePetPluginBz.PetDetailsHashSet.Add(petDetails);
            CreaturePetPluginBz.Log.LogDebug("Adding to HashSet... Done");

            // Set parent, just in case
            petCreatureGameObject.transform.SetParent(parentBase.transform);

            // Done
            ErrorMessage.AddMessage($"You have a new pet! Welcome, {newPetName}!");
        }

        /// <summary>
        /// Kills ALL pets. Use with caution!
        /// </summary>
        internal static void KillAllPets()
        {
            CreaturePet[] creaturePets = GameObject.FindObjectsOfType<CreaturePet>();
            CreaturePetPluginBz.Log.LogDebug($"Killing {creaturePets.Length} pets...");

            // Loop through all Pets and kill them, one by one
            foreach (CreaturePet creaturePet in creaturePets)
            {
                creaturePet.Kill();
            }
        }
    }
}
