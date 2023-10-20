#if SUBNAUTICAZERO

using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class TrivalveBluePet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "TrivalveBluePet";
        public static string TextureName = "TrivalveBlueTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "f5a2317f-6116-4fc6-8e81-824fd8ba9684"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/BZ-PrefabPaths.json
        public static string ModelName = "";
        public static Vector3 ModelScale = new Vector3(1, 1, 1);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 0.5f;

        // Pet DNA
        public static string DnaClassId = "TrivalveBluePetDna";
        public static string DnaTextureName = "TrivalveBlueDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
        };

        public static Color PetObjectColor = Color.blue;

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

        // Trivalve scale factor
        public override float ScaleFactor => 1.0f;

        /// <summary>
        /// Override base Awake method
        /// </summary>
        public override void Awake()
        {
            PreventFloatingOnDeath();
            ConfigureSwimming();
            base.Awake();
        }
    }
}
#endif