using UnityEngine;
using UnityEngine.AI;
namespace DaftAppleGames.SubnauticaPets.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PrefabConfigUtilsBelowZero
    {
        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        public static void CleanNavUpMesh(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "CleanNavUpMesh started...");
            // Remove NavMesh components
            targetGameObject.DestroyComponentsInChildren<MoveOnNavMesh>();
            targetGameObject.DisableComponentsInChildren<NavMeshFollowing>();
            targetGameObject.DisableComponentsInChildren<NavMeshAgent>();
            LogUtils.LogDebug(LogArea.PetConfigUtils, "CleanNavUpMesh done.");
        }

        /// <summary>
        /// Configure Swimming components
        /// </summary>
        /// <param name="targetGameObject"></param>
        public static void ConfigureSwimming(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSwimming started...");
            // Prevent Pet from swimming in interiors   
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... ConfigurePetCreature:  LandCreatureGravity...");
            LandCreatureGravity landCreatureGravity = targetGameObject.GetComponent<LandCreatureGravity>();
            landCreatureGravity.forceLandMode = true;
            landCreatureGravity.enabled = true;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureSwimming done.");
        }

        /// <summary>
        /// Override the SnowStalker movement
        /// </summary>
        public static void ConfigureMovement(GameObject targetGameObject)
        {
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureMovement started...");
            SnowStalkerBaby snowStalker = targetGameObject.GetComponent<SnowStalkerBaby>();

            // Add a SurfaceMovement component, get that little bugger moving around!
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ...");
            OnSurfaceTracker onSurfaceTracker = targetGameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = targetGameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = targetGameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = targetGameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = targetGameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalker.onSurfaceMovement = onSurfaceMovement;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = targetGameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = targetGameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalker;
            avoidObstaclesOnSurface.creature = snowStalker;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            LogUtils.LogDebug(LogArea.PetConfigUtils, "... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = targetGameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = targetGameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            LogUtils.LogDebug(LogArea.PetConfigUtils, "ConfigureMovement started... Done.");
        }
    }
}
