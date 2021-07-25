using Logger = QModManager.Utility.Logger;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace CreaturePetMod_BZ
{
    internal static class PetSpawner
    {
        /// <summary>
        /// Main class for handling pet spawn
        /// </summary>
        internal static void SpawnCreaturePet()
        {
            // First up, we want to check that we can spawn here. Use the toggle combined with IsInterior to decide
            Logger.Log(Logger.Level.Debug, $"Checking player position");
            GameObject interiorGameObject = GetPlayerBaseRoom();

            if (!interiorGameObject && QMod.Config.IndoorPetOnly)
            {
                Logger.Log(Logger.Level.Debug, $"Not inside and indoor pet only, so won't spawn");
                return;
            }

            // Have we reach the limit of pets per room?
            if (NumPetsInRoom(interiorGameObject) >= QMod.Config.MaxPetsPerRoom )
            {
                Logger.Log(Logger.Level.Debug, $"Reached max pet numbers");
                return;
            }

            // Next, we want to decide where to spawn. This will be 1m in front of the player, at floor level
            Logger.Log(Logger.Level.Debug, $"Getting spawn position");

            // Determine spawn position and rotation
            GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition);
            CheckNavMesh(spawnPosition);

            // Call the routine to find the prefab and instantiate the creature
            Logger.Log(Logger.Level.Debug, $"Setting up SnowStalkerBaby");
            UWE.CoroutineHost.StartCoroutine(SetUpCreaturePet(interiorGameObject, spawnPosition, spawnRotation, QMod.Config.ChoiceOfPet));
        }

        /// <summary>
        /// Checks the NavMesh state at the proposed Spawn Point
        /// </summary>
        private static void CheckNavMesh(Vector3 spawnPosition)
        {
            Logger.Log(Logger.Level.Debug, $"Looking for NavMesh...");
            bool foundNavMesh = NavMesh.SamplePosition(spawnPosition, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas);
            Logger.Log(Logger.Level.Debug, $"Status of search: {foundNavMesh}, hit: {navMeshHit}");
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
                Logger.Log(Logger.Level.Debug, $"Can't find interior, so must be outside");
                return null;
            }
            // We have an interior object, so let's check we're really in it. No reason why we wouldn't be
            // but checking for "belt and braces" purposes!
            GameObject interiorGameObject = playerInterior.GetGameObject();
            bool isPlayerInside = playerInterior.IsPlayerInside();
            Logger.Log(Logger.Level.Debug, $"Is Player inside: {isPlayerInside}");
            if (!isPlayerInside)
            {
                Logger.Log(Logger.Level.Debug, $"Player is not inside");
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
            Logger.Log(Logger.Level.Debug, $"Counting room pets");
            Creature[] creaturesInRoom = interior.GetAllComponentsInChildren<Creature>();
            Logger.Log(Logger.Level.Debug, $"Found: {creaturesInRoom.Length}");
            return creaturesInRoom.Length;
        }

        /// <summary>
        /// Finds the correct spawn location for the creature based on player position and floor level
        /// </summary>
        /// <param name="spawnRotation"></param>
        /// <param name="spawnPosition"></param>
        private static void GetSpawnLocation(out Quaternion spawnRotation, out Vector3 spawnPosition)
        {
            // Start with the player position and rotation
            Transform playerTransform = Player.main.transform;
            spawnPosition = playerTransform.position;
            spawnRotation = playerTransform.rotation;

            Logger.Log(Logger.Level.Debug, $"Player position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            spawnPosition += playerTransform.forward;
            Logger.Log(Logger.Level.Debug, $"Player forward position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");

            // Get ground height
            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, 5.0f, -1, QueryTriggerInteraction.Ignore))
            {
                Logger.Log(Logger.Level.Debug, $"Hit position is: {hit.point.x} {hit.point.y} {hit.point.z}");
                spawnPosition = hit.point + new Vector3(0, playerTransform.localScale.y / 2, 0);
                Logger.Log(Logger.Level.Debug, $"Spawn position is: {spawnPosition.x} {spawnPosition.y} {spawnPosition.z}");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, $"Raycast didn't hit. Using player position");
            }
        }

        /// <summary>
        /// Sets up our creature Prefab and GameObject
        /// </summary>
        /// <param name="parentBase"></param>
        /// <param name="spawnPosition"></param>
        /// <param name="spawnRotation"></param>
        /// <param name="petChoice"></param>
        /// <returns></returns>
        private static IEnumerator SetUpCreaturePet(GameObject parentBase, Vector3 spawnPosition, Quaternion spawnRotation, PetChoice petChoice)
        {
            // Setup Prefab
            Logger.Log(Logger.Level.Debug, $"In SetUpCreaturePet");
            TechType petTechType;
            switch (petChoice)
            {
                case PetChoice.SnowstalkerBaby:
                    petTechType = TechType.SnowStalkerBaby;
                    break;
                case PetChoice.PenglingBaby:
                    petTechType = TechType.PenguinBaby;
                    break;
                case PetChoice.SeaEmperorBaby:
                    petTechType = TechType.SeaEmperorBaby;
                    break;
                default:
                    Logger.Log(Logger.Level.Debug, $"Invalid Pet Choice: {petChoice}");
                    yield break;
            }

            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(petTechType);
            yield return task;
            GameObject creaturePetPrefab = task.GetResult();
            Logger.Log(Logger.Level.Debug, $"Instantiating new GameObject");
            GameObject petCreatureGameObject;
            // Configure the prefab
            // Logger.Log(Logger.Level.Debug, $"Calling ConfigurePetBehaviour");
            // ConfigurePetBehaviour(QMod.Config.ChoiceOfPet, creaturePetPrefab);

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
            Logger.Log(Logger.Level.Debug, $"Calling ConfigurePetBehaviour");
            ConfigurePetBehaviour(QMod.Config.ChoiceOfPet, petCreatureGameObject);
        }

        /// <summary>
        /// Configure the base pet behaviour, then call the specific pet config
        /// </summary>
        /// <param name="petChoice"></param>
        private static void ConfigurePetBehaviour(PetChoice petChoice, GameObject petCreatureGameObject)
        {
            // Configure default behaviour
            PetBehaviour.ConfigureBasePet(petCreatureGameObject);

            // Now configure appropriate pet behaviour
            switch (petChoice)
            {
                case PetChoice.SnowstalkerBaby:
                    PetBehaviour.ConfigureSnowStalkerBaby(petCreatureGameObject);
                    break;
                case PetChoice.PenglingBaby:
                    PetBehaviour.ConfigurePenglingBaby(petCreatureGameObject);
                    break;
                case PetChoice.SeaEmperorBaby:
                    PetBehaviour.ConfigureSeaEmperorBaby(petCreatureGameObject);
                    break;

                default:
                    Logger.Log(Logger.Level.Debug, $"Invalid Pet Choice: {petChoice}");
                    break;
            }
        }
    }
}
