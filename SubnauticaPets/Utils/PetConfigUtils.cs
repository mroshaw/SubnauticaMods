#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero;
#endif
#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica;
#endif
using System.Collections.Generic;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Pets.Custom;
using UnityEngine;
using UnityEngine.AI;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    internal static class PetConfigUtils
    {
        /// <summary>
        /// Adds the specified child pet class component to the given creature GameObject
        /// based on the given PetCreatureType
        /// </summary>
        /// <param name="creatureGameObject"></param>
        /// <param name="petCreatureType"></param>
        /// <param name="petName"></param>
        /// <param name="parentGameObject"></param>
        /// <returns></returns>
        public static Pet AddPetComponent(GameObject creatureGameObject, PetCreatureType petCreatureType, string petName, GameObject parentGameObject)
        {
            Pet newPet;

            switch (petCreatureType)
            {
                // Custom Types
                case PetCreatureType.Cat:
                    newPet = creatureGameObject.AddComponent<CatPet>();
                    break;
#if SUBNAUTICA
                case PetCreatureType.CaveCrawler:
                    newPet = creatureGameObject.AddComponent<CaveCrawlerPet>();
                    break;
                case PetCreatureType.BloodCrawler:
                    newPet = creatureGameObject.AddComponent<BloodCrawlerPet>();
                    break;
                case PetCreatureType.CrabSquid:
                    newPet = creatureGameObject.AddComponent<CrabSquidPet>();
                    break;
                case PetCreatureType.AlienRobot:
                    newPet = creatureGameObject.AddComponent<AlienRobotPet>();
                    break;
                default:
                    return null;
#endif
#if SUBNAUTICAZERO
                case PetCreatureType.PenglingBaby:
                    newPet = creatureGameObject.AddComponent<PenglingBabyPet>();
                    break;
                case PetCreatureType.PenglingAdult:
                    newPet = creatureGameObject.AddComponent<PenglingAdultPet>();
                    break;
                case PetCreatureType.SnowstalkerBaby:
                    newPet = creatureGameObject.AddComponent<SnowStalkerBabyPet>();
                    break;
                case PetCreatureType.Pinnicarid:
                    newPet = creatureGameObject.AddComponent<PinnicaridPet>();
                    break;
                case PetCreatureType.BlueTrivalve:
                    newPet = creatureGameObject.AddComponent<TrivalveBluePet>();
                    break;
                case PetCreatureType.YellowTrivalve:
                    newPet = creatureGameObject.AddComponent<TrivalveYellowPet>();
                    break;
                default:
                    return null;
#endif
            }

            newPet.ParentBaseGameObject = parentGameObject;
            newPet.PetName  = petName;
            newPet.PetCreatureType = petCreatureType;
            newPet.enabled = true;
            return newPet;
        }

        /// <summary>
        /// Configure the animator component
        /// </summary>
        public static void ConfigureAnimator(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Configuring animator...");
            Creature creature = petGameObject.GetComponent<Creature>();
            Animator animator;
            if (creature)
            {
                animator = creature.GetAnimator();
            }
            else
            {
                LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Couldn't find Creature component. Unable to find Animator.");
                return;
            }

            if (!animator)
            {
                LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: No animator found, so no pet animations will play");
            }
            else
            {
                animator.enabled = true;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Configuring animator... Done.");
        }

        /// <summary>
        /// Adds a Prefab Identifier component
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void AddPrefabIdentifier(GameObject petGameObject)
        {
            PrefabIdentifier prefabIdentifier = petGameObject.GetComponent<PrefabIdentifier>();
            if (!prefabIdentifier)
            {
                petGameObject.AddComponent<PrefabIdentifier>();
            }
        }

        /// <summary>
        /// Adds the SimpleMovement component
        /// </summary>
        public static void AddSimpleMovement(GameObject petGameObject)
        {
            // Add simple movement component
            SimpleMovement movement = petGameObject.GetComponent<SimpleMovement>();
            if (movement == null)
            {
                movement = petGameObject.AddComponent<SimpleMovement>();
                movement.MoveSpeed = 1.0f;
            }
        }

        /// <summary>
        /// Add the WorldForces component
        /// </summary>
        public static void AddWorldForces(GameObject petGameObject)
        {
            WorldForces worldForces = petGameObject.GetComponent<WorldForces>();
            if (worldForces == null)
            {
                worldForces = petGameObject.AddComponent<WorldForces>();
            }
        }

        /// <summary>
        /// Add the PetHandTarget component
        /// </summary>
        public static void AddPetHandTarget(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Adding PetHandTarget component...");
            petGameObject.AddComponent<PetHandTarget>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Adding PetHandTarget component... Done.");
        }

        /// <summary>
        /// Sets the pet scale
        /// </summary>
        public static void SetScale(GameObject petGameObject, Vector3 scaleFactor)
        {
            petGameObject.transform.localScale = scaleFactor;
        }

        /// <summary>
        /// Configure Pet Traits for "friendly" creatures
        /// </summary>
        public static void ConfigurePetTraits(GameObject petGameObject)
        {
            Creature creature = petGameObject.GetComponent<Creature>();
            if (creature)
            {
                creature.Friendliness.Value = 1.0f;
                creature.Happy.Value = 1.0f;
                creature.Aggression.Value = 0.0f;
                creature.Scared.Value = 0.0f;
                creature.Curiosity.Value = 1.0f;
                creature.Hunger.Value = 1.0f;
                creature.Tired.Value = 0.0f;
            }
        }

        /// <summary>
        /// Adds a RigidBody, if not one already
        /// </summary>
        public static void AddRigidBody(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Adding RigidBody component...");
            Rigidbody rigidbody = petGameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = petGameObject.AddComponent<Rigidbody>();
                rigidbody.mass = 0.5f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                rigidbody.isKinematic = false;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Adding RigidBody component... Done.");
        }

        /// <summary>
        /// Configures the Sky and SkyApplier, to ensure
        /// creature mesh shaders don't look "dull".
        /// </summary>
        public static void ConfigureSkyApplier(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Configuring Sky and SkyApplier...");
            Pet pet = petGameObject.GetComponent<Pet>();

            SkyApplier skyApplier = petGameObject.GetComponent<SkyApplier>();
            if (skyApplier)
            {
                GameObject.Destroy(skyApplier);
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier added SkyApplier component.");
            skyApplier = petGameObject.AddComponent<SkyApplier>();

            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier setting SkyApplier Sky.");
            // skyApplier.SetSky(Skies.BaseInterior);

            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier updating renderers...");
            Renderer[] creatureRenderers = petGameObject.GetComponentsInChildren<Renderer>(true);
            LogUtils.LogDebug(LogArea.PetConfigUtils, $"Pet: ConfigureSkyApplier found {creatureRenderers.Length} renderers...");
            // skyApplier.anchorSky = Skies.Auto;
            // skyApplier.emissiveFromPower = false;
            skyApplier.dynamic = false;
            if (creatureRenderers.Length > 0)
            {
                skyApplier.renderers = creatureRenderers;
            }
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: ConfigureSkyApplier updating renderers... Done.");
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Pet: Configuring Sky and SkyApplier... Done.");
        }

        /// <summary>
        /// Prevents a Pet from floating on death
        /// </summary>
        public static void PreventFloatingOnDeath(GameObject petGameObject)
        {
            // Remove the CreatureDeath component, to prevent floating on death
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... ConfigurePetCreature:  CreatureDeath...");
            CreatureDeath creatureDeath = petGameObject.GetComponent<CreatureDeath>();
            GameObject.Destroy(creatureDeath);
        }

        /// <summary>
        /// Remove the given behaviour from the behavior array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="typeToRemove"></param>
        /// <returns></returns>
        private static Behaviour[] RemoveBehaviourItem(Behaviour[] array, System.Type typeToRemove)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, $"... Removing behaviour: {typeToRemove}");
            List<Behaviour> behaviourList = new List<Behaviour>(array);
            Behaviour behaviorToRemove = behaviourList.Find(x => x.GetType() == typeToRemove);
            behaviourList.Remove(behaviorToRemove);
            LogUtils.LogDebug(LogArea.PetConfigUtils, $"... Behaviour removed: {typeToRemove}");
            return behaviourList.ToArray();
        }


#if SUBNAUTICAZERO
        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        public static void CleanNavUpMesh(GameObject petGameObject)
        {
            // Remove NavMesh components
            MoveOnNavMesh navMeshComp = petGameObject.GetComponent<MoveOnNavMesh>();
            if (navMeshComp)
            {
                GameObject.Destroy(navMeshComp);
            }
            else
            {
                LogUtils.LogDebug(LogArea.PetConfigUtils, "... CleanUpMesh: Couldn't find MoveOnNavMesh!");
            }

            NavMeshFollowing navMeshFollowComp = petGameObject.GetComponent<NavMeshFollowing>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... CleanUpMesh: Destroying NavMeshFollowing");
            if (navMeshFollowComp)
            {
                GameObject.Destroy(navMeshFollowComp);
            }
            else
            {
                LogUtils.LogDebug(LogArea.PetConfigUtils, "... CleanUpMesh: Couldn't find NavMeshFollowing!");
            }

            // Destroy the NavMesh Agent
            NavMeshAgent navMeshAgent = petGameObject.GetComponent<NavMeshAgent>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Destroying NavMeshAgent");
            if (navMeshAgent)
            {
                GameObject.Destroy(navMeshAgent);

                // Remove NavMesh behaviour
                LogUtils.LogDebug(LogArea.PetConfigUtils, "... Removing NavMesh Behaviour");
                SwimWalkCreatureController swimWalkCreatureController = petGameObject.GetComponent<SwimWalkCreatureController>();
                swimWalkCreatureController.walkBehaviours = RemoveBehaviourItem(swimWalkCreatureController.walkBehaviours, typeof(NavMeshAgent));
            }
            else
            {
                LogUtils.LogDebug(LogArea.PetConfigUtils, "... CleanUpMesh: Couldn't find NavMeshAgent!");
            }
        }

        /// <summary>
        /// Configure Swimming components
        /// </summary>
        /// <param name="petGameObject"></param>
        public static void ConfigureSwimming(GameObject petGameObject)
        {
            // Prevent Pet from swimming in interiors   
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... ConfigurePetCreature:  LandCreatureGravity...");
            LandCreatureGravity landCreatureGravity = petGameObject.GetComponent<LandCreatureGravity>();
            landCreatureGravity.forceLandMode = true;
            landCreatureGravity.enabled = true;
        }

        /// <summary>
        /// Override the SnowStalker movement
        /// </summary>
        public static void ConfigureMovement(GameObject petGameObject)
        {
            SnowStalkerBaby snowStalker = petGameObject.GetComponent<SnowStalkerBaby>();

            // Add a SurfaceMovement component, get that little bugger moving around!
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ...");
            OnSurfaceTracker onSurfaceTracker = petGameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = petGameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = petGameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = petGameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = petGameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalker.onSurfaceMovement = onSurfaceMovement;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = petGameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = petGameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalker;
            avoidObstaclesOnSurface.creature = snowStalker;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = petGameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = petGameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring SwimRandom and LastTarget... Done");
        }
#endif
#if SUBNAUTICA

        /// <summary>
        /// Destroy the EmpAttack component
        /// </summary>
        public static void DestroyEmpAttack(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Destroying EMPAttack...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(petGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Destroying EMPAttack... Done.");
        }

        /// <summary>
        /// Destroy the AttackLastTarget component
        /// </summary>
        public static void DestroyAttackLastTarget(GameObject petGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Destroying AttackLastTarget...");
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(petGameObject);
            LogUtils.LogDebug(LogArea.PetConfigUtils, "Destroying AttackLastTarget... Done.");
        }
#endif
}
}
