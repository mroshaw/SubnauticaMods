using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

#if SUBNAUTICA
using static CraftData;
#endif

#if SUBNAUTICAZERO
using static Ingredient;
#endif

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Custom
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required
    /// <summary>
    /// Template MonoBehaviour class. Use this to add new functionality and behaviours to
    /// the game.
    /// </summary>
    public class CatPet : Pet
    {
        public static string ClassId = "CatPet";
        public static string IconTextureName = "CatTexture";
        public static string CustomPrefabName = "PetCat";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public static string ModelName = "Precursor_Droid"; // "Precursor_Droid_horn_geo" is mode, Anim on "Precursor_Droid"
        public static Vector3 ModelScale = new Vector3(0.5f, 0.5f, 0.5f);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 1.2f;

        // Pet DNA
        public static string DnaClassId = "CatPetDna";
        public static string DnaTextureName = "CatDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrandReef_TechSite, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 5, probability = 0.4f},
        };

        public static Color PetObjectColor = Color.grey;

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

        // Cat pet scale factor
        public override float ScaleFactor => 0.5f;

        /// <summary>
        /// Override the base Pet awake method
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            base.AddSimpleMovement();
        }
    }
}
