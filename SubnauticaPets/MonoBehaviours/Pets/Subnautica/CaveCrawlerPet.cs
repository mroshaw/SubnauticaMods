#if SUBNAUTICA
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements CaveCrawler specific Pet functionality
    /// </summary>
    internal class CaveCrawlerPet : Pet
    {
        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "CaveCrawlerPet";
        public static string DisplayName = Language.main.Get("DisplayName_CaveCrawler");
        public static string Description = Language.main.Get("Description_CaveCrawler");
        public static string TextureName = "CaveCrawlerTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "3e0a11f1-e2b2-4c4f-9a8e-0b0a77dcc065";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "CaveCrawlerDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_CaveCrawlerDna");
        public static string DnaDescription = Language.main.Get("Description_CaveCrawlerDna");
        public static string DnaTextureName = "CaveCrawlerDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
        };

        public static Color PetObjectColor = Color.cyan;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 3),
                new CraftData.Ingredient(BuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Cave Crawler scale factor
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