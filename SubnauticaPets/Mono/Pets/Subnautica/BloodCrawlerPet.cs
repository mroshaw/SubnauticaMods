#if SUBNAUTICA
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica
{
    /// <summary>
    /// Implements BloodCrawler specific Pet functionality
    /// </summary>
    internal class BloodCrawlerPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "BloodCrawlerPet";
        public static string TextureName = "BloodCrawlerTexture";
        public static string PrefabGuid = "830a8fa0-d92d-4683-a193-7531e6968042"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json
        public static string ModelName = "Cave_Crawler_blood_01"; // Animator on "Cave_Crawler_03"
        public static Vector3 ModelScale = new Vector3(0.3f, 0.3f, 0.3f);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 1.2f;

        // Pet DNA
        public static string DnaClassId = "BloodCrawlerPetDna"; 
        public static string DnaTextureName = "BloodCrawlerDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Rock, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Dunes_TechSite_Barrier, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SeaTreaderPath_TechSite, count = 4, probability = 2.0f},

        };

        public static Color PetObjectColor = Color.red;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 1),
                new CraftData.Ingredient(TechType.AcidMushroom, 1),
                new CraftData.Ingredient(TechType.Salt, 1),
                new CraftData.Ingredient(DnaBuildablePrefabInfo.TechType, 3));
            return recipe;
        }

        // Blood Crawler scale factor
        public override Vector3 ScaleFactor => new(0.3f, 0.3f, 0.3f);
    }
}
#endif