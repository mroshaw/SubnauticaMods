#if SUBNAUTICAZERO

using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero
{
    /// <summary>
    /// Implements AlienRobot specific Pet functionality
    /// </summary>
    internal class TrivalveBluePet : Pet
    {
        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "TrivalveBluePet";
        public static string DisplayName = Language.main.Get("DisplayName_BlueTrivalve");
        public static string Description = Language.main.Get("Description_BlueTrivalve");
        public static string TextureName = "TrivalveBlueTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "f5a2317f-6116-4fc6-8e81-824fd8ba9684";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "TrivalveBlueDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_BlueTrivalveDna");
        public static string DnaDescription = Language.main.Get("Description_BlueTrivalveDna");
        public static string DnaTextureName = "TrivalveBlueDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static LootDistributionData.BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
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
                new Ingredient(BuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Trivalve scale factor
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