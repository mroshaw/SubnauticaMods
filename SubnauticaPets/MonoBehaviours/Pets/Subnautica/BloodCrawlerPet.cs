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
        public static string TextureName = "BloodCrawlerTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "7ce2ca9d-6154-4988-9b02-38f670e741b8"; // 830a8fa0-d92d-4683-a193-7531e6968042
        public static string ModelName = "";

        // Pet DNA
        public static string DnaClassId = "BloodCrawlerPetDna"; 
        public static string DnaTextureName = "BloodCrawlerDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
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