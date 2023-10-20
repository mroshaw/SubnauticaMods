#if SUBNAUTICAZERO
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class SnowStalkerBabyPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "BabySnowStalkerPet";
        public static string TextureName = "SnowStalkerBabyTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "78d3dbce-856f-4eba-951c-bd99870554e2"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/BZ-PrefabPaths.json
        public static string ModelName = "";
        public static Vector3 ModelScale = new Vector3(1, 1, 1);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 0.5f;

        // Pet DNA
        public static string DnaClassId = "BabySnowStalkerPetDna";
        public static string DnaTextureName = "SnowStalkerBabyDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
        };

        public static Color PetObjectColor = Color.white;

        private static readonly string[] SnowStalkerBabyAnims = { "dryFur", "growl", "bite", "roar", "fidget", "standUpSniff", "standUpHowl" };
        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new Ingredient(TechType.Gold, 3),
                new Ingredient(DnaBuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Snow Stalker Baby scale factor
        public override float ScaleFactor => 1.0f;

        /// <summary>
        /// Override base Awake method
        /// </summary>
        public override void Awake()
        {
            PreventFloatingOnDeath();
            ConfigureMovement();
            base.ConfigureSwimming();
            base.Awake();
        }

        /// <summary>
        /// Override the SnowStalker movement
        /// </summary>
        private void ConfigureMovement()
        {
            SnowStalkerBaby snowStalker = GetComponent<SnowStalkerBaby>();

            // Add a SurfaceMovement component, get that little bugger moving around!
            Log.LogDebug("... Configuring movement components ...");
            OnSurfaceTracker onSurfaceTracker = gameObject.GetComponent<OnSurfaceTracker>();
            WalkBehaviour walkBehaviour = gameObject.GetComponent<WalkBehaviour>();
            OnSurfaceMovement onSurfaceMovement = gameObject.AddComponent<OnSurfaceMovement>();
            MoveOnSurface moveOnSurface = gameObject.GetComponent<MoveOnSurface>();

            // Configure walking and movement components
            onSurfaceMovement.onSurfaceTracker = onSurfaceTracker;
            onSurfaceMovement.locomotion = gameObject.GetComponent<Locomotion>();
            moveOnSurface.onSurfaceMovement = onSurfaceMovement;
            moveOnSurface.moveRadius = 7.0f;
            walkBehaviour.onSurfaceMovement = onSurfaceMovement;
            walkBehaviour.onSurfaceTracker = onSurfaceTracker;
            snowStalker.onSurfaceMovement = onSurfaceMovement;
            Log.LogDebug("... Configuring movement components ... Done");

            // Add Obstacle Avoidance components
            Log.LogDebug("... Configuring AvoidObstaclesOnLand...");
            AvoidObstaclesOnLand avoidObstaclesOnLand = gameObject.AddComponent<AvoidObstaclesOnLand>();
            AvoidObstaclesOnSurface avoidObstaclesOnSurface = gameObject.AddComponent<AvoidObstaclesOnSurface>();
            avoidObstaclesOnLand.creature = snowStalker;
            avoidObstaclesOnSurface.creature = snowStalker;
            avoidObstaclesOnLand.swimBehaviour = walkBehaviour;
            avoidObstaclesOnLand.scanDistance = 0.5f;
            Log.LogDebug("... Configuring AvoidObstaclesOnLand... Done");

            // Configure swim behaviour
            Log.LogDebug("... Configuring SwimRandom and LastTarget...");
            LastTarget lastTarget = gameObject.AddComponent<LastTarget>();
            SwimRandom swimRandom = gameObject.GetComponent<SwimRandom>();
            swimRandom.swimBehaviour = walkBehaviour;
            Log.LogDebug("... Configuring SwimRandom and LastTarget... Done");
        }
    }
}
#endif