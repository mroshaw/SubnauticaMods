#if SUBNAUTICAZERO

using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class SnowStalkerBabyPet : Pet
    {
        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "SnowStalkerBabyPet";
        public static string DisplayName = Language.main.Get("DisplayName_SnowStalker");
        public static string Description = Language.main.Get("Description_SnowStalker");
        public static string TextureName = "SnowStalkerBabyTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "78d3dbce-856f-4eba-951c-bd99870554e2";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "SnowStalkerBabyDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_SnowStalkerDna");
        public static string DnaDescription = Language.main.Get("Description_SnowStalkerDna");
        public static string DnaTextureName = "SnowStalkerBabyDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static LootDistributionData.BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
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
                new Ingredient(BuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Snow Stalker Baby scale factor
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