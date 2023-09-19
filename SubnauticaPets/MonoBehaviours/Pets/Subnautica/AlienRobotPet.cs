using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

#if SUBNAUTICA
namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class AlienRobotPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "AlienRobotPet";
        public static string TextureName = "AlienRobotTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json
        public static string ModelName = "Precursor_Droid"; // "Precursor_Droid_horn_geo" is mode, Anim on "Precursor_Droid"
        public static Vector3 ModelScale = new Vector3(1, 1, 1);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 1.2f;

        // Pet DNA
        public static string DnaClassId = "AlienRobotPetDna";
        public static string DnaTextureName = "AlienRobotDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Scattered, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Barrier, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Scattered, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite_Barrier, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered, count = 1, probability = 1.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Barrier, count = 1, probability = 1.0f},
        };

        public static Color PetObjectColor = Color.grey;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 3),
                                new CraftData.Ingredient(DnaBuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Alien Robot scale factor
        public override float ScaleFactor => 1.0f;
    }
}
#endif