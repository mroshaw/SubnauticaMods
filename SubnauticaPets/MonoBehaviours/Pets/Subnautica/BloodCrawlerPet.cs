#if SUBNAUTICA
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements BloodCrawler specific Pet functionality
    /// </summary>
    internal class BloodCrawlerPet : Pet
    {

        // Configuration required to build and spawn
        // Pet
        public static string ClassId = "BloodCrawlerPet";
        public static string DisplayName = Language.main.Get("DisplayName_BloodCrawler");
        public static string Description = Language.main.Get("Description_BloodCrawler");
        public static string TextureName = "BloodCrawlerTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "BloodCrawlerDnaSample";
        public static string DnaDisplayName = Language.main.Get("DisplayName_BloodCrawlerDna");
        public static string DnaDescription = Language.main.Get("Description_BloodCrawlerDna");
        public static string DnaTextureName = "BloodCrawlerDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectiable distribution biome data
        public static BiomeData LootDistributionBiomeData = new LootDistributionData.BiomeData
        {
            biome = BiomeType.SafeShallows_Grass,
            count = 10,
            probability = 0.8f
        };

        public static Color PetObjectColor = Color.red;

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

        // Blood Crawler scale factor
        public override float ScaleFactor => 0.3f;

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