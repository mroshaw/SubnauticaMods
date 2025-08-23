using DaftAppleGames.SubnauticaPets.Extensions;
using DaftAppleGames.SubnauticaPets.Pets;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.AI;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PrefabConfigUtilsPlatform
    {
        internal static void RegisterCustomPet(PrefabInfo prefabInfo, string classId, string bundlePrefabName,
            string audioClipName,
            TechType techType, TechType dnaTechType)
        {
            CustomPrefab prefab = new CustomPrefab(prefabInfo);

            GameObject prefabGameObject = CustomAssetBundleUtils.GetObjectFromAssetBundle<GameObject>(bundlePrefabName) as GameObject;

            GameObject model = prefabGameObject.transform.Find("model").gameObject;
            Transform petEyes = prefabGameObject.transform.Find("Eyes");

            // Standard components
            PrefabUtils.AddBasicComponents(prefabGameObject, classId, prefabInfo.TechType, LargeWorldEntity.CellLevel.Medium);
            PrefabUtils.AddConstructable(prefabGameObject, prefabInfo.TechType, ConstructableFlags.Base, model);
            PrefabUtils.AddVFXFabricating(prefabGameObject, "model", -0.2f, 0.9f, new Vector3(0.0f, 0.0f, 0.0f), 0.7f, new Vector3(0.0f, 0.0f, 0.0f));
            prefabGameObject.GetComponent<LargeWorldEntity>().enabled = false;
            MaterialUtils.ApplySNShaders(prefabGameObject);

            // Custom Pet Components
            PrefabConfigUtils.AddPetComponent(prefabGameObject);
            PrefabConfigUtils.AddCustomPetComponents(prefabGameObject, audioClipName, AudioUtils.BusPaths.PlayerSFXs, 10.0f);
            AddBelowZeroCustomPetComponents(prefabGameObject);
            PrefabConfigUtils.AddPetHandTarget(prefabGameObject);
            prefab.SetGameObject(prefabGameObject);

            // Set the recipe, depends on whether in "Adventure" or "Creative" mode.
            RecipeData recipe = null;
            if (ModConfig.ModMode == ModMode.Adventure)
            {
                recipe = new RecipeData(
                new Ingredient(TechType.Gold, 1),
                new Ingredient(TechType.Titanium, 1),
                new Ingredient(TechType.Salt, 1));
            }
            else
            {
                recipe = new RecipeData(new Ingredient(TechType.Titanium, 1));
            }
            CraftingGadget crafting = prefab.SetRecipe(recipe);
            prefab.Register();
        }


        /// <summary>
        /// Cleans up all the NavMesh related components on the Pet Game Object
        /// </summary>
        internal static void CleanNavUpMesh(GameObject targetGameObject)
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
        internal static void ConfigureSwimming(GameObject targetGameObject)
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
        internal static void ConfigureMovement(GameObject targetGameObject)
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

        private static void AddBelowZeroCustomPetComponents(GameObject targetGameObject)
        {
            Log.LogDebug("Setting up Below Zero custom Pet components...");

            Log.LogDebug("OnSurfaceTracker");
            OnSurfaceTracker onSurfaceTracker = targetGameObject.EnsureComponent<OnSurfaceTracker>();
            Log.LogDebug("WorldForces");
            WorldForces worldForces = targetGameObject.EnsureComponent<WorldForces>();
            worldForces.useRigidbody = targetGameObject.GetComponent<Rigidbody>();
            Log.LogDebug("LiveMixin");
            LiveMixin liveMixin = targetGameObject.EnsureComponent<LiveMixin>();
            liveMixin.data = ScriptableObject.CreateInstance<LiveMixinData>();
            liveMixin.data.maxHealth = 50;
            liveMixin.health = 50;
            Log.LogDebug("LandCreatureGravity");
            LandCreatureGravity landGravity = targetGameObject.EnsureComponent<LandCreatureGravity>();
            landGravity.creatureRigidbody = targetGameObject.GetComponent<Rigidbody>();
            landGravity.worldForces = worldForces;
            landGravity.liveMixin = liveMixin;
            landGravity.onSurfaceTracker = onSurfaceTracker;
            landGravity.bodyCollider = targetGameObject.GetComponentInChildren<SphereCollider>(true);

            Log.LogDebug("LargeWorldEntity");
            targetGameObject.GetComponent<LargeWorldEntity>().enabled = false;
        }
    }
}