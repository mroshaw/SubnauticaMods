using System;
using System.Collections.Generic;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using UnityEngine.AI;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Allows us to configure standard and pet specific behaviours to our pet instance
    /// </summary>
    static class PetBehaviour
    {

        /// <summary>
        /// Reset creture traits, as these are initialised on Creature.Start()
        /// </summary>
        /// <param name="petCreature"></param>
        internal static void ConfigurePetTraits(Creature petCreature)
        {
            petCreature.Friendliness.Value = 1.0f;
            petCreature.Happy.Value = 1.0f;
            petCreature.Aggression.Value = 0.0f;
            petCreature.Scared.Value = 0.0f;
            petCreature.Curiosity.Value = 1.0f;
            petCreature.Hunger.Value = 1.0f;
            petCreature.Tired.Value = 0.0f;
        }

        /// <summary>
        /// Configure our Pet behaviour based on type
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigurePetCreature(GameObject petCreatureGameObject, PetDetails existingPetDetails)
        {
            // Configure base creature
            
            // Configure base creature behaviours
            Creature petCreature = petCreatureGameObject.GetComponent<Creature>();

            // Set the name for easy debugging
            petCreatureGameObject.name = $"{petCreature.GetType()}(Pet)";

            // Prevent Pet from swimming in interiors   
            LandCreatureGravity landCreatuerGravity = petCreature.gameObject.GetComponent<LandCreatureGravity>();
            landCreatuerGravity.forceLandMode = true;
            landCreatuerGravity.enabled = true;

            // Reconfigure Death to prevent floating
            CreatureDeath creatureDeath = petCreatureGameObject.GetComponent<CreatureDeath>();
            creatureDeath.sink = true;
            // creatureDeath.removeCorpseAfterSeconds = 2.0f;

            // Configure the Pickupable component
            Pickupable pickupable = petCreatureGameObject.GetComponentInParent<Pickupable>();
            if (!petCreatureGameObject.GetComponentInParent<Pickupable>())
            {
                pickupable = petCreatureGameObject.AddComponent<Pickupable>();
            }
            pickupable.isPickupable = false;

            // Add new creaturePet component
            CreaturePet creaturePet = petCreatureGameObject.AddComponent<CreaturePet>() as CreaturePet;
            if (existingPetDetails == null)
            {

                creaturePet.SetPetDetails(QMod.Config.PetName.ToString(), PetUtils.GetCreaturePrefabId(petCreature));
            }
            else
            {
                creaturePet.SetPetDetails(existingPetDetails.PetName, existingPetDetails.PrefabId);
            }
            // Add creature specific config
            string creatureTypeString = petCreature.GetType().ToString();
            switch (creatureTypeString)
            {
                case "SnowStalkerBaby":
                    ConfigureSnowStalkerBaby(petCreatureGameObject);
                    break;

                case "PenguinBaby":
                    ConfigurePenglingBaby(petCreatureGameObject);
                    break;

                case "Penguin":
                    ConfigurePenglingAdult(petCreatureGameObject);
                    break;

                default:
                    Logger.Log(Logger.Level.Debug, $"Invalid Pet Type: {creatureTypeString}");
                    break;
            }

            // Add the pet to our list
            QMod.PetDetailsHashSet.Add(creaturePet.GetPetDetailsObject());
        }

        /// <summary>
        /// Configure Snow Stalker Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        private static void ConfigureSnowStalkerBaby(GameObject petCreatureGameObject)
        {

            /*
              Pengling Components:
                AvoidEdges
                AvoidObstacles
                AvoidObstaclesOnLand
                CreatureDeath
                CreatureFlinch
                GoToPlayerHoldingFish
                LandCreatureGravity
                LastTarget
                LiveMixin
                Locomotion
                MoveOnSurface
                OnSurfaceMovement
                OnSurfaceTracker
                SplineFollowing
                StayAtLeashPosition
                SwimManeuvering
                SwimRandom
                SwimWalkCreatureController
                WalkBehavior
                WorldForces

              SnowStalkerBaby Components:
                AvoidObstacles
                AvoidObstaclesOnLand
                CreatureDeath
                CretureFlinch
                LandCreatureGravity
                LiveMixin
                Locomotion
                MoveOnSurface
                OnSurfaceMovement
                OnSurfaceTracker
                SplineFollowing
                StayAtLeashPosition
                SwimRandom
                SwimWalkCreatureController
                WalkBehaviour
                WorldForces

                -- AvoidObstaclesOnSurface

            */
            SnowStalkerBaby snowStalkerPet = petCreatureGameObject.GetComponent<SnowStalkerBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring SnowStalker: {snowStalkerPet.name}");

            // Switch out the NavMesh with SurfaceMovement
            // Remove NavMesh behaviour
            SwimWalkCreatureController swimWalkCreatureController = petCreatureGameObject.GetComponent<SwimWalkCreatureController>();
            swimWalkCreatureController.walkBehaviours = RemoveBehaviourItem(swimWalkCreatureController.walkBehaviours, typeof(UnityEngine.AI.NavMeshAgent));

            // Clean up the left over NavMesh components
            Logger.Log(Logger.Level.Debug, $"Cleaning up the Mesh");
            CleanUpMesh(petCreatureGameObject);

            // Add a SurfaceMovement component, get that little bugger moving around!
            OnSurfaceTracker onSurfaceTracker = petCreatureGameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = petCreatureGameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = petCreatureGameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = petCreatureGameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = petCreatureGameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalkerPet.onSurfaceMovement = onSurfaceMovement;

            // Add Obstacle Avoidance components
            AvoidObstaclesOnLand avoidObstaclesOnLand = petCreatureGameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = petCreatureGameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalkerPet;
            avoidObstaclesOnSurface.creature = snowStalkerPet;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;

            LastTarget lastTarget = petCreatureGameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = petCreatureGameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;

            // Shake down!
            snowStalkerPet.GetAnimator().SetTrigger("dryFur");
        }

        /// <summary>
        /// Conifugre Pengling Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        private static void ConfigurePenglingBaby(GameObject petCreatureGameObject)
        {
            PenguinBaby penglingPet = petCreatureGameObject.GetComponent<PenguinBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling Baby: {penglingPet.name}");
        }

        /// <summary>
        /// Conifugre Pengling Adult specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        private static void ConfigurePenglingAdult(GameObject petCreatureGameObject)
        {
            Penguin penglingPet = petCreatureGameObject.GetComponent<Penguin>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling: {penglingPet.name}");
        }

        /// <summary>
        /// Used to remove unwanted creature behaviours
        /// </summary>
        /// <param name="array"></param>
        /// <param name="typeToRemove"></param>
        /// <returns></returns>
        private static Behaviour[] RemoveBehaviourItem(Behaviour[] array, Type typeToRemove)
        {
            Logger.Log(Logger.Level.Debug, $"Removing behaviour: {typeToRemove}");
            List<Behaviour> behaviourList = new List<Behaviour>(array);
            Behaviour behaviorToRemove = behaviourList.Find(x => x.GetType() == typeToRemove);
            behaviourList.Remove(behaviorToRemove);
            Logger.Log(Logger.Level.Debug, $"Behaviour removed: {typeToRemove}");
            return (behaviourList.ToArray());
        }

        /// <summary>
        /// Cleans up the NavMesh stuff that we don't want or need
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        private static void CleanUpMesh(GameObject petCreatureGameObject)
        {
            // Remove NavMesh components
            MoveOnNavMesh navMeshComp = petCreatureGameObject.GetComponent<MoveOnNavMesh>();
            Logger.Log(Logger.Level.Debug, $"Destroying MoveOnNavMesh");
            UnityEngine.Object.Destroy(navMeshComp);

            NavMeshFollowing navMeshFollowComp = petCreatureGameObject.GetComponent<NavMeshFollowing>();
            Logger.Log(Logger.Level.Debug, $"Destroying NavMeshFollowing");
            UnityEngine.Object.Destroy(navMeshFollowComp);

            // Destroy the NavMesh Agent
            NavMeshAgent navMeshAgent = petCreatureGameObject.GetComponent<NavMeshAgent>();
            Logger.Log(Logger.Level.Debug, $"Destroying NavMeshAgent");
            UnityEngine.Object.Destroy(navMeshAgent);
        }
    }
}