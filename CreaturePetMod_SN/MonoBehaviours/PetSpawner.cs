using System.Collections;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using DaftAppleGames.CreaturePetModSn.Utils;
using UnityEngine;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace DaftAppleGames.CreaturePetModSn.MonoBehaviours
{
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    internal class PetSpawner : MonoBehaviour
    {
        private PetName _petName;
        private PetCreatureType _petCreatureType;

        // Added to spawn location to give pet room to spawn
        private readonly float _spawnBuffer = 1.8f;

        private string _spawnFailReason = "";

        private bool _isSpawnerPlayer = false;

        /// <summary>
        /// Public setter for Pet Name
        /// </summary>
        public PetName PetName
        {
            set => _petName = value;
        }

        /// <summary>
        /// Public setter for Pet Creature Type
        /// </summary>
        public PetCreatureType PetCreatureType
        {
            set => _petCreatureType = value;
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
        /// Spawns a Pet given a CreatureType
        /// </summary>
        /// <param name="petCreatureType"></param>
        public void SpawnPet(PetCreatureType petCreatureType)
        {
            _petCreatureType = petCreatureType;
            SpawnPet();
        }

        /// <summary>
        /// Spawns a Pet given a TechType
        /// </summary>
        /// <param name="petTechType"></param>
        public void SpawnPet(TechType petTechType)
        {
            SpawnPet(GetPetCreatureType(petTechType));
        }

        /// <summary>
        /// Spawn a pet with current settings
        /// </summary>
        public void SpawnPet()
        {
            Log.LogDebug($"Spawning Pet. Type: {_petCreatureType}, Name: {_petName}");

            Vector3 desiredSpawnLocation;

            // If the player is the spawner, do some checks before spawning
            if (_isSpawnerPlayer)
            {
                Log.LogDebug("PetSpawner: Player is spawner.");
                if (CheckPlayerSpawnConditions())
                {
                    if (GetSpawnLocation(out desiredSpawnLocation))
                    {
                        if (CheckPetSpawnLocation(desiredSpawnLocation))
                        {
                            Log.LogDebug("Preparing to Spawn from Player location!");
                            InstantiatePet(desiredSpawnLocation);
                        }
                    }
                    else
                    {
                        ErrorMessage.AddMessage($"You can't spawn a pet here! {_spawnFailReason}");
                        Log.LogDebug($"You can't spawn a pet here! {_spawnFailReason}");
                    }
                }
                else
                {
                    ErrorMessage.AddMessage($"You can't spawn a pet here! {_spawnFailReason}");
                    Log.LogDebug($"You can't spawn a pet here! {_spawnFailReason}");
                }
            }
            else
            {
                Log.LogDebug("PetSpawner: Player is not spawner. Assume Fabricator.");
                desiredSpawnLocation = gameObject.transform.position + (Vector3.up * 0.8f);
                Log.LogDebug("Preparing to Spawn from Fabricator location!");
                InstantiatePet(desiredSpawnLocation);
            }
        }

        /// <summary>
        /// Instantiate prefab instance and configure
        /// </summary>
        /// <param name="spawnLocation"></param>
        private void InstantiatePet(Vector3 spawnLocation)
        {
            StartCoroutine(InstantiatePetAsync(spawnLocation));
        }

        /// <summary>
        /// Async method to fetch the prefab by techtype then instantiate a GameObject instance
        /// Adding the Pet component will enable pet behaviours via that component.
        /// </summary>
        /// <param name="spawnLocation"></param>
        /// <returns></returns>
        private IEnumerator InstantiatePetAsync(Vector3 spawnLocation)
        {
            yield return null;
            TechType techType = GetPetCreatureTechType(_petCreatureType);

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
            Pet newPet = PetUtils.AddPetComponent(newCreatureGameObject, _petCreatureType, _petName);
            newPet.PetCreatureType = _petCreatureType;
            newPet.PetName = _petName;

            // Set the position and rotation
            newCreatureGameObject.transform.position = spawnLocation;
            newCreatureGameObject.transform.LookAt(Player.main.transform);
            newCreatureGameObject.name = $"{newCreatureGameObject.name}_Pet";

            // Register pet with the saver
            Log.LogDebug("Pet: Registering new Pet...");
            newPet.RegisterNewPet();
            Log.LogDebug("Pet: Registering new Pet... Done.");

            Log.LogDebug($"Instantiated {_petCreatureType} at {spawnLocation} as {newCreatureGameObject.name}");
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
                _spawnFailReason = "Player must be in a base to spawn a pet.";
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
                _spawnFailReason = $"There is blocking the spawn location: {hit.collider.gameObject.name}";
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
            Pet[] allPets = FindObjectsOfType<Pet>();
            foreach (Pet pet in allPets)
            {
                pet.Kill();
            }
        }
    }
}
