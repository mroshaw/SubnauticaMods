using System.Collections;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Utility;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{
    // Define available Pet types, depending on which game the mod is compiled for
#if SUBNAUTICA
    public enum PetCreatureType { CaveCrawler, BloodCrawler, CrabSquid, AlienRobot, Cat }
#endif

#if SUBNAUTICAZERO
    public enum PetCreatureType { SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, BlueTrivalve, YellowTrivalve, Cat }
#endif

    /// <summary>
    /// PetSpawner MonoBehaviour. Allows the spawning of a pet. Can be attached
    /// to a Player, or to a Fabricator / Workbench
    /// </summary>
    internal class PetSpawner : MonoBehaviour
    {
        // Distance above the Fabricator position that pet will be spawned
        public float FabricatorSpawnYAdjustment = 0.8f;

        private bool _skipSpawnObstacleCheck;

        // Added to spawn location to give pet room to spawn
        private readonly float _spawnBuffer = 1.8f;

        private string _spawnFailReason = "";

        private bool _isSpawnerPlayer = false;

        /// <summary>
        /// Public setter for Skip Obstacle Check
        /// </summary>
        public bool SkipSpawnObstacleCheck
        {
            set => _skipSpawnObstacleCheck = value;
        }

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            // Determine if the player is the spawner
            _isSpawnerPlayer = GetComponent<Player>();
        }

        /// <summary>
        /// Spawns a Pet given a TechType
        /// </summary>
        /// <param name="petTechType"></param>
        /// <param name="petName"></param>
        public void SpawnPet(TechType petTechType, string petName)
        {
            SpawnPet(GetPetCreatureType(petTechType), petName);
        }

        /// <summary>
        /// Spawn a pet with current settings
        /// </summary>
        public void SpawnPet(PetCreatureType petType, string petName)
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSpawner: Spawning Pet. Type: {petType}, Name: {petName}");
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSpawner: IsANaughtyBoyThen is {IsANaughtyBoy}");

            // We don't allow Pirates to look after Pets
            if (IsANaughtyBoy)
            {
                ErrorMessage.AddError(Language.main.Get("Alert_NaughtyBoy"));
                return;
            }

            Vector3 desiredSpawnLocation;
            bool spawnSuccess = false;

            // If the player is the spawner, do some checks before spawning
            if (_isSpawnerPlayer)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "PetSpawner: Player is spawner.");
                if (CheckPlayerSpawnConditions())
                {
                    if (GetSpawnLocation(out desiredSpawnLocation))
                    {
                        if (CheckPetSpawnLocation(desiredSpawnLocation) || _skipSpawnObstacleCheck)
                        {
                            LogUtils.LogDebug(LogArea.MonoPets, $"Preparing to Spawn from Player location. Skip obstacle check is {_skipSpawnObstacleCheck}");
                            InstantiatePet(desiredSpawnLocation, petType, petName);
                            spawnSuccess = true;
                        }
                    }
                }
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoPets, "PetSpawner: Player is not spawner. Assume Fabricator.");
                CrafterGhostModel ghostModel = GetComponentInChildren<CrafterGhostModel>();
                desiredSpawnLocation = ghostModel.itemSpawnPoint.position;

                // desiredSpawnLocation = gameObject.transform.position + (Vector3.up * fabricatorSpawnYAdjustment);
                LogUtils.LogDebug(LogArea.MonoPets, "Preparing to Spawn from Fabricator location!");
                InstantiatePet(desiredSpawnLocation, petType, petName);
                spawnSuccess = true;
            }

            // Problem encountered, so log and display a message to user
            if (!spawnSuccess)
            {
                ErrorMessage.AddMessage($"{Language.main.Get("Alert_SpawnError")} {_spawnFailReason}");
                LogUtils.LogDebug(LogArea.MonoPets, $"You can't spawn a pet here! {_spawnFailReason}");
            }
        }

        /// <summary>
        /// Instantiate prefab instance and configure
        /// </summary>
        /// <param name="spawnLocation"></param>
        /// <param name="petType"></param>
        /// <param name="petName"></param>
        private void InstantiatePet(Vector3 spawnLocation, PetCreatureType petType, string petName)
        {
            StartCoroutine(InstantiatePetAsync(spawnLocation, petType, petName));
        }

        /// <summary>
        /// Async method to fetch the prefab by techtype then instantiate a GameObject instance
        /// Adding the Pet component will enable pet behaviours via that component.
        /// </summary>
        /// <param name="spawnLocation"></param>
        /// <param name="petType"></param>
        /// <param name="petName"></param>
        /// <returns></returns>
        private IEnumerator InstantiatePetAsync(Vector3 spawnLocation, PetCreatureType petType, string petName)
        {
            LogUtils.LogDebug(LogArea.MonoPets, $"InstantiatePetAsync called with: Type: {petType}, Name: {petName}");

            yield return null;
            TechType techType = GetPetCreatureTechType(petType);

            if (techType == TechType.None)
            {
                LogUtils.LogDebug(LogArea.MonoPets, "Failed to instantiate! Invalid TechType detected");
                yield break;
            }
            LogUtils.LogDebug(LogArea.MonoPets, "Instantiating Pet...");
            GameObject newCreatureGameObject;

            // Parent the spawn to the current Base interior and set the position and rotation
            GameObject baseGameObject = gameObject.transform.parent.gameObject;
            if (!baseGameObject.GetComponent<Base>())
            {
                Log.LogError("PetSpawner: Pet Fabricator not in a base!!");
            }

            // Custom Types
            if (petType == PetCreatureType.Cat)
            {
                // Get instance from asset bundle
                newCreatureGameObject =
                    ModUtils.GetGameObjectInstanceFromAssetBundle(CatPet.CustomPrefabName, false);
                newCreatureGameObject.transform.SetParent(baseGameObject.transform);
            }
            // Built in types
            else
            {
                CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(techType);
                yield return task;
                GameObject newCreaturePrefab = task.GetResult();
                newCreaturePrefab.SetActive(false);
                newCreatureGameObject = Instantiate(newCreaturePrefab, baseGameObject.transform);
            }

            // Configure the new Pet GameObject
            Pet newPet = PetConfig.ConfigurePet(newCreatureGameObject, petType, petName, baseGameObject);

            // Position and name the spawn GameObject
            newCreatureGameObject.transform.position = spawnLocation;
            newCreatureGameObject.transform.LookAt(Player.main.transform);
            newCreatureGameObject.name = $"{newCreatureGameObject.name}_Pet";

            // Spawning complete. Set the Pet GameObject active
            LogUtils.LogDebug(LogArea.MonoPets, $"Instantiated {petType} at {spawnLocation} as {newCreatureGameObject.name} in parent {newCreatureGameObject.transform.parent.name}");
            ErrorMessage.AddMessage($"{Language.main.Get("Alert_PetBorn")} {newPet.PetTypeString}, {newPet.PetNameString}!");
            newCreatureGameObject.SetActive(true);
            LogUtils.LogDebug(LogArea.MonoPets, "Done!");
        }
        
        /// <summary>
        /// Lookup the TechType from the PetCreatureType
        /// </summary>
        /// <param name="creatureType"></param>
        /// <returns></returns>
        private TechType GetPetCreatureTechType(PetCreatureType creatureType)
        {
            switch (creatureType)
            {
                // Custom pets
                case PetCreatureType.Cat:
                    return PetBuildablePrefab.CatBuildable.Info.TechType;
#if SUBNAUTICA
                case PetCreatureType.CaveCrawler:
                    return TechType.CaveCrawler;
                case PetCreatureType.BloodCrawler:
                    return TechType.Shuttlebug;
                case PetCreatureType.CrabSquid:
                    return TechType.CrabSquid;
                case PetCreatureType.AlienRobot:
                    return TechType.PrecursorDroid;
                default:
                    return TechType.None;
#endif
#if SUBNAUTICAZERO
                case PetCreatureType.SnowstalkerBaby:
                    return TechType.SnowStalkerBaby;
                case PetCreatureType.PenglingBaby:
                    return TechType.PenguinBaby;
                case PetCreatureType.PenglingAdult:
                    return TechType.Penguin;
                case PetCreatureType.Pinnicarid:
                    return TechType.Pinnacarid;
                case PetCreatureType.BlueTrivalve:
                    return TechType.TrivalveBlue;
                case PetCreatureType.YellowTrivalve:
                    return TechType.TrivalveYellow;
                default:
                    return TechType.None;
#endif

            }
        }
        
        /// <summary>
        /// Look up the CreatureType from the TechType
        /// </summary>
        /// <param name="techType"></param>
        /// <returns></returns>
        private PetCreatureType GetPetCreatureType(TechType techType)
        {
            // Custom types
            if (techType == PetBuildablePrefab.CatBuildable.Info.TechType)
            {
                return PetCreatureType.Cat;
            }

            // Build in types
            switch (techType)
            {
#if SUBNAUTICA
                case TechType.CaveCrawler:
                    return PetCreatureType.CaveCrawler;
                case TechType.Shuttlebug:
                    return PetCreatureType.BloodCrawler;
                case TechType.CrabSquid:
                    return PetCreatureType.CrabSquid;
                case TechType.PrecursorDroid:
                    return PetCreatureType.AlienRobot;
                default:
                    return PetCreatureType.CaveCrawler;
#endif
#if SUBNAUTICAZERO
                // SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, BlueTrivalve, YellowTrivalve 
                case TechType.SnowStalkerBaby:
                    return PetCreatureType.SnowstalkerBaby;
                case TechType.PenguinBaby:
                    return PetCreatureType.PenglingBaby;
                case TechType.Penguin:
                    return PetCreatureType.PenglingAdult;
                case TechType.Pinnacarid:
                    return PetCreatureType.Pinnicarid;
                case TechType.TrivalveBlue:
                    return PetCreatureType.BlueTrivalve;
                case TechType.TrivalveYellow:
                    return PetCreatureType.YellowTrivalve;
                default:
                    return PetCreatureType.SnowstalkerBaby;
#endif
            }
        }

        /// <summary>
        /// Determines whether it's possible to spawn a pet at the current point in time
        /// </summary>
        /// <returns></returns>
        private bool CheckPlayerSpawnConditions()
        {
            if(!Player.main.IsInBase())
            {
                _spawnFailReason = Language.main.Get("Alert_PlayerMustBeInBase");
                LogUtils.LogDebug(LogArea.MonoPets, "CheckPlayerSpawnConditions: Player is not in base.");

                return false;
            }
            LogUtils.LogDebug(LogArea.MonoPets, "CheckPlayerSpawnConditions: Spawning conditions met.");
            return true;
        }

        /// <summary>
        /// Checks the surrounding geometry, to see if a spawn is possible
        /// at the requested location.
        /// </summary>
        /// <returns></returns>
        private bool CheckPetSpawnLocation(Vector3 spawnPosition)
        {
            Vector3 fromPosition = MainCameraControl.main.transform.position;
            if (Physics.Linecast(fromPosition, spawnPosition, out RaycastHit hit))
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"CheckPetSpawnLocation: Linecast has hit an object: {hit.collider.gameObject.name}");
                _spawnFailReason = $"{Language.main.Get("Alert_ObjectBlocking")} {hit.collider.gameObject.name}";
                return false;
            }

            LogUtils.LogDebug(LogArea.MonoPets, "CheckPetSpawnLocation: Linecast has found no objects in it's path.");
            return true;
        }

        /// <summary>
        /// Returns a desired spawn position, based on the current camera
        /// </summary>
        /// <returns></returns>
        private bool GetSpawnLocation(out Vector3 spawnPosition)
        {
            // Get Camera and Player transforms
            Transform cameraTransform = MainCameraControl.main.transform;
            Transform playerTransform = Player.main.transform;

            // Move forward, give the pet space to spawn
            Vector3 initialPosition = cameraTransform.position + (cameraTransform.forward * _spawnBuffer);

            // Set the spawn on the ground
            if(GetGroundPosition(initialPosition, out spawnPosition))
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"GetSpawnLocation: Location on ground has been found: {spawnPosition}.");
                return true;
            }

            LogUtils.LogDebug(LogArea.MonoPets, "GetSpawnLocation: Could not find location on ground.");
            return false;
        }

        /// <summary>
        /// Returns a Vector3 indicating a position on the ground directly beneath
        /// the given samplePosition.
        /// </summary>
        /// <param name="samplePosition"></param>
        /// <param name="groundPosition"></param>
        /// <returns></returns>
        private bool GetGroundPosition(Vector3 samplePosition, out Vector3 groundPosition)
        {
            // Get ground height
            if (Physics.Raycast(samplePosition, Vector3.down, out RaycastHit hit, 10.0f))
            {
                groundPosition = hit.point;

                LogUtils.LogDebug(LogArea.MonoPets, $"GetGroundPosition: Raycast hit ground: {groundPosition}.");
                return true;
            }

            LogUtils.LogDebug(LogArea.MonoPets, "GetGroundPosition: Raycast did not hit ground.");
            groundPosition = new Vector3();
            return false;
        }

        /// <summary>
        /// Kills all spawned pets
        /// </summary>
        public void KillAllPets()
        {
            PetUtils.KillAllPets();
        }
    }
}
