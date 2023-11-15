#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets.Subnautica
{
    /// <summary>
    /// Implements CrabSquid specific Pet functionality
    /// </summary>
    internal class CrabSquidPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "CrabSquidPet";
        public static string TextureName = "CrabSquidTexture";
        public static string PrefabGuid = "4c2808fe-e051-44d2-8e64-120ddcdc8abb"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json
        public static string ModelName = "Crab_Squid"; // Anim on "Crab_Squid"
        public static Vector3 ModelScale = new Vector3(0.07f, 0.07f, 0.07f);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 1.2f;

        // Pet DNA
        public static string DnaClassId = "CrabSquidPetDna";
        public static string DnaTextureName = "CrabSquidDnaStrandTexture";

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite_Barrier, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite_Barrier, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite_Barrier, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite_Scatter, count = 4, probability = 2.0f},
            new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite_Barrier, count = 4, probability = 2.0f},
        };

        public static Color PetObjectColor = Color.blue;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 1),
                new CraftData.Ingredient(TechType.JellyPlant, 1),
                new CraftData.Ingredient(TechType.Salt, 1),
                new CraftData.Ingredient(PetDnaPrefab.CrabSquidDnaPrefab.Info.TechType, 3));
            return recipe;
        }

        // Crab Squid scale factor
        public override Vector3 ScaleFactor => new(0.07f, 0.07f, 0.07f);
    }
}
#endif