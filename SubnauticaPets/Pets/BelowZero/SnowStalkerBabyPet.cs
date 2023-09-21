#if SUBNAUTICAZERO

using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Pets.BelowZero
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
    }
}
#endif