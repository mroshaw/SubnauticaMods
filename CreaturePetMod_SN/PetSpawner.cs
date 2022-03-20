using Logger = QModManager.Utility.Logger;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using ECCLibrary;

namespace CreaturePetMod_SN
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
            // Secide where to spawn. This will be 1m in front of the player, at floor level
            Logger.Log(Logger.Level.Debug, $"Getting spawn position");

            // Determine spawn position and rotation
            GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition);

            // First up, we want to check that we can spawn here. Use the toggle combined with IsInterior to decide
            Logger.Log(Logger.Level.Debug, $"Checking player position");
            bool isSpawnInBoundary = IsSpawnInBoundary(spawnPosition);

            if (!isSpawnInBoundary)
            {
                Logger.Log(Logger.Level.Debug, $"Spawn location too close to wall or object");
                ErrorMessage.AddMessage($"Too close to wall or object!");
                return false;
            }

            // Can the Sync prefab spawn
            SpawnCreaturePet(spawnPosition, spawnRotation, QMod.Config.ChoiceOfPet, QMod.Config.PetName.ToString());
            return true;
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
                Logger.Log(Logger.Level.Debug, $"Raycast didn't hit. Using player position");
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
            else
            {
                Logger.Log(Logger.Level.Debug, $"Spawn would be within boundary");
                return true;
            }
        }

        /// <summary>
        /// Spawn our new Pet!
        /// </summary>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <param name="petChoice"></param>
        private static void SpawnCreaturePet(Vector3 spawnPosition, Quaternion spawnRotation, PetCreatureType petChoice, string petName)
        {
            GameObject newPetCreature;
            GameObject prefab;

            Logger.Log(Logger.Level.Debug, $"Spawning: {petChoice}");

            switch (petChoice)
            {   
                case PetCreatureType.DypPenguin:
                    prefab = CraftData.GetPrefabForTechType(QMod.dypTechType);
                    newPetCreature = GameObject.Instantiate(prefab, spawnPosition, spawnRotation);
                    break;
                default:
                    return;
            }
            newPetCreature.SetActive(true);
            ErrorMessage.AddMessage($"You have a new pet! Welcome {petName}");
        }
    }
}
