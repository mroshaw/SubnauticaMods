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
        /// Configure base creature behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigureBasePet(GameObject petCreatureGameObject)
        {
            // Configure base creature behaviours
            Creature creaturePet = petCreatureGameObject.GetComponent<Creature>();
            creaturePet.Friendliness.Value = 1.0f;
            creaturePet.Aggression.Value = 0.0f;
            creaturePet.Scared.Value = 0.0f;
            creaturePet.Hunger.Value = 0.0f;
        }

        /// <summary>
        /// Configure Snow Stalker Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigureSnowStalkerBaby(GameObject petCreatureGameObject)
        {
            SnowStalkerBaby snowStalkerPet = petCreatureGameObject.GetComponent<SnowStalkerBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring SnowStalker: {snowStalkerPet.name}");

            // Remove NavMesh related actions
            // CreatureAction[] allActions = petCreatureGameObject.GetAllComponentsInChildren<CreatureAction>();
            List<CreatureAction> actionsToRemove = new List<CreatureAction>();
            List<CreatureAction> allActions = snowStalkerPet.actions;
            Logger.Log(Logger.Level.Debug, $"Found {allActions.Count} actions");
            foreach (CreatureAction action in allActions)
            {
                Logger.Log(Logger.Level.Debug, $"Found action: {action.GetType()}");

            }

            // Switch out the NavMesh with SurfaceMovement
            // Add a SurfaceMovement component, get that little bugger moving around!
            OnSurfaceTracker onSurfaceTracker = petCreatureGameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = petCreatureGameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = petCreatureGameObject.AddComponent<OnSurfaceMovement>() as OnSurfaceMovement;
            MoveOnSurface moveOnSurface = petCreatureGameObject.GetComponent<MoveOnSurface>();
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = petCreatureGameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            onSurfaceMovement.Start();

            Logger.Log(Logger.Level.Debug, $"Cleaning up the Mesh left over");
            CleanUpMesh(petCreatureGameObject);

              // Shake down!
            snowStalkerPet.GetAnimator().SetTrigger("dryFur");

        }

        internal static void CleanUpMesh(GameObject petCreatureGameObject)
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

        /// <summary>
        /// Conifugre Pengling Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigurePenglingBaby(GameObject petCreatureGameObject)
        {
            PenguinBaby penglingPet = petCreatureGameObject.GetComponent<PenguinBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling: {penglingPet.name}");
        }

        /// <summary>
        /// Conifugre Sea Emperor Baby specific behaviours
        /// </summary>
        /// <param name="petCreatureGameObject"></param>
        internal static void ConfigureSeaEmperorBaby(GameObject petCreatureGameObject)
        {
            SeaEmperorBaby seaEmprorPet = petCreatureGameObject.GetComponent<SeaEmperorBaby>();
            Logger.Log(Logger.Level.Debug, $"Configuring Pengling: {seaEmprorPet.name}");
        }
    }
}
