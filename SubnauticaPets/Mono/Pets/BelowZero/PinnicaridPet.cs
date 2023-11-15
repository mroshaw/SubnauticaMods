#if SUBNAUTICAZERO
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets.BelowZero
{
    /// <summary>
    /// Implements Pinicarid specific Pet functionality
    /// </summary>
    internal class PinnicaridPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "PinnicaridPet";
        public static string TextureName = "PinnicaridTexture";
        public static string PrefabGuid = "f9eccfe2-a06f-4c06-bc57-01c2e50ffbe8"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/BZ-PrefabPaths.json
        public static string ModelName = "";
        public static Vector3 ModelScale = new Vector3(1, 1, 1);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 0.5f;

        // Pet DNA
        public static string DnaClassId = "PinnicaridPetDna";
        public static string DnaTextureName = "PinnicaridDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 5, probability = 0.4f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 8, probability = 0.5f},
        };

        public static Color PetObjectColor = Color.blue;

        private static readonly string[] PinnacaridAnims = { "flutter", "beg", "bite", "fidget", "flinch", "call" };

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
        
        // Pinnicard scale factor
        public override Vector3 ScaleFactor => new(1.0f, 1.0f, 1.0f);
    }
}
#endif