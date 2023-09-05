#if SUBNAUTICAZERO
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class PenglingBabyPet : Pet
    {
        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "PenglingBabyPet";
        public static string DisplayName = Language.main.Get("DisplayName_BabyPengling");
        public static string Description = Language.main.Get("Description_BabyPengling");
        public static string TextureName = "PenglingBabyTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "PenglingBabyDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_BabyPenglingDna");
        public static string DnaDescription = Language.main.Get("Description_BabyPenglingDna");
        public static string DnaTextureName = "PenglingBabyDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectiable distribution biome data
        public static LootDistributionData.BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
        };

        public static Color PetObjectColor = Color.magenta;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new Ingredient(TechType.Gold, 3),
                new Ingredient(BuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Baby Pengling scale factor
        public override float ScaleFactor => 1.0f;

        /// <summary>
        /// Add Creature specific components
        /// </summary>
        public override void AddComponents()
        {
            base.AddComponents();
        }

        /// <summary>
        /// Remove Creature specific components
        /// </summary>
        public override void RemoveComponents()
        {
            base.RemoveComponents();
        }

        /// <summary>
        /// Update Creature specific components
        /// </summary>
        public override void UpdateComponents()
        {
            base.UpdateComponents();
        }
    }
}
#endif