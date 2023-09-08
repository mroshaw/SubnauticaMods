using System.Collections;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{
    /// <summary>
    /// PetSpawner MonoBehaviour. Allows the spawning of a pet. Can be attached
    /// to a Player, or to a Fabricator / Workbench
    /// </summary>
    internal class PetSpawner : MonoBehaviour
    {
        // Distance above the Fabricator position that pet will be spawned
        public float fabricatorSpawnYAdjustment = 0.8f;

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
            Log.LogDebug($"PetSpawner: Spawning Pet. Type: {petType}, Name: {petName}");
            Log.LogDebug($"PetSpawner: IsANaughtyBoyThen is {IsANaughtyBoy}");

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
                Log.LogDebug("PetSpawner: Player is spawner.");
                if (CheckPlayerSpawnConditions())
                {
                    if (GetSpawnLocation(out desiredSpawnLocation))
                    {
                        if (CheckPetSpawnLocation(desiredSpawnLocation) || _skipSpawnObstacleCheck)
                        {
                            Log.LogDebug($"Preparing to Spawn from Player location. Skip obstacle check is {_skipSpawnObstacleCheck}");
                            InstantiatePet(desiredSpawnLocation, petType, petName);
                            spawnSuccess = true;
                        }
                    }
                }
            }
            else
            {
                Log.LogDebug("PetSpawner: Player is not spawner. Assume Fabricator.");
                CrafterGhostModel ghostModel = GetComponentInChildren<CrafterGhostModel>();
                desiredSpawnLocation = ghostModel.itemSpawnPoint.position;

                // desiredSpawnLocation = gameObject.transform.position + (Vector3.up * fabricatorSpawnYAdjustment);
                Log.LogDebug("Preparing to Spawn from Fabricator location!");
                InstantiatePet(desiredSpawnLocation, petType, petName);
                spawnSuccess = true;
            }

            // Problem encountered, so log and display a message to user
            if (!spawnSuccess)
            {
                ErrorMessage.AddMessage($"{Language.main.Get("Alert_SpawnError")} {_spawnFailReason}");
                Log.LogDebug($"You can't spawn a pet here! {_spawnFailReason}");
            }
        }

        /// <summary>
        /// Instantiate prefab instance and configure
        /// </summary>
        /// <param name="spawnLocation"></param>
        private void InstantiatePet(Vector3 spawnLocation, PetCreatureType petType, string petName)
        {
            StartCoroutine(InstantiatePetAsync(spawnLocation, petType, petName));
        }

        /// <summary>
        /// Async method to fetch the prefab by techtype then instantiate a GameObject instance
        /// Adding the Pet component will enable pet behaviours via that component.
        /// </summary>
        /// <param name="spawnLocation"></param>
        /// <returns></returns>
        private IEnumerator InstantiatePetAsync(Vector3 spawnLocation, PetCreatureType petType, string petName)
        {
            Log.LogDebug($"InstantiatePetAsync called with: Type: {petType}, Name: {petName}");

            yield return null;
            TechType techType = GetPetCreatureTechType(petType);

            if (techType == TechType.None)
            {
                Log.LogDebug("Failed to instantiate! Invalid TechType detected");
                yield break;
            }
            Log.LogDebug("Instantiating Pet...");
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return task;
            GameObject newCreaturePrefab = task.GetResult();
            GameObject newCreatureGameObject = Instantiate(newCreaturePrefab);

            // Add the Pet component and configure name and type
            Log.LogDebug("Adding Pet component...");
            Pet newPet = PetUtils.AddPetComponent(newCreatureGameObject, petType, petName);
            newPet.PetCreatureType = petType;
            newPet.PetName = petName;

            // Set the position and rotation
            newCreatureGameObject.transform.position = spawnLocation;
            newCreatureGameObject.transform.LookAt(Player.main.transform);
            newCreatureGameObject.name = $"{newCreatureGameObject.name}_Pet";

            // Register pet with the saver
            Log.LogDebug("PetSpawner: Registering new Pet...");
            newPet.RegisterNewPet();
            Log.LogDebug("PetSpawner: Registering new Pet... Done.");

            Log.LogDebug($"Instantiated {petType} at {spawnLocation} as {newCreatureGameObject.name}");
            newPet.Born();
            Log.LogDebug("Done!");

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
                Log.LogDebug("CheckPlayerSpawnConditions: Player is not in base.");

                return false;
            }
            Log.LogDebug("CheckPlayerSpawnConditions: Spawning conditions met.");
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
                Log.LogDebug($"CheckPetSpawnLocation: Linecast has hit an object: {hit.collider.gameObject.name}");
                _spawnFailReason = $"{Language.main.Get("Alert_ObjectBlocking")} {hit.collider.gameObject.name}";
                return false;
            }

            Log.LogDebug("CheckPetSpawnLocation: Linecast has found no objects in it's path.");
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
                Log.LogDebug($"GetSpawnLocation: Location on ground has been found: {spawnPosition}.");
                return true;
            }

            Log.LogDebug("GetSpawnLocation: Could not find location on ground.");
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

                Log.LogDebug($"GetGroundPosition: Raycast hit ground: {groundPosition}.");
                return true;
            }

            Log.LogDebug("GetGroundPosition: Raycast did not hit ground.");
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
